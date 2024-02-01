using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Color = System.Drawing.Color;
using System.Configuration;

namespace OctoManager
{
    public partial class Form1 : Form
    {
        public TreeNode CurrentModelsNode;

        public string CurrentContentText;

        public Form1()
        {
            InitializeComponent();

            axWindowsMediaPlayer1.Hide();
            LoadModels();
            CreateRootNodes();
            LoadProfilesTagsSubTags();
            UpdateModels();
            ShowEdit(false);
            Initing();
        }

        private async Task Initing()
        {
            await WebPoster.Initing();
        }

        public void UpdateRichTextBox(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.Text += text));
            }
            else
            {
                richTextBox1.Text += text;
            }
        }

        private void LoadProfilesTagsSubTags()
        {
            LoadAntiDetectProfiles(true);
        }

        public static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private List<MyPost> GetPostByFrequency(List<MyPost> posts,int count)
        {
            List<MyPost> result = new List<MyPost>();
            if(posts != null)
            {
                result = posts.Where(post => TimeSpanded(post))
              .Take(count)
              .ToList();
            }
            return result;
        }

        private async void buttonRunModel_Click(object sender, EventArgs e)
        {
            await LoadModelsAsync();
            MyModel model = GetSelectedModel();

            if(model != null)
            {
                model.Run = true;
                timerButtonActivate.Enabled = false;
                await GetModelDaylyPosts(model, 1000);
                labelPostTotalCount.Text = "Загальна кількість можливих постів: " + model.DayPostList.Count; // FAKE CODE FIX IT!
                //Post
                int currentPostIndex = 0;

                int postCounter = 0;
                for (int p = 0; p < model.DayPostList.Count; p++)
                {
                    MessageBox.Show("Модель " + model.Run.ToString());
                    if (model.Run == false)
                    {
                        break;
                    }
                    MyPost post = model.DayPostList[p];

                    WebPoster poster = new WebPoster();
                    model.Status = "Постимо";
                    await poster.Post(post,model,UpdateRichTextBox, postCounter);
                    postCounter++;
                    if (model != null)
                    {
                        if (currentPostIndex >= model.PostPerPause - 1)
                        {
                            model.Status = "Пауза";
                            await Task.Delay(TimeSpan.FromSeconds(model.PostPause));
                            currentPostIndex = 0;
                        }
                        else
                        {
                            currentPostIndex++;
                        }
                    }
                    await SaveModelsAsync();
                }
                //Post
            }
            
            timerButtonActivate.Enabled = true;
        }

        private async Task GetModelDaylyPosts(MyModel model,int count)
        {
            List<MyPost> result = new List<MyPost>();

            LoadModels();
            richTextBox1.AppendText("Створюємо список постів..Зачекайте\n");
            ResetAntiDetect();
            if (model != null)
            {
                model.PostNotCreated = 0;
                model.PostCreated = 0;
                model.PostTotal = 1000;
                for (int c = 0; c < model.Content.Count; c++)
                {
                    MyContent content = model.Content[c];
                    if (content.Tags.Count > 0 || content.SubTags.Count > 0)
                    {
                        if (content.Posts.Count == 0)
                        {
                            var contentPostsNotConfirmed = content.GetPosts();
                            List<MyPost> contentPostsConfirmed = new List<MyPost>();
                            foreach (var post in contentPostsNotConfirmed)
                            {
                                post.MyModelName = model.Name;
                                post.AntiDetectProfile = content.AntiDetectProfile;
                                post.FileName = content.FileName;
                                contentPostsConfirmed.Add(post);
                            }
                            content.Posts = contentPostsConfirmed;
                        }
                        result.AddRange(content.Posts);
                    }
                }

                Shuffle(result);
                model.DayPostList = (GetPostByFrequency(result,count));
                int countFrequency = 0;
                foreach (var post in model.DayPostList)
                {
                    richTextBox1.AppendText($"{countFrequency} URL " + post.URL + " Tag " + post.Tag + ";SubTag " + post.SubTag + ";Filename " + post.FileName + "\n");
                    countFrequency++;
                }
                model.Run = true;
            }
            SaveModels();
        }

        private bool TimeSpanded(MyPost post)
        {
            DateTime now = DateTime.Now;

            // Перевіряємо, чи поточна дата є пізнішою за дату завантаження плюс частоту публікацій
            if (now > post.TimeUploaded.AddDays(post.PostingFrequency))
            {
                return true;
            }

            return false;
        }

        private void LoadAntiDetectProfiles(bool load)
        {
            if (load)
            {
                var result = Task.Run(GetProfilesAsync).GetAwaiter().GetResult();

                for (int i = 0; i < Data.Profiles.Count; i++)
                {
                    checkedListBoxProfiles.Items.Add(Data.Profiles[i]);
                }
            }
            else
            {
                for (int i = 1; i < 5; i++)
                {
                    checkedListBoxProfiles.Items.Add("Профіль " + i);
                }
            }
        }

        public async Task<bool> GetProfilesAsync()
        {
            // Вказання шляху до конфігураційного файлу вручну
            string configFilePath = Path.Combine(Data.ModelsDirectoryPath,"app.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFilePath
            };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            // Отримання значень з конфігураційного файлу
            string antiDetectApi = config.AppSettings.Settings["ADetectApi"].Value;

            bool result = false;
            // Ваш API-токен, який ви отримаєте під час авторизації
            string apiToken = antiDetectApi;

            // URL для отримання профілів
            string apiUrl = "https://app.octobrowser.net/api/v2/automation/profiles";

            // Параметри запиту
            int pageLen = 100;
            int page = 0;

            // Побудова URL з параметрами
            string fullUrl = $"{apiUrl}?page_len={pageLen}&page={page}&fields=title,description,proxy,start_pages,tags,status,last_active,version,storage_options,created_at,updated_at&ordering=active";

            // Створення клієнта HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Додавання заголовка з API-токеном
                client.DefaultRequestHeaders.Add("X-Octo-Api-Token", apiToken);

                try
                {
                    // Виконання GET-запиту і отримання відповіді
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    // Перевірка статусу відповіді
                    if (response.IsSuccessStatusCode)
                    {
                        // Отримання та обробка вмісту відповіді
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse JSON response using Newtonsoft.Json
                        JObject jsonResponse = JObject.Parse(responseBody);

                        // Access the "data" array
                        JArray dataArray = (JArray)jsonResponse["data"];

                        // Check if there are any profiles in the array
                        if (dataArray.Count > 0)
                        {
                            // Iterate through all profiles in the array
                            foreach (JObject profile in dataArray)
                            {
                                // Access the "title" property
                                string title = (string)profile["title"];

                                // Output the title
                                Data.Profiles.Add(title);
                            }
                        }


                    }
                    else
                    {
                        Data.Profiles.Add($"Помилка: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Data.Profiles.Add($"Виникла помилка: {ex.Message}");
                }
            }
            return result;
        }

        private void ResetAntiDetect()
        {
            MyModel model = GetSelectedModel();

            if (model != null && model.Profiles.Count > 0 && model.Content.Count > 0)
            {
                int totalContent = model.Content.Count;
                int totalAntidetectProfiles = model.Profiles.Count;

                if (totalAntidetectProfiles == 0)
                {
                    //richTextBox1.AppendText("Немає доступних антидетекторних профілів.\n");
                    return;
                }

                int contentPerProfile = totalContent / totalAntidetectProfiles;

                int currentProfile = 0;

                for (int c = 0; c < totalContent; c++)
                {
                    model.Content[c].AntiDetectProfile = model.Profiles[currentProfile];
                    //richTextBox1.AppendText(model.Content[c].AntiDetectProfile + "\n");

                    if ((c + 1) % contentPerProfile == 0)
                    {
                        currentProfile++;
                        if (currentProfile >= totalAntidetectProfiles)
                        {
                            currentProfile = 0; // Повторна індексація профілів, якщо закінчились
                        }
                    }
                }
            }
            else
            {
                //richTextBox1.AppendText("Немає доступного контенту або антидетекторних профілів.\n");
            }
        }

        private void LoadTags(bool load,MyModel model)
        {
            List<string> tempTags = new List<string>();
            List<string> Tags = new List<string>();
            string filePath = model.TagsFileName;
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }

            if (load)
            {
                tempTags = Data.ReadExcelFileLineByLine("output.xlsx", "Sheet1",Data.ColumnNameTag);

                foreach (var tag in tempTags)
                {
                    // Перевірте, чи містить рядок кому
                    if (tag.Contains(","))
                    {
                        // Розділіть кожен елемент на слова за пробільними та комами
                        string[] words = tag.Split(new char[] { ' ', ',', }, StringSplitOptions.RemoveEmptyEntries);

                        // Додайте слова до списку тегів
                        Tags.AddRange(words);
                    }
                    else
                    {
                        // Якщо немає коми, додайте рядок як є
                        Tags.Add(tag);
                    }
                }

                // Очистіть тег від повторюючихся слів
                Tags = Tags.Distinct().ToList();
            }

            string fileName = Path.Combine(Data.ModelsDirectoryPath,model.Name + "Tags");

            model.TagsFileName = fileName;
            // Використання File.WriteAllLines для перезапису файлу зі списком тегів
            File.WriteAllLines(fileName, Tags);

            for (int i = 0; i < Tags.Count; i++)
            {
                checkedListBoxTags.Items.Add(Tags[i]);
            }
        }

        private void LoadSubTags(bool load,MyModel model)
        {
            List<string> tempSubTags = new List<string>();
            List<string> SubTags = new List<string>();

            string filePath = model.SubTagsFileName;
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, string.Empty);
            }
            if (load)
            {
                tempSubTags = Data.ReadExcelFileLineByLine("output.xlsx", "Sheet1",Data.ColumnNameSubtag);

                foreach (var tag in tempSubTags)
                {
                    // Перевірте, чи містить рядок кому
                    if (tag.Contains(","))
                    {
                        // Розділіть кожен елемент на слова за пробільними та комами
                        string[] words = tag.Split(new char[] { 'Ї', ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Додайте слова до списку тегів
                        SubTags.AddRange(words);
                    }
                    else
                    {
                        // Якщо немає коми, додайте рядок як є
                        SubTags.Add(tag);
                    }
                }

                // Очистіть тег від повторюючихся слів
                SubTags = SubTags.Distinct().ToList();
            }


            string fileName = Path.Combine(Data.ModelsDirectoryPath,model.Name + "SubTags");

            model.SubTagsFileName = fileName;
            // Використання File.WriteAllLines для перезапису файлу зі списком тегів
            File.WriteAllLines(fileName, SubTags);

            for (int i = 0; i < SubTags.Count; i++)
            {
                checkedListBoxSubTags.Items.Add(SubTags[i]);
            }
        }

        private bool IsContentExtension(string extension)
        {
            string[] contentExtensions = { ".jpg", ".jpeg", ".png", ".bmp",".mp4", ".avi", ".mkv", ".mov", ".gif" };
            return contentExtensions.Contains(extension);
        }

        // You can add a method to generate thumbnails for video files
        private Image GenerateThumbnail(string videoPath)
        {
            // Implement thumbnail generation logic for video here
            // Return the generated thumbnail image
            return null;
        }

        private void LoadContent(string path)
        {
            MyModel model = GetSelectedModel();

            if (model != null && path != "")
            {
                // Check if the directory exists
                if (Directory.Exists(model.ContentFolder))
                {
                    // Get all image files from the directory (you can specify the file extensions you want)
                    string[] photoVideoExtensions = new string[]
                    {
                        "*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", // Формати фото
                        "*.mp4", "*.avi", "*.mkv", "*.mov", // Формати відео
                        // Додайте інші розширення, які вас цікавлять
                    };

                    string[] contentFiles = new string[0]; // Початковий пустий масив

                    string contentFolder = model.ContentFolder; // Замініть це на ваш шлях до папки

                    foreach (string extension in photoVideoExtensions)
                    {
                        string[] filesWithExtension = Directory.GetFiles(contentFolder, extension);
                        contentFiles = contentFiles.Concat(filesWithExtension).ToArray();
                    }

                    List<MyContent> myContents = new List<MyContent>();
                    for (int i = 0; i < contentFiles.Length; i++)
                    {
                        MyContent content = new MyContent(contentFiles[i],"",-1);
                        myContents.Add(content);
                    }
                    // Add the image file paths to the Photos list
                    model.Content.AddRange(myContents);
                    myContents.Clear();
                }
                else
                {
                    // Handle the case where the directory doesn't exist
                    MessageBox.Show("Photo directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                checkedListBoxContent.Items.Clear();
                for (int i = 0; i < model.Content.Count; i++)
                {
                    if (!checkedListBoxContent.Items.Contains(model.Content[i].FileName))
                    {
                        checkedListBoxContent.Items.Add(model.Content[i].FileName);
                    }
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Parent == null)
            {
                if (CurrentModelsNode != null)
                {
                    if (e.Node.Text != CurrentModelsNode.Text)
                    {
                        checkedListBoxTags.Items.Clear();
                        checkedListBoxSubTags.Items.Clear();
                    }
                }
                CurrentModelsNode = e.Node;
                checkedListBoxProfiles.Show();

                axWindowsMediaPlayer1.Hide();

                var model = GetSelectedModel();

                if (model != null)
                {
                    textBoxEditName.Text = model.Name; // EDIT Model
                }

                if (model != null)
                {                    
                    textBoxContentPath.Text = model.ContentFolder;

                    SaveModels();
                    checkedListBoxContent.Items.Clear();

                    for(int c = 0; c < model.Content.Count; c++)
                    {
                        checkedListBoxContent.Items.Add(model.Content[c].FileName);
                        checkedListBoxContent.SetItemChecked(c, model.Content[c].Checked);
                    }

                    for (int cp = 0; cp < checkedListBoxProfiles.Items.Count; cp++)
                    {
                        if (model.Profiles.Contains(checkedListBoxProfiles.Items[cp].ToString()))
                        {
                            checkedListBoxProfiles.SetItemChecked(cp, true);
                        }
                        else
                        {
                            checkedListBoxProfiles.SetItemChecked(cp, false);
                        }
                    }

                    UpdateModels();
                }
            }
            else
            {
                labelCurrentModel.Text = "Немає вибраного вузла";
                checkedListBoxProfiles.Hide();
            }
        }

        private void CreateRootNodes()
        {
            if (!File.Exists(Data.ModelsDirectoryPath + Data.ModelsFilePath))
            {
                // Створення 1 коренів з віткою "profile"
                for (int i = 1; i <= 1; i++)
                {
                    MyModel model = new MyModel();
                    model.Index = i;
                    model.Name = model.Name + model.Index;
                    Data.Models.Add(model);
                }
                UpdateModels();
            }
        }
        private void checkedListBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        public MyModel GetSelectedModel()
        {
            MyModel result = null;

            for(int i = 0;i < Data.Models.Count; i++)
            {
                if (CurrentModelsNode != null)
                {
                    if (CurrentModelsNode.Text == Data.Models[i].Name)
                    {
                        result = Data.Models[i];
                        break;
                    }
                }
            }

            return result;
        }

        public MyContent GetSelectedContent(MyModel model)
        {
            MyContent result = null;

            for (int i = 0; i < model.Content.Count; i++)
            {
                if (CurrentContentText == model.Content[i].FileName)
                {
                    result = model.Content[i];
                    break;
                }
            }

            return result;
        }

        private void checkedListBoxProfiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(CurrentModelsNode != null)
            {
                List<string> checkedItems = new List<string>();
                foreach (var item in checkedListBoxProfiles.CheckedItems)
                {
                    checkedItems.Add(item.ToString());
                }

                MyModel model = GetSelectedModel();

                if(model != null)
                {
                    var newItem = checkedListBoxProfiles.Items[e.Index].ToString();

                    if (e.NewValue == CheckState.Checked)
                    {
                        if (!model.Profiles.Contains(newItem))
                        {
                            model.Profiles.Add(newItem);
                        }
                    }
                    else
                    {
                        if (model.Profiles.Contains(newItem))
                        {
                            model.Profiles.Remove(newItem);
                        }
                    }
                }

                SaveModels();
                UpdateModels();
            }
        }

        private void UpdateModels()
        {
            LoadModels();
            treeViewModels.Nodes.Clear();

            for (int i = 0;i< Data.Models.Count;i++)
            {
                TreeNode rootNode = new TreeNode(Data.Models[i].Name);
                treeViewModels.Nodes.Add(rootNode);
                rootNode.Expand();
                for (int p = 0;p < Data.Models[i].Profiles.Count; p++)
                {
                    rootNode.Nodes.Add(Data.Models[i].Profiles[p]);
                }
            }

            if (CurrentModelsNode != null)
            {
                labelCurrentModel.Text = "Вибрано: " + CurrentModelsNode.Text;
            }
            else
            {
                labelCurrentModel.Text = "Немає вибраної моделі: ";
            }

            treeViewModels.ExpandAll();

            MyModel model = GetSelectedModel();

            if (model != null)
            {
                Data.GoogleId = model.GoogleId;
                textBox1.Text = Data.GoogleId;
                comboBoxCopyModel.SelectedItem = model.CopyFrom;
                for (int c = 0; c < model.Content.Count; c++)
                {
                    checkedListBoxContent.SetItemChecked(c, model.Content[c].Checked);
                }

                if (model.Run)
                {
                    labelCurrentModel.BackColor = Color.Green;
                    labelCurrentModel.Text = "Вибрано: " + model.Name;
                }
                else
                {
                    labelCurrentModel.BackColor = Color.White;
                    labelCurrentModel.Text = "Вибрано: " + model.Name;
                }

                checkedListBoxTags.Items.Clear();
                checkedListBoxSubTags.Items.Clear();
                string fileNameTag = model.TagsFileName;

                if (File.Exists(fileNameTag))
                {
                    // Читання всіх рядків файлу
                    string[] dataTags = File.ReadAllLines(fileNameTag);

                    foreach (string tag in dataTags)
                    {
                        if(tag != "")
                        {
                            checkedListBoxTags.Items.Add(tag);
                        }
                    }
                }

                string fileNameSubTag = model.SubTagsFileName;

                if (File.Exists(fileNameSubTag))
                {
                    // Читання всіх рядків файлу
                    string[] dataTags = File.ReadAllLines(fileNameSubTag);

                    foreach (string subTag in dataTags)
                    {
                        if(subTag != "")
                        {
                            checkedListBoxSubTags.Items.Add(subTag);
                        }
                    }
                }
                ResetAntiDetect();
            }
            SaveModels();
        }

        private void checkedListBoxContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentContentText = checkedListBoxContent.SelectedItem.ToString();
            MyModel model = GetSelectedModel();
            if (model != null)
            {
                MyContent content = GetSelectedContent(model);
                for (int cst = 0; cst < checkedListBoxSubTags.Items.Count; cst++)
                {
                    if (content.SubTags.Contains(checkedListBoxSubTags.Items[cst]))
                    {
                        checkedListBoxSubTags.SetItemChecked(cst, true);
                    }
                    else
                    {
                        checkedListBoxSubTags.SetItemChecked(cst, false);
                    }
                }

                for (int ct = 0; ct < checkedListBoxTags.Items.Count; ct++)
                {
                    if (content.Tags.Contains(checkedListBoxTags.Items[ct]))
                    {
                        checkedListBoxTags.SetItemChecked(ct, true);
                    }
                    else
                    {
                        checkedListBoxTags.SetItemChecked(ct, false);
                    }
                }
            }

            // Check if an item is selected in the checkedListBox
            if (checkedListBoxContent.SelectedItem != null)
            {
                // Get the selected item's index
                int selectedIndex = checkedListBoxContent.SelectedIndex;

                // Get the selected item's text
                string selectedText = checkedListBoxContent.GetItemText(checkedListBoxContent.SelectedItem);

                label6.Text = Path.GetFileName(selectedText);

                CurrentContentText = selectedText;

                // Check if the selected index is within the range of your Photos list
                if (selectedIndex >= 0 && selectedIndex < model.Content.Count)
                {
                    try
                    {
                        // Get the file path of the selected photo
                        string selectedPhotoPath = model.Content[selectedIndex].FileName;

                        // Check if the file extension indicates an image
                        string extension = Path.GetExtension(selectedPhotoPath).ToLower();
                        if (IsContentExtension(extension))
                        {
                            // Handle the case where the selected item is not an image (e.g., video)
                            // You can display a message or take other appropriate action
                            axWindowsMediaPlayer1.Show();
                            axWindowsMediaPlayer1.URL = selectedPhotoPath;
                            axWindowsMediaPlayer1.uiMode = "none"; // Приховує всі елементи управління
                            axWindowsMediaPlayer1.Ctlcontrols.play();
                            axWindowsMediaPlayer1.settings.volume = 0;

                            // Підписка на подію PlayStateChange
                            axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        // Handle exceptions as needed
                        //richTextBox1.AppendText(ex.Message + "\n");
                    }
                }
                SaveModels();
            }
        }

        private void AxWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            // Перевірка, чи завершилося відтворення
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsStopped)
            {
                // Відтворення завершено, почати відтворення знову
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }


        private void checkedListBoxContent_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            LoadModels();
            if (CurrentModelsNode != null)
            {
                MyModel model = GetSelectedModel();

                if(model != null)
                {
                    MyContent content = GetSelectedContent(model);
                    if (e.NewValue == CheckState.Checked)
                    {
                        if(content != null)
                        {
                            content.Checked = true;
                        }
                        else
                        {
                            //richTextBox1.AppendText("Контент не знайдено\n");
                        }
                        
                    }
                    else if(e.NewValue == CheckState.Unchecked)
                    {
                        if (content != null)
                        {
                            content.Checked = false;
                        }
                        else
                        {
                            //richTextBox1.AppendText("Контент не знайдено\n");
                        }
                    }
                }               
            }
            SaveModels();
        }

        private void SaveModels()
        {
            // Серіалізація списку Models в JSON
            string json = JsonConvert.SerializeObject(Data.Models);

            // Збереження JSON у файл
            File.WriteAllText(Data.ModelsDirectoryPath + Data.ModelsFilePath, json);
        }

        private async Task SaveModelsAsync()
        {
            // Серіалізація списку Models в JSON
            string json = JsonConvert.SerializeObject(Data.Models);

            // Збереження JSON у файл
            File.WriteAllText(Data.ModelsDirectoryPath + Data.ModelsFilePath, json);
        }

        private void LoadModels()
        {
            if (Directory.Exists(Data.ModelsDirectoryPath))
            {
                if (File.Exists(Data.ModelsDirectoryPath + Data.ModelsFilePath))
                {
                    // Зчитування JSON з файлу
                    string json = File.ReadAllText(Data.ModelsDirectoryPath + Data.ModelsFilePath);

                    // Десеріалізація JSON у список Models
                    List<MyModel> deserializedModels = JsonConvert.DeserializeObject<List<MyModel>>(json);
                    Data.Models = deserializedModels;
                }
            }
            else
            {
                Directory.CreateDirectory(Data.ModelsDirectoryPath);
            }
        }

        private async Task LoadModelsAsync()
        {
            if (Directory.Exists(Data.ModelsDirectoryPath))
            {
                if (File.Exists(Data.ModelsDirectoryPath + Data.ModelsFilePath))
                {
                    // Зчитування JSON з файлу
                    string json = File.ReadAllText(Data.ModelsDirectoryPath + Data.ModelsFilePath);

                    // Десеріалізація JSON у список Models
                    List<MyModel> deserializedModels = JsonConvert.DeserializeObject<List<MyModel>>(json);
                    Data.Models = deserializedModels;
                }
            }
            else
            {
                Directory.CreateDirectory(Data.ModelsDirectoryPath);
            }
        }

        private void buttonUpdatePath_Click(object sender, EventArgs e)
        {
            MyModel model = GetSelectedModel();

            if(model != null)
            {
                // Створення об'єкта FolderBrowserDialog
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

                // Налаштування властивостей FolderBrowserDialog
                folderBrowserDialog.Description = "Виберіть папку";
                // Встановлення RootFolder на поточний робочий каталог
                folderBrowserDialog.SelectedPath = Environment.CurrentDirectory;

                // Відкриття діалогу вибору папки і очікування вибору користувачем
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    // Отримання вибраного каталогу (папки)
                    string selectedFolder = folderBrowserDialog.SelectedPath;

                    model.ContentFolder = selectedFolder;
                    textBoxContentPath.Text = model.ContentFolder;
                    LoadContent(model.ContentFolder);
                    // Ваш код обробки вибраного каталогу (папки)
                }
            }

            SaveModels();
            UpdateModels();
        }

        private void buttonEditName_Click(object sender, EventArgs e)
        {
            Data.EditOrAdd = true;
            ShowEdit(true);
        }

        private void ShowEdit(bool edit)
        {
            comboBoxCopyModel.Items.Clear();
            comboBoxCopyModel.Items.Add("Не копіювати");
            for (int m = 0; m < Data.Models.Count; m++)
            {
                comboBoxCopyModel.Items.Add(Data.Models[m].Name);
            }
            if(edit)
            {
                labelEditName.Show();
                textBoxEditName.Show();
                buttonEditNameSubmit.Show();
                label9.Show();
                textBox1.Show();
                label10.Show();
                comboBoxCopyModel.Show();
            }
            else
            {
                labelEditName.Hide();
                textBoxEditName.Hide();
                buttonEditNameSubmit.Hide();
                label9.Hide();
                textBox1.Hide();
                label10.Hide();
                comboBoxCopyModel.Hide();
            }
        }

        private void buttonEditNameSubmit_Click(object sender, EventArgs e)
        {
            LoadModels();

            MyModel model = GetSelectedModel();

            if(model != null)
            {
                if (Data.EditOrAdd)
                {
                    model.Name = textBoxEditName.Text;
                    model.GoogleId = textBox1.Text;
                    model.CopyFrom = comboBoxCopyModel.SelectedText;
                }

                if (comboBoxCopyModel.SelectedItem != null)
                {
                    string copyModelName = comboBoxCopyModel.SelectedItem.ToString();
                    MyModel copyModel = WebPoster.GetModelByName(copyModelName);

                    if (copyModel != null)
                    {
                        model.Run = copyModel.Run;

                        // Копіювання списку рядків
                        //model.Profiles = new List<string>(copyModel.Profiles);

                        // Копіювання складного списку. Припускаючи, що в MyContent є метод DeepCopy()
                        model.Content = new List<MyContent>(copyModel.Content);

                        model.ContentFolder = copyModel.ContentFolder;
                        model.Debug = copyModel.Debug;
                        model.PostPerPause = copyModel.PostPerPause;
                        model.PostPause = copyModel.PostPause;
                        model.GoogleId = copyModel.GoogleId;
                        model.PostTotal = copyModel.PostTotal;
                        model.PostCreated = copyModel.PostCreated;
                        model.PostNotCreated = copyModel.PostNotCreated;
                        model.TagsFileName = copyModel.TagsFileName;
                        model.SubTagsFileName = copyModel.SubTagsFileName;
                    }
                    else
                    {
                        richTextBox1.AppendText("Копіювання не знайшло модель");
                    }
                }
             
            }

            if(Data.EditOrAdd == false)
            {
                MyModel newModel = new MyModel();
                newModel.Name = textBoxEditName.Text;
                newModel.Index = Data.Models[Data.Models.Count - 1].Index + 1;
                newModel.CopyFrom = comboBoxCopyModel.SelectedText;
                Data.Models.Add(newModel);
            }

            ShowEdit(false);

            SaveModels();
            UpdateModels();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (axWindowsMediaPlayer1 != null)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop(); // Зупинка відтворення медіа
                axWindowsMediaPlayer1.close(); // Закриття медіа
                axWindowsMediaPlayer1.Dispose(); // Звільнення ресурсів
            }
        }

        private void buttonDeleteModel_Click(object sender, EventArgs e)
        {
            if(CurrentModelsNode != null)
            {
                MyModel model = GetSelectedModel();

                if(model!= null && Data.Models.Count > 1)
                {
                    Data.Models.Remove(model);
                    treeViewModels.Nodes.Remove(CurrentModelsNode);
                    SaveModels();
                    UpdateModels();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data.EditOrAdd = false;
            ShowEdit(true);
        }

        private void checkedListBoxTags_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MyModel model = GetSelectedModel();

            if (model != null)
            {
                MyContent content = GetSelectedContent(model);

                if (content != null)
                {
                    // Отримати текст обраного елемента
                    string checkText = checkedListBoxTags.GetItemText(checkedListBoxTags.Items[e.Index]);

                    if (e.NewValue == CheckState.Checked)
                    {
                        if (!content.Tags.Contains(checkText))
                        {
                            content.Tags.Add(checkText);
                        }
                    }else if(e.NewValue == CheckState.Unchecked)
                    {
                        if (content.Tags.Contains(checkText))
                        {
                            content.Tags.Remove(checkText);
                        }
                    }
                }
            }

            // Зберегти моделі
            SaveModels();
        }

        private void timerContentCheck_Tick(object sender, EventArgs e)
        {
            MyModel model = GetSelectedModel();
            if (model != null)
            {
                //labelPostTotalCount.Text = "Загальна кількість можливих постів: " + model.PostTotal.ToString();
                labelPostCreated.Text = "Створено постів: " + model.PostCreated.ToString();
                labelPostNotCreated.Text = "Не вдалося створити постів: " + model.PostNotCreated.ToString();
                labelStatus.Text = "Статус: " + model.Status;
            }
            //richTextBox1.AppendText("Content Timer\n");
        }

        private void checkedListBoxSubTags_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Отримати текст обраного елемента
            string checkText = checkedListBoxSubTags.GetItemText(checkedListBoxSubTags.Items[e.Index]);

            MyModel model = GetSelectedModel();

            if (model != null)
            {
                MyContent content = GetSelectedContent(model);

                if(content != null)
                {
                    if (e.NewValue == CheckState.Checked)
                    {
                        if (!content.SubTags.Contains(checkText))
                        {
                            content.SubTags.Add(checkText);
                        }
                    }
                }
            }

            // Зберегти моделі
            SaveModels();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MyModel model = GetSelectedModel();
            if (model != null)
            {
                model.Run = false;
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText($"Модель {model.Name} зупинена\n");
                SaveModels();
                UpdateModels();
            }
        }

        private void textBoxInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Якщо символ не є цифрою чи клавішею керування, відхиляємо його
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyModel model = GetSelectedModel();

            if(model != null)
            {
                MyContent content = GetSelectedContent(model);

                if(content != null)
                {
                    
                }
                SaveModels();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateData();
            SaveModels();
            UpdateModels();
        }

        private void UpdateData()
        {
            MyModel model = GetSelectedModel();
            var prevColor = button3.BackColor;

            if (model != null)
            {
                if (textBox1.Text != "Пусто")
                {
                    button3.Enabled = false;
                    richTextBox1.AppendText("Завантаження..\n");
                    button3.BackColor = Color.Yellow;
                    checkedListBoxTags.Items.Clear();
                    checkedListBoxSubTags.Items.Clear();
                    Data.Clear();
                    Data.DriveDownloadFile(Data.GoogleId);
                    LoadTags(true, model);
                    LoadSubTags(true, model);
                    Data.LoadData();

                    richTextBox1.AppendText("Завантаження готово!\n");
                    button3.BackColor = prevColor;
                    timerButtonActivate.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Вкажіть id google таблиці");
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MyModel model = GetSelectedModel();

            if(model != null)
            {
                model.PostPause = Convert.ToDouble(textBoxPausePerPost.Text);
                model.PostPerPause = Convert.ToInt32(textBoxPostCount.Text);
            }
            SaveModels();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MyModel model = GetSelectedModel();

            if (model != null)
            {
                Data.GoogleId = model.GoogleId;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkedListBoxProfiles.Items.Clear();
            Data.Profiles.Clear();
            LoadAntiDetectProfiles(true);
            MyModel model = GetSelectedModel();

            if(model != null)
            {
                for(int c = 0; c < checkedListBoxProfiles.Items.Count; c++)
                {
                    for(int p = 0;p < model.Profiles.Count; p++)
                    {
                        if (model.Profiles[p] == checkedListBoxProfiles.Items[c].ToString())
                        {
                            checkedListBoxProfiles.SetItemChecked(c, true);
                        }
                    }
                }
            }
        }

        private void timerButtonActivate_Tick(object sender, EventArgs e)
        {
            button3.Enabled = true;
            buttonRunModel.Enabled = true;
            timerButtonActivate.Enabled = false;
        }

        private void timerDayReset_Tick(object sender, EventArgs e)
        {
            for(int m = 0;m < Data.Models.Count; m++)
            {
                MyModel model = Data.Models[m];

                model.Run = false;
            }
        }
    }
}
