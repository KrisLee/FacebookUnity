using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TestFb : MonoBehaviour
{
		
		ISocialPlatform socialNetwork;

		public TestFb ()
		{
				socialNetwork = SocialPlatformMrg.GetInstance ();
		}
		// Use this for initialization
		void Start ()
		{
				Debug.Log ("Start test project");	
				socialNetwork.onLoginSuccess += OnLoginSuccess;
				
				socialNetwork.Init ();						
		}

		// Update is called once per frame
		void Update ()
		{			
				if (Input.GetMouseButtonDown (0)) {

						socialNetwork.Login ();
						//socialNetwork.PostTextOnWall (DateTime.Now.ToString ());			
						//socialNetwork.AutoPostOnWall ("Proko", "Great site for tutorial", "Any one wanna join", "no idea", "http://playfury.com/Games/MineMania/AppIcon128.png", "http://www.proko.com/");
			socialNetwork.SendAppRequest(pMessage:"here is the request text",pTitle:"lelelelelelel");	
		}

		}

		void OnLoginSuccess ()
		{
				Debug.Log ("Suzy - On Login Success");
				socialNetwork.onGetFriendList += OnGetFriendList;
				//Test get app friends
				socialNetwork.GetAppFriend ();
				//Test post message on wall
				//socialNetwork.PostTextOnWall (DateTime.Now.ToString ());
				
		}

		void OnGetFriendList (List<SocialUserInfo> pFriendList)
		{
				Debug.Log ("Suzy get App Friends");
				foreach (var item in pFriendList) {
						Debug.Log ("Suzy Friend: " + item.ToString ());		
				}

		}
}
