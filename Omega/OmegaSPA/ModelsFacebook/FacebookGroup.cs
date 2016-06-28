namespace OmegaSPA.ModelsFacebook
{
    public class FacebookGroup
    {
        public FacebookGroup( string id, string name, string cover )
        {
            Id = id;
            Name = name;
            Cover = cover;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }
    }
}