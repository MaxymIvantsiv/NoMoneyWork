using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OctoManager
{
    public class MyModel
    {
        public string Name = "Model";
        public int Index = 0;
        public volatile bool Run = false;
        public List<string> Profiles = new List<string>();
        public List<MyContent> Content = new List<MyContent>();
        public string ContentFolder = "";
        public string Debug = "";
        public int PostPerPause = 0;
        public double PostPause;
        public string GoogleId = "Пусто";
        public volatile int PostTotal = 0;
        public volatile int PostCreated = 0;
        public volatile int PostNotCreated = 0;
        public string TagsFileName = "";
        public string SubTagsFileName = "";
        public string CopyFrom = "";
        public string Status = "";
        public volatile List<MyPost> DayPostList = new List<MyPost>();

        public MyModel()
        {

        }

        public List<string> GetSubReddits(string tag)
        {
            List<string> result = new List<string>();



            return result;
        }

        public bool ContentContains(string file)
        {
            bool result = false;

            for(int i = 0;i < Content.Count;i++)
            {
                if(file == Content[i].FileName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void ContentAdd(MyContent content)
        {
            Content.Add(content);
        }

        public List<MyPost> GetAllPost()
        {
            List<MyPost> posts = new List<MyPost>();

            for(int c = 0;c < Content.Count; c++)
            {
                posts.AddRange(Content[c].Posts);
            }

            return posts;
        }

        public void ContentRemove(string file)
        {
            for (int i = 0; i < Content.Count; i++)
            {
                if (file == Content[i].FileName)
                {
                    Content.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
