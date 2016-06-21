using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmegaSPA.ModelsFacebook
{
    public class FacebookEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }
        // public List<string> Attendings { get; set; }

        public FacebookEvent(string id, string name, string cover )
        {
            Id = id;
            Name = name;
            Cover = cover;
        }
    }
}