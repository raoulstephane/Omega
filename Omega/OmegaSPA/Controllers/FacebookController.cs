using Facebook;
using Newtonsoft.Json.Linq;
using OmegaSPA.ModelsFacebook;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

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
        public List<FacebookEvent> GetAllFacebookEvents()
        {
            FacebookClient fbClient = new FacebookClient();
            dynamic fbEvents = fbClient.Get( "me/events" );
            JObject JObjFbEvents = JObject.FromObject( fbEvents );

            var eventNames = JObjFbEvents["events"]["data"].Select( t => t.Value<string>( "name" ) );
            var eventId    = JObjFbEvents["events"]["data"].Select( t => t.Value<string>( "id" ) );

            List<string> listEventNames = new List<string>( eventNames );
            List<string> listEventId = new List<string>( eventId );

            List<FacebookEvent> listEvents = new List<FacebookEvent>();

            for( int i = 0; i < listEventId.Count; i++)
            {
                FacebookEvent f = new FacebookEvent();
                f.id = listEventId[0];
                f.name = listEventNames[0];
                listEvents.Add( f );
            }

            return listEvents;
        }

        /// <summary>
        /// Get all the user's facebook groups
        /// </summary>
        /// <returns> list of FacebookGroup </returns>
        [HttpGet]
        public List<FacebookGroup> GetAllFacebookGroups()
        {
            FacebookClient fbClient = new FacebookClient();
            dynamic fbGroups = fbClient.Get( "me/groups" );
            JObject JObjFbGroups = JObject.FromObject( fbGroups );

            var groupNames = JObjFbGroups["groups"]["data"].Select( t => t.Value<string>( "name" ) );
            var groupId = JObjFbGroups["groups"]["data"].Select( t => t.Value<string>( "id" ) );

            List<string> listGroupNames = new List<string>( groupNames );
            List<string> listGroupId = new List<string>( groupId );

            List<FacebookGroup> listGroups = new List<FacebookGroup>();

            for (int i = 0; i < listGroupId.Count; i++)
            {
                FacebookGroup f = new FacebookGroup();
                f.id = listGroupId[0];
                f.name = listGroupNames[0];
                listGroups.Add( f );
            }
            return listGroups;
        }
    }
}
