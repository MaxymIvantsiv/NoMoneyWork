using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OctoManager
{
    public static class PostManager
    {
        private static List<string> Code = new List<string>();

        private static void CreateUploadFileCodeFile(MyContent content)
        {
            Code.Add("import asyncio;");
            Code.Add("import logging;");
            Code.Add("import os;");
            Code.Add("import httpx;");
            Code.Add("import pyppeteer;");
            Code.Add("import json;");
            Code.Add("import sys;");
            Code.Add("import random;");
            Code.Add("from datetime import datetime, timedelta;");

            Code.Add("os.chdir(r\"C:\\Users\\Max\\source\\repos\\OctoManager\\OctoManager\\bin\\Debug\\Data\\Models\");");
            Code.Add("logging.basicConfig(level=logging.INFO, format='%(asctime)s [%(levelname)s] %(name)s: %(message)s');");
            Code.Add("log = logging.getLogger('octo');");
            Code.Add("OCTO_TOKEN = os.getenv('OCTO_TOKEN', '15d5c39426514595830334855833f876');");
            Code.Add("LOCAL_API = 'http://localhost:58888/api/profiles/start';");
            Code.Add("HEADERS = {'X-Octo-Api-Token': OCTO_TOKEN};");
            Code.Add("OCTO_API = 'https://app.octobrowser.net/api/v2/automation/profiles';");

            Code.Add("async def get_profiles(cli):");
            Code.Add("    profiles = [];");
            Code.Add("    api_token = \"15d5c39426514595830334855833f876\";");
            Code.Add("    api_url = \"https://app.octobrowser.net/api/v2/automation/profiles\";");
            Code.Add("    page_len = 100;");
            Code.Add("    page = 0;");
            Code.Add("    full_url = f\"{api_url}?page_len={page_len}&page={page}&fields=title,description,proxy,start_pages,tags,status,last_active,version,storage_options,created_at,updated_at&ordering=active\";");
            Code.Add("    try:");
            Code.Add("        response = await cli.get(full_url, headers={'X-Octo-Api-Token': api_token});");
            Code.Add("        if response.status_code == 200:");
            Code.Add("            json_response = response.json();");
            Code.Add("            data_array = json_response.get(\"data\", []);");
            Code.Add("            if data_array:");
            Code.Add("                for profile in data_array:");
            Code.Add("                    profiles.append(profile);");
            Code.Add("        else:");
            Code.Add("            print(f\"Error: {response.status_code}\");");
            Code.Add("    except Exception as ex:");
            Code.Add("        print(f\"Exception: {ex}\");");
            Code.Add("    return profiles;");

            Code.Add("async def get_cdp(cli, profile):");
            Code.Add("    uuid = profile['uuid'];");
            Code.Add("    resp = (await cli.post(LOCAL_API, json={'uuid': uuid, 'debug_port': True})).json();");
            Code.Add("    log.info(f'Start profile resp: {resp}');");
            Code.Add("    return resp['ws_endpoint'];");

            Code.Add("async def pause_random():");
            Code.Add("    pause_time = random.uniform(3, 6);");
            Code.Add("    await asyncio.sleep(pause_time);");
            Code.Add("    print(f'Paused for {pause_time} seconds');");

            Code.Add("async def check_and_launch_profiles():");
            Code.Add("    flair = \"Cat Picture\";");
            Code.Add("    post_title_text = \"\";");
            Code.Add("    red_gifs = False;");
            Code.Add("    async with httpx.AsyncClient() as cli:");
            Code.Add("        profiles = await get_profiles(cli);");
            Code.Add("        for profile in profiles:");
            Code.Add($"            profile_content = \"{content.AntiDetectProfile}\";");
            Code.Add("            profile_title = profile['title'];");
            Code.Add("            if profile_title == profile_content:");
            Code.Add("                ws = await get_cdp(cli, profile);");
            Code.Add("                browser = await pyppeteer.launcher.connect(browserWSEndpoint=ws);");
            Code.Add("                try:");
            Code.Add("                    reddit_cat_link = \"https://www.reddit.com/r/cats\";");
            Code.Add("                    reddit_cat_description = \"Kawaii cat\";");
            Code.Add("                    page = await browser.newPage();");
            Code.Add("                    url = reddit_cat_link;");
            Code.Add("                    await page.goto(url);");
            Code.Add("                    await pause_random();");
            Code.Add("                    join_button = \"//button[contains(@class,'_1LHxa-yaHJwrPK8kuyv_Y4 _2iuoyPiKHN3kfOoeIQalDT _10BQ7pjWbeYP63SAPNS8Ts HNozj_dKjQZ59ZsfEegz8 _34mIRHpFtnJ0Sk97S2Z3D9')]\";");
            Code.Add("                    try:");
            Code.Add("                        button_join_handle = await page.Jx(join_button);");
            Code.Add("                        if button_join_handle:");
            Code.Add("                            button_text = await page.evaluate('(element) => element.textContent', button_join_handle[0]);");
            Code.Add("                            button_text_lower = button_text.lower();");
            Code.Add("                            print(f'Text of the button: {button_text_lower}');");
            Code.Add("                            if button_text_lower == \"join\":");
            Code.Add("                                await button_join_handle[0].click();");
            Code.Add("                                print(\"Join\");");
            Code.Add("                                await pause_random();");
            Code.Add("                    except Exception as ex:");
            Code.Add("                        print(f\"Exception: {ex}\");");
            Code.Add("                    create_post_button = \"//input[@class='zgT5MfUrDMC54cpiCpZFu']\";");
            Code.Add("                    button_create_post_handle = await page.Jx(create_post_button);");
            Code.Add("                    if button_create_post_handle:");
            Code.Add("                        await button_create_post_handle[0].click();");
            Code.Add("                    button_media_content = \"//button[@class='Z1w8VkpQ23E1Wdq_My3U4 ']\";");
            Code.Add("                    try:");
            Code.Add("                        await pause_random();");
            Code.Add("                        if red_gifs==False:");
            Code.Add("                            button_media_content_handle = await page.Jx(button_media_content);");
            Code.Add("                            if button_media_content_handle:");
            Code.Add("                                await button_media_content_handle[0].click();");
            Code.Add("                            await pause_random();");
            Code.Add("                        post_title_xpath = \"//textarea[@placeholder='Title']\";");
            Code.Add("                        post_title_handle = await page.Jx(post_title_xpath);");
            Code.Add("                        if post_title_handle:                       ");
            Code.Add("                            # Ввести текст у поле");
            Code.Add("                            await post_title_handle[0].type(post_title_text);");
            Code.Add("                            print(\"Title entered!\");");
            Code.Add("                        await pause_random();");
            Code.Add("                        file_input_xpath = \"//input[@type='file']\";");
            Code.Add($"                        file_content_path = r\"{content.FileName}\";");
            Code.Add("                        # Знайти елемент input за допомогою XPath");
            Code.Add("                        file_input = await page.xpath(file_input_xpath);");
            Code.Add("                        # Завантажити файл (замість 'ваш_шлях_до_файлу' вкажіть реальний шлях до файлу)");
            Code.Add("                        await pause_random();");
            Code.Add("                        await file_input[0].uploadFile(file_content_path);");
            Code.Add("                        await pause_random();");
            Code.Add("                        if flair != \"\":");
            Code.Add("                            button_post_flair_xpath = \"//div[@class='Nb7NCPTlQuxN_WDPUg5Q2 zprH8YpG-gVpFuEr-eQJw'][contains(.,'Add flair')]\";");
            Code.Add("                            button_post_flair_handle = await page.Jx(button_post_flair_xpath);");
            Code.Add("                            if button_post_flair_handle:");
            Code.Add("                                await button_post_flair_handle[0].click();");
            Code.Add("                                await pause_random();");
            Code.Add("                                post_flair_xpath = \"//span[contains(.,'\"+str(flair)+\"')]\";");
            Code.Add("                                post_flair_handle = await page.Jx(post_flair_xpath);");
            Code.Add("                                if post_flair_handle:");
            Code.Add("                                    print(\"Flair finded!\");");
            Code.Add("                                    await post_flair_handle[0].click();");
            Code.Add("                                    await pause_random();");
            Code.Add("                                    post_flair_apply_button_xpath = \"//button[@class='_2iuoyPiKHN3kfOoeIQalDT _10BQ7pjWbeYP63SAPNS8Ts HNozj_dKjQZ59ZsfEegz8 ']\";");
            Code.Add("                                    post_flair_apply_button_handle = await page.Jx(post_flair_apply_button_xpath);");
            Code.Add("                                    if post_flair_apply_button_handle:");
            Code.Add("                                        await post_flair_apply_button_handle[0].click();");
            Code.Add("                                        print(\"Flair apply\");");
            Code.Add("                                    await pause_random();");
            Code.Add("                                    post_make_post_xpath = \"//button[@class='_18Bo5Wuo3tMV-RDB8-kh8Z _2iuoyPiKHN3kfOoeIQalDT _10BQ7pjWbeYP63SAPNS8Ts HNozj_dKjQZ59ZsfEegz8 ']\";");
            Code.Add("                                    post_make_post_handle = await page.Jx(post_make_post_xpath);");
            Code.Add("                                    if post_make_post_handle:");
            Code.Add("                                        await post_make_post_handle[0].click();");
            Code.Add("                                        print(\"Post created!\");");
            Code.Add("                                        await pause_random();");
            Code.Add("                    except Exception as ex:");
            Code.Add("                        print(f\"Exception: {ex}\");");
            Code.Add("                finally:");
            Code.Add("                    await browser.close();");

            Code.Add("asyncio.get_event_loop().run_until_complete(check_and_launch_profiles());");
            Code.Add("file_path = \"cat_1_upload.py\";");
            Code.Add("try:");
            Code.Add("    os.remove(file_path)");
            Code.Add("    print(f\"Файл {file_path} видалено успішно.\");");
            Code.Add("except OSError as e:");
            Code.Add("    print(f\"Виникла помилка при видаленні файлу {file_path}: {e}\");");
            Code.Add("app_path = os.getcwd();");
            Code.Add("print(\"Шлях до аппки \" + str(app_path));");

            //Code.Add("input(\"Натисніть Enter для завершення програми.\")");
        }

        public static void GeneratePythonScript(MyContent content)
        {
            string file = Path.GetFileNameWithoutExtension(content.FileName);
            // Шлях до файлу, куди ми будемо записувати код Python
            string filePath = Data.ModelsDirectoryPath + $@"\{file}_upload.py";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (File.Exists(filePath) == false)
            {
                CreateUploadFileCodeFile(content);

                // Записати кожен рядок коду в файл
                File.WriteAllLines(filePath, Code);

                // Пауза на 1 секунди (2000 мілісекунд)
                Thread.Sleep(1000);
                Code.Clear();
                // Використовуйте Process.Start, щоб відкрити файл за замовчуванням у Windows
                Process.Start(filePath);
            }
        }
    }
}
