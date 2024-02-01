using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoManager
{
    public class MyPost
    {
        public string Title = "Test title";
        public bool TodayUploaded = false;
        public string URL = "";
        public string Flair = "";
        public DateTime TimeUploaded;
        public string Tag = "";
        public string SubTag = "";
        public string FileName;
        public string AntiDetectProfile = "";
        public string MyModelName = "";
        public int Priority = 0;
        public int PostingFrequency = 0;

        public MyPost(string url)
        {
            if(url != null)
            {
                URL = url;
            }
        }
    }
}
