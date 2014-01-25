using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.MiniJSON;

public class Util : ScriptableObject
{
		public static string GetPictureURL (string facebookID, int? width = null, int? height = null, string type = null)
		{
				string url = string.Format ("/{0}/picture", facebookID);
				string query = width != null ? "&width=" + width.ToString () : "";
				query += height != null ? "&height=" + height.ToString () : "";
				query += type != null ? "&type=" + type : "";
				if (query != "")
						url += ("?g" + query);
				return url;// /me/picture?g&width=128&height=128
		}

		public static void FriendPictureCallback (FBResult result)
		{
				if (result.Error != null) {
						Debug.LogError (result.Error);
						return;
				}

				//        GameStateManager.FriendTexture = result.Texture;
		}

		public static Dictionary<string, string> RandomFriend (List<object> friends)
		{
				var fd = ((Dictionary<string, object>)(friends [Random.Range (0, friends.Count - 1)]));
				var friend = new Dictionary<string, string> ();
				friend ["id"] = (string)fd ["id"];
				friend ["first_name"] = (string)fd ["first_name"];
				return friend;
		}
		// Assuming that respond data is correct
		public static SocialUserInfo DeserializeJSONProfile (string response)
		{
				SocialUserInfo userInfo = new SocialUserInfo ();
				var responseObject = Json.Deserialize (response) as Dictionary<string, object>;
				object valueH;
				userInfo.Name = responseObject ["name"].ToString ();				
				userInfo.SocialId = responseObject ["id"].ToString ();
				
				if (responseObject.TryGetValue ("picture", out valueH)) {						
						object dataH;
						if (((Dictionary<string,object>)valueH).TryGetValue ("data", out dataH)) {								
								userInfo.PhotoUrl = ((Dictionary<string,object>)dataH) ["url"].ToString ();
						}
				}
				return userInfo;
		}
    	
		// Not implemented yet
		public static List<object> DeserializeScores (string response)
		{
				var responseObject = Json.Deserialize (response) as Dictionary<string, object>;
				object scoresh;
				var scores = new List<object> ();
				if (responseObject.TryGetValue ("data", out scoresh)) {
						scores = (List<object>)scoresh;
				}

				return scores;
		}
		// Not needed yet
		public static List<object> DeserializeJSONFriends (string response)
		{
				var responseObject = Json.Deserialize (response) as Dictionary<string, object>;
				object friendsH;
				var friends = new List<object> ();
				if (responseObject.TryGetValue ("friends", out friendsH)) {
						friends = (List<object>)(((Dictionary<string, object>)friendsH) ["data"]);
				}
				return friends;
		}
		
		public static List<SocialUserInfo> DeserializeJSONAppFriends (string response)
		{
				//Debug.Log ("Suzy  " + response);
				var responseObject = Json.Deserialize (response) as Dictionary<string, object>;
				object friendsH;
				List<SocialUserInfo> appFriends = new List<SocialUserInfo> ();
				if (responseObject.TryGetValue ("data", out friendsH)) {
						Debug.Log ("Suzy data: " + friendsH);
						var friends = (List<object>)friendsH;
						object isAppFriend;
						foreach (Dictionary<string,object> item in friends) {								
								if (((Dictionary<string,object>)item).TryGetValue ("installed", out isAppFriend)) {
										if ((bool)isAppFriend) {												
												SocialUserInfo friendInfo = new SocialUserInfo ();
												friendInfo.SocialId = item ["id"].ToString ();												
												friendInfo.Name = item ["name"].ToString ();												
												object valueH;
												if (item.TryGetValue ("picture", out valueH)) {						
														object dataH;
														if (((Dictionary<string,object>)valueH).TryGetValue ("data", out dataH)) {								
																friendInfo.PhotoUrl = ((Dictionary<string,object>)dataH) ["url"].ToString ();
														}
												}
												appFriends.Add (friendInfo);
										}
								}

						}
				}
				return appFriends;
		}
    
		public static void DrawActualSizeTexture (Vector2 pos, Texture texture, float scale = 1.0f)
		{
				Rect rect = new Rect (pos.x, pos.y, texture.width * scale, texture.height * scale);
				GUI.DrawTexture (rect, texture);
		}

		public static void DrawSimpleText (Vector2 pos, GUIStyle style, string text)
		{
				Rect rect = new Rect (pos.x, pos.y, Screen.width, Screen.height);
				GUI.Label (rect, text, style);
		}
}
