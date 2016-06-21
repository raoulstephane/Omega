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
        [HttpGet]
        public async Task<string> GetAllFacebookEvents()
        {
            //List of events to return
            List<FacebookEvent> events = new List<FacebookEvent>();

            // Get the access token
            ClaimsIdentity claimsIdentity = await Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
            Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:facebook_access_token" );
            string accessToken = claim.Value;

            FacebookClient fbClient = new FacebookClient( accessToken );

            dynamic fbEvents = fbClient.Get( "me/events" );
            JObject facebookEventsJson = JObject.FromObject( fbEvents );
            
            IEnumerable<string> eventsId = facebookEventsJson["events"]["data"].Select( t => t.Value<string>( "id" ) );
            foreach(string id in eventsId)
            {
                dynamic eventDetailed = fbClient.Get( "me/" + id );
                JObject eventDetailedJson = JObject.FromObject( eventDetailed );
                string startTime = (string)eventDetailedJson["start_time"];
                startTime = startTime.Remove( 10 );
                string name = (string)eventDetailedJson["name"];

                dynamic eventPicture = fbClient.Get( "me/" + id + "/picture");
                JObject eventPictureJson = JObject.FromObject( eventPicture );
                string cover = (string)eventPictureJson["data"]["url"];

                FacebookEvent fbEvent = new FacebookEvent( id, name, cover );
                events.Add( fbEvent );
            }
            string allEvents = JsonConvert.SerializeObject( events );

            return allEvents;
        }

        /// <summary>
        /// Get all the user's facebook groups
        /// </summary>
        /// <returns> list of FacebookGroup </returns>
        //[HttpGet]
        //public List<FacebookGroup> GetAllFacebookGroups()
        //{
        //    FacebookClient fbClient = new FacebookClient();
        //    dynamic fbGroups = fbClient.Get( "me/groups" );
        //    JObject JObjFbGroups = JObject.FromObject( fbGroups );

        //    var groupNames = JObjFbGroups["groups"]["data"].Select( t => t.Value<string>( "name" ) );
        //    var groupId = JObjFbGroups["groups"]["data"].Select( t => t.Value<string>( "id" ) );

        //    List<string> listGroupNames = new List<string>( groupNames );
        //    List<string> listGroupId = new List<string>( groupId );

        //    List<FacebookGroup> listGroups = new List<FacebookGroup>();

        //    for (int i = 0; i < listGroupId.Count; i++)
        //    {
        //        FacebookGroup f = new FacebookGroup();
        //        f.id = listGroupId[0];
        //        f.name = listGroupNames[0];
        //        listGroups.Add( f );
        //    }
        //    return listGroups;
        //}
    }
}
