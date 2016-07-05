using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Omega.Model
{
    public class Ambiance
    {
        public static JArray Ambiancer(string ambiance, string playlist)
        {
            Livefusion l = new Livefusion();
            MetaDonnees metadonnéesMax = new MetaDonnees();
            MetaDonnees metadonnéesMin = new MetaDonnees();
            using (var streamReader = new StreamReader(@"D:\Intech\PI\S4\Projet\Omega\Omega\Omega\Omega.Model\Modes.txt", Encoding.UTF8))
            {
                string text = streamReader.ReadToEnd();
                JObject modesJ = JObject.Parse(text);
                metadonnéesMax = JsonConvert.DeserializeObject<MetaDonnees>(modesJ[ambiance + "Max"].ToString());
                metadonnéesMin = JsonConvert.DeserializeObject<MetaDonnees>(modesJ[ambiance + "Min"].ToString());
            }
            return l.PlaylistAnalyser(playlist, metadonnéesMax, metadonnéesMin);
        }
    }
}
