using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SocialPlatformMrg: MonoBehaviour
{
		public enum SocialPlatforms
		{
				Facebook,
				Twitter
		}
		;		
		

		// Change this later if we want to implement for anyother Social network
		private static SocialPlatforms mPlatform = SocialPlatforms.Facebook;
		private static ISocialPlatform mInstance = null;
		private static Object lockingObject = new Object ();

		public event SocialCallback UserInfoCallback;
	
		public static ISocialPlatform GetInstance ()
		{
				if (mInstance == null) {
						lock (lockingObject) {
								if (mInstance == null) {
										mInstance = CreateSocialPlatform (mPlatform);
								}
						}
				}
				return mInstance;
		}
		
		// Factory method to create Social Instance. Need to extend if we want to add more Social platform
		// By default will return Facebook instance.
		private static ISocialPlatform CreateSocialPlatform (SocialPlatforms pPlatform)
		{
				switch (pPlatform) {
				default:
						{
								Debug.Log ("SocialPlatformMrg CreateSocialPlatform:48");
								return new FacebookMrg ();								
						}
				}
		}



}
