using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class AnonymousLogin : MonoBehaviour
{
	private FirebaseAuth auth;


	public void InitializeFirebase()
	{
		auth = FirebaseAuth.DefaultInstance;
	}
	public void SignInAnonymously()
	{
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
		auth.SignInAnonymouslyAsync().ContinueWith(task =>
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

			var result = task.Result;

			FirebaseUser user = result.User;

			LoginManager.Instance.LoginSuccess(user, null);
			Debug.Log("Anonymous login success!");
			Debug.Log("User ID: " + user.UserId);
		});
	}

}