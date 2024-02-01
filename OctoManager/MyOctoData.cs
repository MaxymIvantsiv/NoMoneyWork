using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoManager
{
    public class MyOctoData
    {
        public JObject Profile;
        public string WsUrl;
        public string Uuid = "";
    }
}
