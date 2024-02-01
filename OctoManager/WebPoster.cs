using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Windows.Forms;
using PuppeteerSharp;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace OctoManager
{
    public class WebPoster
    {
        private static string _apiToken = "15d5c39426514595830334855833f876";
        private HttpClient _httpClient = new HttpClient();

        private string OctoToken = Environment.GetEnvironmentVariable("OCTO_TOKEN") ?? _apiToken;
        private string OctoApi = "https://app.octobrowser.net/api/v2/automation/profiles";
        private string LocalApi = "http://localhost:58888/api/profiles/start";
        private HttpClient httpClient = new HttpClient();
        private string wsUrl = "";
        private IBrowser browser = null;
        private IPage page = null;
        private static List<MyOctoData> Profiles = new List<MyOctoData>();
        public delegate void UpdateRichTextBoxDelegate(string text);

        public static async Task Initing()
        {
            await GetProfiles();
        }


        public async Task Post(MyPost post,MyModel model,UpdateRichTextBoxDelegate updateRichTextBox,int postCounter)
        {
            DateTime now = DateTime.Now;

            if (now.Year >= post.TimeUploaded.Year && now.Month >= post.TimeUploaded.Month && now.Day > post.TimeUploaded.Day)
            {
                updateRichTextBox($"[{postCounter}]---------------\n");
                MyOctoData profile = GetProfileByTitle(post.AntiDetectProfile);

                wsUrl = await GetCdp(profile.Profile["title"].ToString());

                await new BrowserFetcher().DownloadAsync();
                browser = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = wsUrl });
                try
                {
                    var pages = await browser.PagesAsync();
                    page = pages[0]; // Взяти першу вкладку

                    var url = post.URL;
                    await page.GoToAsync(url);

                    string joinButtonXPath = "//button[contains(@class,'_1LHxa-yaHJwrPK8kuyv_Y4 _2iuoyPiKHN3kfOoeIQalDT _10BQ7pjWbeYP63SAPNS8Ts HNozj_dKjQZ59ZsfEegz8 _34mIRHpFtnJ0Sk97S2Z3D9')]";
                    var buttonJoinHandles = await page.XPathAsync(joinButtonXPath);

                    if (buttonJoinHandles.Length > 0)
                    {
                        var buttonJoinHandle = buttonJoinHandles[0];
                        string buttonText = await page.EvaluateFunctionAsync<string>("element => element.textContent", buttonJoinHandle);
                        string joinTextLower = buttonText.ToLower();
                        if (joinTextLower == "join")
                        {
                            updateRichTextBox("Приєднуємось до сабредіту" + "\n");
                            await buttonJoinHandle.ClickAsync();
                        }
                    }

                    string createPostButtonXPath = "//input[@class='zgT5MfUrDMC54cpiCpZFu']";
                    var buttonCreatePostHandles = await page.XPathAsync(createPostButtonXPath);

                    if (buttonCreatePostHandles.Length > 0)
                    {
                        updateRichTextBox("Створити пост" + "\n");
                        await buttonCreatePostHandles[0].ClickAsync();
                    }

                    string buttonMediaContentXPath = "//button[@class='Z1w8VkpQ23E1Wdq_My3U4 ']";

                    var buttonMediaContentHandles = await page.XPathAsync(buttonMediaContentXPath);

                    if (buttonMediaContentHandles.Length > 0)
                    {
                        await buttonMediaContentHandles[0].ClickAsync();
                        // Додаткові дії після кліку, якщо потрібно
                    }
                    await RandomPauseAsync();
                    string postTitleXPath = "//textarea[@placeholder='Title']";
                    var postTitleHandles = await page.XPathAsync(postTitleXPath);

                    if (postTitleHandles.Length > 0)
                    {
                        updateRichTextBox("Заповнюємо заголовок" + "\n");
                        await postTitleHandles[0].TypeAsync(post.Title);
                        // Додаткові дії після введення тексту, якщо потрібно
                    }
                    else
                    {
                        //MessageBox.Show("Не знайдено Title Element");
                    }

                    string fileInputXPath = "//input[@type='file']";
                    var fileInputs = await page.XPathAsync(fileInputXPath);

                    if (fileInputs.Length > 0)
                    {
                        updateRichTextBox("Вивантажуємо контент\n");
                        await fileInputs[0].UploadFileAsync(post.FileName);
                        await Task.Delay(10000);
                        // Додаткові дії після завантаження файлу, якщо потрібно
                    }

                    string flair = post.Flair;

                    if (flair != "")
                    {
                        string buttonPostFlairXPath = "//div[contains(@class, 'Nb7NCPTlQuxN_WDPUg5Q2')][contains(.,'Add flair')]";
                        var buttonPostFlairHandles = await page.XPathAsync(buttonPostFlairXPath);

                        if (buttonPostFlairHandles.Length > 0)
                        {
                            await buttonPostFlairHandles[0].ClickAsync();
                            await RandomPauseAsync();

                            string postFlairXPath = $"//span[contains(.,'{flair}')]";
                            var postFlairHandles = await page.XPathAsync(postFlairXPath);

                            if (postFlairHandles.Length > 0)
                            {
                                updateRichTextBox("Присвоюємо флаєр\n");
                                await postFlairHandles[0].ClickAsync();
                                await RandomPauseAsync();

                                string postFlairApplyButtonXPath = "//button[@class='_2iuoyPiKHN3kfOoeIQalDT _10BQ7pjWbeYP63SAPNS8Ts HNozj_dKjQZ59ZsfEegz8 ']";
                                var postFlairApplyButtonHandles = await page.XPathAsync(postFlairApplyButtonXPath);

                                if (postFlairApplyButtonHandles.Length > 0)
                                {
                                    await postFlairApplyButtonHandles[0].ClickAsync();
                                    Console.WriteLine("Флаєр обрано!\n");
                                }
                            }
                        }
                    }
                    await RandomPauseAsync();

                    string postMakePostXPath = "//button[contains(@class, '_18Bo5Wuo3tMV-RDB8-kh8Z')][contains(@class, '_2iuoyPiKHN3kfOoeIQalDT')][contains(@class, '_10BQ7pjWbeYP63SAPNS8Ts')][contains(@class, 'HNozj_dKjQZ59ZsfEegz8')]";
                    var postMakePostHandles = await page.XPathAsync(postMakePostXPath);

                    if(model.Run)
                    {
                        if (postMakePostHandles.Length > 0)
                        {
                            await RandomPauseAsync();
                            updateRichTextBox("Пост створено\n");
                            post.TimeUploaded = DateTime.Today;
                            model.PostCreated++;
                            await postMakePostHandles[0].ClickAsync();
                        }
                        else
                        {
                            updateRichTextBox("Пост не створено\n");
                            model.PostNotCreated++;
                        }
                    }
                    else
                    {
                        updateRichTextBox(model.Name + " Зупинена\n");
                    }

                    await RandomPauseAsync();

                    string errorMessageXPath = "//div[@class='Nt8TnDvJ2BsL8KWcFQKy5']";

                    // Знаходження елементів за XPath
                    var elements = await page.XPathAsync(errorMessageXPath);

                    if (elements.Length > 0)
                    {
                        string actionbuttonXPath = "(//i[contains(@class,'_38GxRFSqSC-Z2VLi5Xzkjy icon icon-overflow_horizontal')])[1]";

                        // Знаходження кнопки за XPath
                        var actionButton = await page.XPathAsync(actionbuttonXPath);

                        if (actionButton.Length > 0)
                        {
                            // Клік на першій кнопці
                            await actionButton[0].ClickAsync();
                        }

                        await RandomPauseAsync();

                        string deleteButtonXPath = "//span[@class='_2-cXnP74241WI7fpcpfPmg'][contains(.,'delete')]";

                        // Чекаємо, поки елемент з'явиться на сторінці
                        await page.WaitForXPathAsync(deleteButtonXPath);

                        // Знаходження кнопки
                        var deleteButton = await page.XPathAsync(deleteButtonXPath);

                        if (deleteButton.Length > 0)
                        {
                            // Клік на кнопці
                            await deleteButton[0].ClickAsync();
                        }
                    }

                    await RandomPauseAsync();
                }
                catch (Exception ex)
                {
                    updateRichTextBox("ERROR####### " + ex.Message + "\n");
                }
            }
        }

        public static MyModel GetModelByName(string name)
        {
            MyModel modelReturn = null;

            foreach(var model in Data.Models)
            {
                if(model.Name == name)
                {
                    modelReturn = model;
                    break;
                }
            }

            return modelReturn;
        }

        private static async Task GetProfiles()
        {
            if (Profiles.Count <= 0)
            {
                string apiUrl = "https://app.octobrowser.net/api/v2/automation/profiles";
                int pageLen = 100;
                int page = 0;
                string fullUrl = $"{apiUrl}?page_len={pageLen}&page={page}&fields=title,description,proxy,start_pages,tags,status,last_active,version,storage_options,created_at,updated_at&ordering=active";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-Octo-Api-Token", _apiToken);

                    try
                    {
                        var response = await httpClient.GetAsync(fullUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            var json = JObject.Parse(content);
                            var data = json["data"]?.ToObject<List<JObject>>();
                            if (data != null)
                            {
                                for (int d = 0; d < data.Count; d++)
                                {
                                    MyOctoData myOctoData = new MyOctoData();
                                    myOctoData.Profile = data[d];
                                    myOctoData.WsUrl = "";
                                    Profiles.Add(myOctoData);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex}");
                    }
                }
            }
        }

        private MyOctoData GetProfileByTitle(string title)
        {
            MyOctoData result = new MyOctoData();
            for (int p = 0; p < Profiles.Count; p++)
            {
                if (Profiles[p].Profile["title"].ToString() == title)
                {
                    result = Profiles[p];
                    break;
                }
            }
            return result;
        }
        private async Task<string> GetCdp(string profileTargetTitle)
        {
            string result = "";
            string uuid = "";
            for(int p =0;p < Profiles.Count; p++)
            {
                if (Profiles[p].Profile["title"].ToString() == profileTargetTitle)
                {
                    uuid = Profiles[p].Profile["uuid"].ToString();

                    if (string.IsNullOrEmpty(Profiles[p].WsUrl))
                    {
                        var resp = await httpClient.PostAsJsonAsync(LocalApi, new { uuid, debug_port = true });
                        var respContent = await resp.Content.ReadAsStringAsync();
                        var respJson = JObject.Parse(respContent);

                        result = respJson["ws_endpoint"].ToString();
                        Console.WriteLine($"Start profile resp: {respJson}");
                        Profiles[p].WsUrl = result;
                    }
                    else
                    {
                        result = Profiles[p].WsUrl;
                    }
                }
            }

            return result;
        }

        private async Task RandomPauseAsync()
        {
            int min = 3000;
            int max = 3000;

            Random random = new Random();
            int pauseDuration = random.Next(min, max); // Генерує випадкове число від 500 до 5000 мілісекунд
            await Task.Delay(pauseDuration);
        }
    }
}
