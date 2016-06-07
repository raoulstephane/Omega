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
            double ratio = 0.05;
            Livefusion l = new Livefusion();
            var newPlaylist = l.PlaylistAnalyser(playlist, askedDonnees, ratio);
            Console.WriteLine(newPlaylist);
        }
    }
}
