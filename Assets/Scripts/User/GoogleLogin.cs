using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
	private FirebaseAuth auth;

	// Lấy từ Firebase Console
	private string webClientId =
		"119536262468-54ns270348bo2r0958euu0621dabks7c.apps.googleusercontent.com";

	private string idToken;
	private bool errorMessage;
	private void Start()
	{
		auth = FirebaseAuth.DefaultInstance;

		GoogleSignIn.Configuration = new GoogleSignInConfiguration
		{
			WebClientId = webClientId,
			RequestIdToken = true,
			RequestEmail = true
		};
	}

	public void SignInWithGoogle()
	{
		Debug.Log("Step 1");
		Debug.Log(GoogleSignIn.Configuration.WebClientId);

		var signInTask = GoogleSignIn.DefaultInstance.SignIn();


		Debug.Log("Step 2");

		signInTask.ContinueWith(task =>
		{
			Debug.Log("Step 3");
			OnGoogleAuthenticated(task);
		});
		//signInTask.ContinueWithOnMainThread(task =>
		//{
		//	OnGoogleAuthenticated(task);
		//});
	}

	private void OnGoogleAuthenticated(
		System.Threading.Tasks.Task<GoogleSignInUser> task)
	{
		if (task.IsFaulted)
		{
			Debug.LogError("Lỗi : " + task.Exception);
			return;
		}

		if (task.IsCanceled)
		{
			Debug.Log("Google Sign-In canceled");
			return;
		}

		GoogleSignInUser googleUser = task.Result;

		Credential credential =
			GoogleAuthProvider.GetCredential(
				googleUser.IdToken,
				null);

		auth.SignInAndRetrieveDataWithCredentialAsync(credential)
			.ContinueWith(task =>
			{
				if (task.IsCanceled)
				{
					Debug.LogError("Canceled");
					return;
				}

				if (task.IsFaulted)
				{
					Debug.LogError(task.Exception);
					return;
				}

				AuthResult result = task.Result;

				Debug.Log(result.User.DisplayName);
			});
	}

	public void SignOut()
	{
		auth.SignOut();
		GoogleSignIn.DefaultInstance.SignOut();
	}
}