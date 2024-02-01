using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Util.Store;
using System.Threading;
using System.Net;
using OfficeOpenXml;

namespace OctoManager
{
    public class MyTitle
    {
        public string ListName;
        public List<string> Titles = new List<string>();
        public List<string> Questions = new List<string>();
    }
    public static class Data
    {
        public static List<MyModel> Models = new List<MyModel>();

        public static List<MyPost> Posts = new List<MyPost>();

        public static string ModelsDirectoryPath = @"Data\Models";

        public static string ModelsFilePath = @"\models.json";

        public static bool EditOrAdd = false; // if true - edit, if false - add

        public static List<string> Profiles = new List<string>(); // Anti detect

        public static List<string> Links = new List<string>();
        public static List<string> Tags = new List<string>();
        public static List<string> SubTags = new List<string>();
        public static List<string> TitleList = new List<string>();
        public static List<string> FlairsList = new List<string>();
        public static List<string> Priority = new List<string>();

        public static List<string> AllTitleList = new List<string>();

        public static List<MyTitle> Titles = new List<MyTitle>();

        public static List<string> PostingFrequency = new List<string>();

        public static DateTime LastDate = new DateTime();

        public static bool CanPost = true;

        public static int GoogleDocsSize = 1605; // 1605

        public static string apiKey = "AIzaSyD_U0uHQEqhNsxnm1of0biljGCLzMp9M-s";

        private static SheetsService Service;

        static string[] Scopes = { DriveService.Scope.DriveReadonly };

        static string ApplicationName = "GoogleApiManager";
        public static string GoogleapiKey = "AIzaSyD_U0uHQEqhNsxnm1of0biljGCLzMp9M-s";
        static public string GoogleId = "";

        private static Random random = new Random();

        private static string exelFile = "output.xlsx";

        public static string ColumnNameSubreddits= "Subreddits";
        public static string ColumnNameTag = "Tag";
        public static string ColumnNameSubtag = "Subtag";
        public static string ColumnNameFlair = "Flair";
        public static string ColumnNameNameoflistforTitle = "Name of list for Title";
        public static string ColumnNamePostingFrequency = "Posting frequency";
        public static string ColumnNamePriority = "Priority";
        public static string ColumnNameRedgifs = "Redgifs";

        public static void LoadData()
        {
            Links = ReadExcelFileLineByLine(exelFile, "Sheet1",ColumnNameSubreddits);
            Tags = ReadExcelFileLineByLine(exelFile, "Sheet1",ColumnNameTag);
            SubTags = ReadExcelFileLineByLine(exelFile, "Sheet1", ColumnNameSubtag);

            TitleList = ReadExcelFileLineByLine(exelFile, "Sheet1", ColumnNameNameoflistforTitle);
            FlairsList = ReadExcelFileLineByLine(exelFile, "Sheet1", ColumnNameFlair);
            AllTitleList = GetAllTitleList();
            Priority = ReadExcelFileLineByLine(exelFile, "Sheet1", ColumnNamePriority);
            PostingFrequency = ReadExcelFileLineByLine(exelFile, "Sheet1", ColumnNamePostingFrequency);
        }

        public static void Clear()
        {
            Links.Clear();
            Tags.Clear();
            SubTags.Clear();
            TitleList.Clear();
            FlairsList.Clear();
            AllTitleList.Clear();
            Priority.Clear();
            PostingFrequency.Clear();
        }

        public static string GetSectorByLineName(string filePath, string sheetName,string lineName)
        {
            string result = "";

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                    if (worksheet == null)
                    {
                        throw new Exception($"Аркуш з назвою '{sheetName}' не знайдено.");
                    }

                    int columnCount = worksheet.Dimension?.End.Column ?? 0;

                    int row = 1;
                    for (int col = 1; col <= columnCount; col++)
                    {
                        string header = GetExcelColumnName(col);
                        var cellValue = worksheet.Cells[row, col].Value;
                        string cellText = cellValue?.ToString() ?? "";
                        if(cellText == lineName)
                        {
                            result = header;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при читанні файлу: {ex.Message}");
            }

            return result;
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }

        public static List<string> ReadExcelFileLineByLine(string filePath, string sheetName,string lineName)
        {
            List<string> results = new List<string>();

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                    string startColumn = GetSectorByLineName(filePath, sheetName, lineName);
                    int startColumnIndex = ExcelColumnToInt(startColumn);
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        var cellValue = worksheet.Cells[row, startColumnIndex].Value;
                        string cellText = cellValue?.ToString() ?? "";
                        results.Add(cellText);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception more effectively
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return results;
        }

        // Helper function to convert Excel column letter to index (A->1, B->2, ..., Z->26)
        private static int ExcelColumnToInt(string column)
        {
            int result = 0;
            foreach (char c in column)
            {
                result = result * 26 + (c - 'A' + 1);
            }
            return result;
        }

        public static void DriveDownloadFile(string fileId)
        {
            try
            {
                // Авторизація користувача
                string credentialsPath = Path.Combine(ModelsDirectoryPath, "credentials.json");
                UserCredential credential;
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        new[] { SheetsService.Scope.SpreadsheetsReadonly },
                        "user",
                        CancellationToken.None,
                        new FileDataStore("token.json", true)).Result;
                }

                // Замените значениями из вашего проекта
                string spreadsheetId = fileId;
                string range = "Сабреддитс"; // Имя листа

                // Создайте экземпляр сервиса Google Sheets API
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "OctoManager"
                });

