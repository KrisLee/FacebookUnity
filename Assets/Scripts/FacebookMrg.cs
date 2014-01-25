using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;

public delegate void OnRequesrCallBack (Object respond);

public class FacebookMrg : ISocialPlatform
{
		
		
		
		private SocialUserInfo mUserInfo = new SocialUserInfo ();
		private static List<object>                 scores = null;
		private static Dictionary<string, Texture>  friendImages = new Dictionary<string, Texture> ();
		private OnRequesrCallBack mOnLoginCallback;

		public FacebookMrg ()
		{
		}
	#region Override Method
		public override void Init ()
		{
				FB.Init (SetInit);
		}

		public override void Login ()
		{
				if (!FB.IsLoggedIn) {
						FB.Login ("email,publish_actions", LoginCallback);
				}
		}

		public override void Logout ()
		{
				throw new System.NotImplementedException ();
		}

		public override void FetchUserInfo (string SocialId)
		{
				Debug.Log ("Suzy - FetchUserInfo");
				if (FB.IsLoggedIn) {
						FB.API ("/me/?fields=id,name,picture.width(128).height(128)", Facebook.HttpMethod.GET, InternalUserInfoCallback);
//						FB.API (Util.GetPictureURL ("me", 128, 128), Facebook.HttpMethod.GET, UserInfoAndPhotoCallback);	
				}
		}

		public override void AutoPostOnWall (string pHeading="", string pCaption="", string pMessage="", string pDescription="", string pBadgeIconURL="", string pLinkURL="")
		{
				Debug.Log ("Suzy On POst test wall");
				//FB.Feed (link: "www.google.com", linkName: "Google", linkCaption: pMessage, linkDescription: "description"); //Can not use it because it will open a dialog
				Dictionary<string,string> postdata = new Dictionary<string, string> ();
//		Dictionary<string,string> linkData = new Dictionary<string, string> ();

				postdata.Add ("name", pHeading);
				postdata.Add ("caption", pCaption);
				postdata.Add ("message", pMessage);
				postdata.Add ("description", pDescription);
				postdata.Add ("link", pLinkURL);
				postdata.Add ("picture", pBadgeIconURL);

				FB.API ("/me/feed", method: Facebook.HttpMethod.POST, formData: postdata);
		}

		public override void GetAppFriend ()
		{
				Debug.Log ("Suzy On getapp Friend");
				if (FB.IsLoggedIn) {
						Debug.Log ("Suzy IsLoggedIn");
						FB.API ("/me/friends?fields=id,picture.height(128).width(128),installed,name", Facebook.HttpMethod.GET, InternalGetAppFriendCallback);
				}
		}

		public override void SendAppRequest (string pMessage="", string pTitle="", List<SocialUserInfo> pSelectedFriends=null, Dictionary<string, string> data=null)
		{
				Debug.Log ("Send app request");
				List<string> friendID = new List<string> ();
				if (pSelectedFriends != null) {
						foreach (var item in pSelectedFriends) {
								friendID.Add (item.SocialId);
						}
						FB.AppRequest (message: pMessage, to: friendID.ToArray (), filters: "", excludeIds: null, maxRecipients: null, data: "asdasd", title: pTitle, callback: appRequestCallback);						
				} else {						
						FB.AppRequest (message: pMessage, title: pTitle, callback: appRequestCallback, data: "adasd");
				}
		}

		public override void SendGiftToFriend (Dictionary<string, object> pGiftData, string pGiftText)
		{
				//FB.AppRequest (message: pGiftText, title: "You recieved gift !",callback:appRequestCallback ,data: "adasd");
				Dictionary<string,string> data = new Dictionary<string, string> ();
				data.Add ("message", "This is a test message");
				data.Add ("data", "asdasdasdasdasdas");
				FB.API ("/me/apprequests", Facebook.HttpMethod.POST, GiftCallback, data);
		}


	#endregion





	#region Private Method
		private void SetInit ()
		{
				FbDebug.Log ("Suzy - SetInit");

				if (FB.IsLoggedIn) {
						FbDebug.Log ("Suzy - Already logged in");
						LoginSuccess (FB.UserId);
				}
		}

		// This callback send client the login's information
		private void LoginCallback (FBResult result)
		{
				Debug.Log ("Suzy - LoginCallback");
		
				if (FB.IsLoggedIn) {
						LoginSuccess (FB.UserId);
				} else {
						LoginFailed ();
				}
				
		}

		private void InternalGetAppFriendCallback (FBResult result)
		{
				Debug.Log ("Suzy - InternalGetAppFriendCallback");
				if (result.Error != null) {
						//try again
						Debug.LogError (result.Error);
						FB.API ("/me/friends?fields=id,picture.height(128).width(128),installed,name", Facebook.HttpMethod.GET, InternalGetAppFriendCallback);
						return;
				}
				List<SocialUserInfo> appFriends = Util.DeserializeJSONAppFriends (result.Text);	
				ListFriendsCallback (appFriends);
		}

		private void InternalUserInfoCallback (FBResult result)
		{
				if (result.Error != null) {
						FbDebug.Error ("Suzy error: " + result.Error);
						// Let's just try again
						FB.API ("/me/?fields=id,name,picture.width(128).height(128)", Facebook.HttpMethod.GET, InternalUserInfoCallback);
						return;
				}		
				mUserInfo = Util.DeserializeJSONProfile (result.Text);
				UserInfoCallback (true, mUserInfo);//TODO need to modified score !!!
		}

		private void appRequestCallback (FBResult result)
		{
				// Callback when some friend send me gift/or something
				// we have to process the data store in the respond, save that gift somewhere.
				// when we comsume it, delete that request

				Debug.Log ("suzy appRequestCallback");
				if (result != null) {
						var responseObject = Json.Deserialize (result.Text) as Dictionary<string, object>;
						object obj = 0;
						if (responseObject.TryGetValue ("cancelled", out obj)) {
								Debug.Log ("Suzy Request cancelled");
						} else if (responseObject.TryGetValue ("request", out obj)) {
								// Record that we went sent a request so we can display a message
								//lastChallengeSentTime = Time.realtimeSinceStartup;
								Debug.Log ("Suzy Request sent");
								Debug.Log ("Suzy Respond value :" + responseObject.ToString ());
						}
				}
		}

		private void  GiftCallback (FBResult result)
		{
				Debug.Log ("suzy appRequestCallback");
				if (result != null) {
						var responseObject = Json.Deserialize (result.Text) as Dictionary<string, object>;
						Debug.Log ("Suzy repsond : " + responseObject.ToString ());
				}
		}
	#endregion
	
		//		void UserInfoAndPhotoCallback (FBResult result)
		//		{
		//				Debug.Log ("Suzy - MyPictureCallback");
		//		
		//				if (result.Error != null) {
//						//FbDebug.Error (result.Error);
//						// Let's just try again
//						FB.API (Util.GetPictureURL ("me", 128, 128), Facebook.HttpMethod.GET, UserInfoAndPhotoCallback);
//						return;
//				}
//				UserPhotoCallback (result.Texture);
//
//		}

}
