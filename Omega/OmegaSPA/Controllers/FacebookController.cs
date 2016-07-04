using Facebook;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using OmegaSPA.ModelsFacebook;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;
using Omega.DataManager;

namespace OmegaSPA.Controllers
{
    public class FacebookController : ApiController
    {
        /// <summary>
        /// Get the user's basic informations :
        ///     email,
        ///     first name,
        ///     last name,
        ///     id
        /// </summary>
        /// <returns> FacebookUser </returns>
        [HttpGet]
        public FacebookUser GetCurrentFacebookUser()
        {
            FacebookClient fbClient = new FacebookClient();
            dynamic fbUser = fbClient.Get( "me?fields=email,first_name,last_name" );
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookClient>( fbUser.ToString() );
        }

        /// <summary>
        /// Get all the user's facebook events
        /// </summary>
        /// <returns>List of FacebookEvents</returns>
        [Route( "Facebook/events" )]
        public async Task<JToken> GetAllFacebookEvents()
        {
            //List of events to return
            List<FacebookEvent> events = new List<FacebookEvent>();

            // Get the access token
            ClaimsIdentity claimsIdentity = await Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:user_email" );
            string email = claim.Value;
            string accessToken = DatabaseQueries.GetFacebookAccessTokenByEmail( email );

            FacebookClient fbClient = new FacebookClient( accessToken );

            dynamic fbEvents = fbClient.Get( "me/events" );
            JObject facebookEventsJson = JObject.FromObject( fbEvents );
            foreach( var dataEvent in facebookEventsJson["data"])
            {
                string eventId = (string)dataEvent["id"];
                dynamic eventDetailed = fbClient.Get( eventId );
                JObject eventDetailedJson = JObject.FromObject( eventDetailed );
                string startTime = (string)eventDetailedJson["start_time"];
                startTime = startTime.Remove( 10 );
                string name = (string)eventDetailedJson["name"];


                Uri eventDetailUri = new Uri(
                    string.Format( "https://graph.facebook.com/{0}/picture?fields=url&access_token={1}&format=json&redirect=false",
                    eventId,
                    accessToken ));
                HttpWebRequest fbEventsProfilePicture = (HttpWebRequest)HttpWebRequest.Create( eventDetailUri );
                
                string cover;
                using (WebResponse response = await fbEventsProfilePicture.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader( responseStream ))
                {
                    string eventPictureJson = reader.ReadToEnd();
                    JObject eventJson = JObject.Parse( eventPictureJson );
                    cover = (string)eventJson["data"]["url"];
                }

                FacebookEvent fbEvent = new FacebookEvent( eventId, name, cover );
                events.Add( fbEvent );
            }
            
            string eventsString = JsonConvert.SerializeObject( events );
            JToken eventsJson = JToken.Parse( eventsString );
            return eventsJson;
        }

        /// <summary>
        /// Get all the user's facebook groups
        /// </summary>
        /// <returns> list of FacebookGroup </returns>
        [Route( "Facebook/groups" )]
        public async Task<JToken> GetAllFacebookGroups()
        {
            List<FacebookGroup> allGroups = new List<FacebookGroup>();

            ClaimsIdentity claimsIdentity = await Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:user_email" );

            string email = claim.Value;
            string accessToken = DatabaseQueries.GetFacebookAccessTokenByEmail( email );
            //string fbId = DatabaseQueries.GetFacebookIdByEmail( email );

            FacebookClient fbClient = new FacebookClient( accessToken );
            dynamic fbGroups = fbClient.Get( "https://graph.facebook.com/me/groups" );
            JObject fbGroupsJson = JObject.FromObject( fbGroups );

            foreach( var dataGroup in fbGroupsJson["data"] )
            {
                string groupId = (string)dataGroup["id"];
                string groupName = (string)dataGroup["name"];
                string groupCover;

                // Uri eventDetailUri = new Uri( "https://graph.facebook.com/" + groupId + "/picture?fields=url&access_token=" + accessToken + "&format=json" );
                Uri eventDetailUri = new Uri( string.Format(
                    "https://graph.facebook.com/v2.6/{0}/picture?fields=url&access_token={1}&format=json&redirect=false",
                    groupId,
                    accessToken));
                HttpWebRequest fbGroupPicture = (HttpWebRequest)HttpWebRequest.Create( eventDetailUri );
                using (WebResponse response = await fbGroupPicture.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader( responseStream ))
                {
                    string eventPictureJson = reader.ReadToEnd();
                    JObject eventJson = JObject.Parse( eventPictureJson );
                    groupCover = (string)eventJson["data"]["url"];
                }
                FacebookGroup fbGroup = new FacebookGroup( groupId, groupName, groupCover );
                allGroups.Add( fbGroup );
            }
            string groupsString = JsonConvert.SerializeObject( allGroups );
            JToken groupsJson = JToken.Parse( groupsString );

            return groupsJson;
        }

