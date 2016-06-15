using NUnit.Framework;
using Omega.DataManager;
using System;
using System.Collections.Generic;

namespace Omega.Model.Tests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void Test()
        {
            List<string> playlist = new List<string>();

            MetaDonnees askedDonnees = new MetaDonnees();
            askedDonnees.acousticness = "0.51";
            playlist.Add("d:3135556");
            playlist.Add("s:0eGsygTp906u18L0Oimnem");
            playlist.Add("s:11hqMWwX7sF3sOGdtijofF"); 
            playlist.Add("s:06AKEBrKUckW0KREUWRnvT");
            double ratio = 0.10;
            Livefusion l = new Livefusion();
            var newPlaylist = l.PlaylistAnalyser(playlist, askedDonnees, ratio);
            Console.WriteLine(newPlaylist);
        }
    }
}
