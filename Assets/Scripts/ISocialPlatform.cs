using UnityEngine;
using System;
using System.Collections.Generic;

public delegate void SocialCallback (object pRespond);

public delegate void SocialNoParamCallback ();

public delegate void SocialGetFriendListCallback (List<SocialUserInfo> pFriendList);

public delegate void SocialRecieveRequest (int pRespondCode,string pMessage,string pSender,object pDataDictionary);

public delegate void SocialSendGiftRequestCallback (List<SocialUserInfo> pFriendGotRequests);

public delegate void SocialUserPhotoReceive (string pSocialId,Texture pPhoto);

public abstract class ISocialPlatform
{
		
		public event SocialNoParamCallback onLoginSuccess;//onLoginSucess will be called in syncComleteCallback (sync data with server successfully)
		public event SocialNoParamCallback onLoginFailed;
		public event SocialNoParamCallback onLogout;
		public event SocialGetFriendListCallback onGetFriendList;
		public event SocialUserPhotoReceive onUserPhotoReceive;
		public event SocialRecieveRequest onRecieveRequest;
		public event SocialSendGiftRequestCallback onSendGiftRequetCallback;

		public abstract void Init ();
						
		public abstract  void Login ();

		public abstract  void Logout ();

		public abstract void FetchUserInfo (string SocialId);

		public abstract void AutoPostOnWall (string pHeading, string pCaption, string pMessage, string pDescription, string pBadgeIconURL, string pLinkURL);

		public abstract void GetAppFriend ();

		public abstract void SendAppRequest (string pMessage="", string pTitle="", List<SocialUserInfo> pSelectedFriends=null, Dictionary<string,string> data=null);

	public abstract void SendGiftToFriend (Dictionary<string,object> pGiftData, string pGiftText); 
		//public abstract void QueryIncomeRequest ();

		protected void ListFriendsCallback (List<SocialUserInfo> pFriendList)
		{
				if (onGetFriendList != null) {
						onGetFriendList (pFriendList);		
				}
		}

		protected void SendRequestCallback (List<SocialUserInfo> pFriendGotRequests)
		{
				if (onSendGiftRequetCallback != null) {
						onSendGiftRequetCallback (pFriendGotRequests);	
				}
		}

		protected void UserInfoCallback (bool isLoginSuccess, SocialUserInfo pUserInfo)
		{				
				if (isLoginSuccess) {
						// perform storing userdata, call ws..						
						Debug.Log ("Suzy Userinfo" + pUserInfo.ToString ());
						
				} else {
						LoginFailed ();
				}
		}

		protected void UserPhotoCallback (string pSocialId, Texture pPhoto)
		{
				if (onUserPhotoReceive != null) {
						onUserPhotoReceive (pSocialId, pPhoto);
				}
		}

		protected void RequestRecieved ()
		{
				if (onRecieveRequest != null) {
//					onRecieveRequest(
				}
		}

		protected void LoginSuccess (string pUserID)
		{
				FetchUserInfo (pUserID);
				if (onLoginSuccess != null) {
						onLoginSuccess ();
				}
				
		}

		protected void LoginFailed ()
		{
				if (onLoginFailed != null) {
						onLoginFailed ();
				}
		}
		


}

#region Social User Info
public class SocialUserInfo
{
		public SocialUserInfo ()
		{
				this.SocialId = "";
				this.Name = "";
				this.Score = 0;
				this.PhotoUrl = "";

		}

		public SocialUserInfo (string id, string name, long score, string photoUrl)
		{
				this.SocialId = id;
				this.Name = name;
				this.Score = score;
				this.PhotoUrl = photoUrl;
		}

		public string SocialId{ get; set; }

		public string Name { get; set; }

		public long Score { get; set; }

		public string PhotoUrl { get; set; }

		public override string ToString ()
		{
				return string.Format ("[SocialUserInfo: SocialId={0}, Name={1}, Score={2}, PhotoUrl={3}]", SocialId, Name, Score, PhotoUrl);
		}
}
#endregion
