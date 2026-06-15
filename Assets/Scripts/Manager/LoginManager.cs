using System;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
	public static LoginManager Instance { get; private set; }

	public GoogleLogin GoogleLogin { get; private set; }
	public AnonymousLogin AnonymousLogin { get; private set; }

	public FirebaseUser CurrentUser { get; private set; }
	public string IdToken { get; private set; }
	public string UserId => CurrentUser?.UserId;
	public event Action<FirebaseUser> OnLoginSuccess;
	public event Action<string> OnLoginFailed;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		GoogleLogin = GetComponent<GoogleLogin>();
		AnonymousLogin = GetComponent<AnonymousLogin>();

		Initialize();
	}

	private void Initialize()
	{
		AnonymousLogin.InitializeFirebase();
	}
	public void LoginSuccess(FirebaseUser user, string idToken)
	{
		CurrentUser = user;
		IdToken = idToken;

		OnLoginSuccess?.Invoke(user);

	}

	public void LoginFailed(string error)
	{
		OnLoginFailed?.Invoke(error);
	}

	public void Logout()
	{
		FirebaseAuth.DefaultInstance.SignOut();

		CurrentUser = null;
		IdToken = null;
	}
}