        [Route( "Facebook/group/{groupId}/playlistsGroup")]
        public async Task<JToken> GetAllPlaylistsFromGroup( string groupId )
        {
            List<PlaylistEntity> AllPlaylistsFromGroupMembers = new List<PlaylistEntity>();

            ClaimsIdentity claimsIdentity = await Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:user_email" );
            string email = claim.Value;
            string accessToken = DatabaseQueries.GetFacebookAccessTokenByEmail( email );

            FacebookClient fbClient = new FacebookClient( accessToken );
            dynamic groupMembers = fbClient.Get( string.Format( "{0}/members", groupId ) );
            JObject groupMembersJson = JObject.FromObject( groupMembers );
            
            foreach( var member in groupMembersJson["data"])
            {
                string currentMemberId = (string)member["id"];
                string fbUserEmail = DatabaseQueries.GetEmailByFacebookId( currentMemberId );

                if (fbUserEmail != null)
                {
                    if (DatabaseQueries.IsUserPresentInBase( fbUserEmail ))
                    {
                        AllPlaylistsFromGroupMembers.AddRange( DatabaseQueries.GetAllPlaylistsFromOwner( fbUserEmail ) );
                    }
                }
            }
            string allPlaylistsString = JsonConvert.SerializeObject( AllPlaylistsFromGroupMembers );
            JToken allPlaylistsJson = JToken.Parse( allPlaylistsString );
            return allPlaylistsJson;
        }

        [Route( "Facebook/event/{eventId}/playlistsEvent" )]
        public async Task<JToken> GetAllPlaylistsFromEvent( string eventId )
        {
            List<PlaylistEntity> AllPlaylistsFromGroupMembers = new List<PlaylistEntity>();

            ClaimsIdentity claimsIdentity = await Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:user_email" );
            string email = claim.Value;
            string accessToken = DatabaseQueries.GetFacebookAccessTokenByEmail( email );

            FacebookClient fbClient = new FacebookClient( accessToken );
            dynamic eventMembers = fbClient.Get( string.Format( "{0}/attending", eventId ) );
            JObject eventMembersJson = JObject.FromObject( eventMembers );

            foreach (var member in eventMembersJson["data"])
            {
                string currentMemberId = (string)member["id"];
                string fbUserEmail = DatabaseQueries.GetEmailByFacebookId( currentMemberId );

                if (fbUserEmail != null)
                {
                    if (DatabaseQueries.IsUserPresentInBase( fbUserEmail ))
                    {
                        AllPlaylistsFromGroupMembers.AddRange( DatabaseQueries.GetAllPlaylistsFromOwner( fbUserEmail ) );
                    }
                }
            }
            string allPlaylistsString = JsonConvert.SerializeObject( AllPlaylistsFromGroupMembers );
            JToken allPlaylistsJson = JToken.Parse( allPlaylistsString );
            return allPlaylistsJson;
        }
    }
}
