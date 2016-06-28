using NUnit.Framework;
using Omega.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega.DataManager.Test
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestQueu()
        {
            DatabaseQueries.InsertSpotifyTrack("a","aa","aaa","aaaa","aaaaa","aaaaaa","aaaaaaa");
        }
    }
}
