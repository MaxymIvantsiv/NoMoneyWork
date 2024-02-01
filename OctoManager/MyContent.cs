using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OctoManager
{
    public class MyContent
    {
        public string FileName;

        public List<string> Tags = new List<string>();
        public List<string> SubTags = new List<string>();

        public string AntiDetectProfile = "";

        public bool Checked = true;

        public List<MyPost> Posts = new List<MyPost>();

        public MyContent(string file,string antiDetectProfile,int checkedValue)
        {
            FileName = file;
            
            if(antiDetectProfile != "")
            {
                AntiDetectProfile = antiDetectProfile;
            }

            if(checkedValue == 0)
            {
                Checked = false;
            }else if (checkedValue == 1)
            {
                Checked = true;
            }
        }

        public List<MyPost> GetPosts()
        {
            List<MyPost> results = new List<MyPost>();
            Random random = new Random();

            for (int s = 0; s < Data.SubTags.Count; s++)
            {
                for (int mt = 0; mt < SubTags.Count; mt++)
                {
                    if (Data.SubTags[s].Contains(SubTags[mt]))
                    {
                        bool titleOrQuestion = random.Next(2) == 0;

                        MyPost post = new MyPost(Data.Links[s]);

                        string titleList = Data.TitleList[s];
                        if (titleList == "")
                        {
                            titleList = "Универсальные";
                        }
                        post.SubTag = SubTags[mt];
                        
                        post.Title = Data.GetRandomTitleByListName(titleList, titleOrQuestion);
                        if (s < Data.FlairsList.Count)
                        {
                            post.Flair = Data.FlairsList[s];
                        }
                        else
                        {
                            post.Flair = "";
                        }

                        if (s < Data.Priority.Count)
                        {
                            if(Data.Priority[s] != "")
                            {
                                post.Priority = Convert.ToInt32(Data.Priority[s]);
                            }
                            else
                            {
                                post.Priority = 0;
                                
                            }                      
                        }
                        else
                        {
                            post.Priority = 0;
                        }
                            
                        if(s < Data.PostingFrequency.Count)
                        {
                            if(Data.PostingFrequency[s] != "")
                            {
                                post.PostingFrequency = Convert.ToInt32(Data.PostingFrequency[s]);
                            }
                            else
                            {
                                post.PostingFrequency = 0;
                            }
                        }
                        else
                        {
                            post.PostingFrequency = 0;
                        }

                        results.Add(post);
                    }
                }
            }

            for (int t = 0; t < Data.Tags.Count; t++)
            {
                for (int mt = 0; mt < Tags.Count; mt++)
                {
                    if (Data.Tags[t].Contains(Tags[mt]))
                    {
                        bool titleOrQuestion = random.Next(2) == 0;

                        MyPost post = new MyPost(Data.Links[t]);

                        string titleList = Data.TitleList[t];
                        if (titleList == "")
                        {
                            titleList = "Универсальные";
                        }
                        post.Tag = Tags[mt];
                        post.Title = Data.GetRandomTitleByListName(titleList, titleOrQuestion);
                        if (t < Data.FlairsList.Count)
                        {
                            post.Flair = Data.FlairsList[t];
                        }
                        else
                        {
                            post.Flair = "";
                        }

                        if (t < Data.Priority.Count)
                        {
                            if(Data.Priority[t] != "")
                            {
                                post.Priority = Convert.ToInt32(Data.Priority[t]);
                            }
                            else
                            {
                                post.Priority = 0;
                            }
                        }
                        else
                        {
                            post.Priority = 0;
                        }

                        if (t < Data.PostingFrequency.Count)
                        {
                            if(Data.PostingFrequency[t] != "")
                            {
                                post.PostingFrequency = Convert.ToInt32(Data.PostingFrequency[t]);
                            }
                            else
                            {
                                post.PostingFrequency = 0;
                            }
                        }
                        else
                        {
                            post.PostingFrequency = 0;
                        }

                        results.Add(post);
                    }
                }
            }

            return results;
        }
    }
}
