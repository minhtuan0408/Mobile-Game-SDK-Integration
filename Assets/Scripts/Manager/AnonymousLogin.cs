using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class AnonymousLogin : MonoBehaviour
{
	private FirebaseAuth auth;
	public void InitializeFirebase()
	{
		auth = FirebaseAuth.DefaultInstance;
	}
	public void SignInAnonymously()
	{
		Debug.Log("auth = " + auth);
		Debug.Log("current user = " + auth?.CurrentUser);
		if (auth == null)
		{
			Debug.LogError("FirebaseAuth not initialized!");
			return;
		}
		if (auth.CurrentUser != null)
		{
			Debug.Log("Already signed in.");
			SceneManager.LoadScene("GamePlay");
			return;
		}
		auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
		{
			Debug.Log("Callback fired");

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

			FirebaseUser user = task.Result.User;

			Debug.Log("Anonymous login success!");
			Debug.Log("User ID: " + user.UserId);

			LoginManager.Instance.LoginSuccess(user, null);
		});
	}

}