                // Загрузите данные из Google таблицы
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

                ValueRange response = request.Execute();
                IList<IList<object>> values = response.Values;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Создайте объект для представления данных в формате Excel
                var excelPackage = new ExcelPackage();
                

                // Создайте новый лист в Excel
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Заполните лист данными из Google таблицы
                for (int i = 0; i < values.Count; i++)
                {
                    for (int j = 0; j < values[i].Count; j++)
                    {
                        worksheet.Cells[i + 1, j + 1].Value = values[i][j];
                    }
                }

                // Сохраните Excel файл
                using (var stream = new FileStream(exelFile, FileMode.Create))
                {
                    excelPackage.SaveAs(stream);
                }

                //MessageBox.Show("Excel файл успешно создан.");
            }
            catch (Exception e)
            {
                // Обробка помилок
                MessageBox.Show($"An error occurred: {e.Message}");
            }
        }

        public static string GetRandomFlair(int index)
        {
            string result = "";

            string flairs = "";

            if (index < FlairsList.Count)
            {
                flairs = FlairsList[index];
            }

            // Розділити рядок flairs по комах
            string[] flairArray = flairs.Split(',');

            // Випадково вибрати елемент із масиву
            if (flairArray.Length > 0)
            {
                int randomIndex = random.Next(flairArray.Length);
                result = flairArray[randomIndex].Trim(); // Trim, щоб вилучити зайві пробіли
            }

            return result;
        }

        public static string GetTitleByListName(string listName,int index,bool titleOrQuestion)
        {
            string result = "";

            for(int i = 0;i < Titles.Count; i++)
            {
                if (titleOrQuestion)
                {
                    if (Titles[i].ListName == listName)
                    {
                        result = Titles[i].Titles[index];
                        break;
                    }
                }
                else
                {
                    if (Titles[i].ListName == listName)
                    {
                        result = Titles[i].Questions[index];
                        break;
                    }
                }
            }

            return result;
        }

        public static string GetRandomTitleByListName(string listName, bool titleOrQuestion)
        {
            string result = "";

            for (int i = 0; i < Titles.Count; i++)
            {
                if (titleOrQuestion)
                {
                    if (Titles[i].ListName == listName)
                    {
                        int index = random.Next(Titles[i].Titles.Count);
                        result = Titles[i].Titles[index];
                        break;
                    }
                }
                else
                {
                    if(Titles[i].Questions.Count > 0)
                    {
                        if (Titles[i].ListName == listName)
                        {
                            int index = random.Next(Titles[i].Questions.Count);
                            result = Titles[i].Questions[index];
                            break;
                        }
                    }
                    else
                    {
                        if (Titles[i].ListName == listName)
                        {
                            int index = random.Next(Titles[i].Titles.Count);
                            result = Titles[i].Titles[index];
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static List<string> GetAllTitleList()
        {
            List<string> result = new List<string>();

            string spreadsheetId = "1DXCv-h_NkW2iXFmRRIDgcsWRp0wEc3W3Xo0cgnOxQj4"; // Замініть це на свій spreadsheetId

            Service = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = apiKey, // Замініть це на свій ключ API
                ApplicationName = ApplicationName
            }); ;

            var sheetMetadata = Service.Spreadsheets.Get(spreadsheetId).Execute();
            var sheets = sheetMetadata.Sheets;

            if (sheets != null && sheets.Count > 0)
            {
                foreach (var sheet in sheets)
                {
                    var listName = sheet.Properties.Title;
                    MyTitle myTitle = new MyTitle();
                    myTitle.ListName = listName;
                    // Визначте діапазон клітинок, які ви хочете отримати
                    string range = $"{listName}!A1:A{GoogleDocsSize}"; // Замініть це на свій діапазон

                    // Викликайте API для отримання значень клітинок
                    var request = Service.Spreadsheets.Values.Get(spreadsheetId, range);
                    var response = request.Execute();

                    // Обробляйте відповідь та виводьте значення клітинок
                    IList<IList<object>> values = response.Values;
                    if (values != null && values.Count > 0)
                    {
                        foreach (var row in values)
                        {
                            foreach (var cell in row)
                            {
                                myTitle.Titles.Add(cell.ToString());
                            }
                            Console.WriteLine();
                        }
                        Titles.Add(myTitle);
                    }
                    else
                    {
                        Console.WriteLine("Немає даних.");
                    }


                    // Визначте діапазон клітинок, які ви хочете отримати
                    string rangeQ = $"{listName}!B1:B{GoogleDocsSize}"; // Замініть це на свій діапазон

                    // Викликайте API для отримання значень клітинок
                    var requestQ = Service.Spreadsheets.Values.Get(spreadsheetId, rangeQ);
                    var responseQ = requestQ.Execute();

                    // Обробляйте відповідь та виводьте значення клітинок
                    IList<IList<object>> valuesQ = responseQ.Values;
                    if (valuesQ != null && valuesQ.Count > 0)
                    {
                        foreach (var rowQ in valuesQ)
                        {
                            foreach (var cellQ in rowQ)
                            {
                                myTitle.Questions.Add(cellQ.ToString());
                            }
                            Console.WriteLine();
                        }
                        Titles.Add(myTitle);
                    }
                    else
                    {
                        Console.WriteLine("Немає даних.");
                    }


                    result.Add(listName);
                }
            }
            else
            {
                Console.WriteLine("Листів не знайдено.");
            }

            return result;
        }

        public static string GetRandomTitle(string listName,int index)
        {
            string result = "";

            string spreadsheetId = "1DXCv-h_NkW2iXFmRRIDgcsWRp0wEc3W3Xo0cgnOxQj4"; // Замініть це на свій spreadsheetId
            string sheetNameToFind = listName; // Замініть це на назву листа, яку ви шукаєте

            var service = Service; // Отримайте об'єкт SheetsService

            // Викличте функцію для отримання інформації про листи у таблиці
            IList<Sheet> sheets = GetSheets(service, spreadsheetId);

            // Знайдіть лист за назвою
            Sheet targetSheet = FindSheetByName(sheets, sheetNameToFind);

            if (targetSheet != null)
            {
                // Get values from the A column of the sheet
                IList<object> columnValues = GetColumnValues(service, spreadsheetId, targetSheet.Properties.Title, "A");

                if (columnValues != null && columnValues.Count > 0)
                {
                    result = columnValues[index].ToString();
                    //MessageBox.Show(result);
                }
                else
                {
                    MessageBox.Show("Sheet's A column has no data.");
                }
            }
            else
            {
                MessageBox.Show("Лист не знайдено.");
            }

            return result;
        }

        private static IList<Sheet> GetSheets(SheetsService service, string spreadsheetId)
        {
            // Створіть запит для отримання інформації про листи у таблиці
            SpreadsheetsResource.GetRequest request = service.Spreadsheets.Get(spreadsheetId);
            Spreadsheet spreadsheet = request.Execute();

            // Отримайте список листів у таблиці
            IList<Sheet> sheets = spreadsheet.Sheets;

            return sheets;
        }

        private static Sheet FindSheetByName(IList<Sheet> sheets, string sheetName)
        {
            // Пошук листа за назвою
            return sheets.FirstOrDefault(sheet => sheet.Properties.Title == sheetName);
        }

        // Function to get values from a specific column of a sheet
        private static IList<object> GetColumnValues(SheetsService service, string spreadsheetId, string sheetName, string column)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!{column}1:{column}");

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                return values.Select(row => row.Count > 0 ? row[0] : null).ToList();
            }

            return null;
        }

        public static List<string> ReadDocumentLineByLine(string range)
        {
            List<string> results = new List<string>();

            string spreadsheetId = "15ClKtx6a-l6AJ3VTgodP67CI7WTZJcs4bKs3fN-MuAU";
            try
            {
                // Create Google Sheets API service
                Service = new SheetsService(new BaseClientService.Initializer
                {
                    ApplicationName = "GoogleDocsReader",
                    ApiKey = Data.apiKey
                });

                SpreadsheetsResource.ValuesResource.GetRequest request =
                    Service.Spreadsheets.Values.Get(spreadsheetId, range);

                ValueRange response = request.Execute();
                IList<IList<object>> values = response.Values;

                if (values != null && values.Count > 0)
                {
                    foreach (var row in values)
                    {
                        // Join the cells in the row with a newline character
                        string rowText = string.Join(Environment.NewLine, row.Select(cell => cell.ToString()));
                        results.Add(rowText);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception more effectively
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return results;
        }

    }
}
