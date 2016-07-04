using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Security.Claims;

namespace Owin.Security.Providers.Deezer.Provider
{
    public class DeezerAuthenticatedContext: BaseContext
    {
        public DeezerAuthenticatedContext(IOwinContext context, JObject user, string accessToken, string expiresIn)
            : base(context)
        {
            User = user;
            AccessToken = accessToken;

            int expiresValue;
            if (int.TryParse(expiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue))
            {
                ExpiresIn = TimeSpan.FromSeconds(expiresValue);
            }

            Id = TryGetValue(user, "id");
            Name = TryGetValue(user, "name");

            ProfilePicture = TryGetValue( user, "picture" );
        }

        /// <summary>
        /// Gets the JSON-serialized user
        /// </summary>
        /// <remarks>
        /// Contains the Spotify user obtained from token ednpoint
        /// </remarks>
        public JObject User { get; private set; }

        /// <summary>
        /// Gets the Spotify access token
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets Spotify access token expiration time
        /// </summary>
        public TimeSpan? ExpiresIn { get; set; }

        /// <summary>
        /// Gets the Deezer user ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the user's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the Deezer users profile picture
        /// </summary>
        public string ProfilePicture { get; private set; }

        /// <summary>
        /// Gets the <see cref="ClaimsIdentity"/> representing the user
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }

        private static string TryGetValue(JObject user, string propertyName)
        {
            JToken value;
            return user.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }

        private static string TryGetListValue(JObject user, string listPropertyName, int listPosition, string listEntryPropertyName)
        {
            JToken listValue;
            var valueExists = user.TryGetValue(listPropertyName, out listValue);
            if (!valueExists) return null;
            var list = (JArray)listValue;
            
            if (list.Count <= listPosition) return null;
            var entry = list[listPosition];

            return entry.Value<string>(listEntryPropertyName);
        }
    }
}