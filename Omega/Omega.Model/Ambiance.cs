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
        Livefusion l = new Livefusion();
        public List<string> Ambiancer(string ambiance, List<string> playlist)
        {
            MetaDonnees metadonnées = new MetaDonnees();
            using (var streamReader = new StreamReader(@"D:\Intech\PI\S4\Projet\Omega\Omega\Omega\Omega.Model\Modes.txt", Encoding.UTF8))
            {
                string text = streamReader.ReadToEnd();
                JObject modesJ = JObject.Parse(text);
                metadonnées = JsonConvert.DeserializeObject<MetaDonnees>(modesJ[ambiance].ToString());
            }
            return l.PlaylistAnalyser(playlist, metadonnées, 10);
        }
    }
}
