using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
	[SerializeField] private GameObject loginPanel;

	[SerializeField] private GameObject loginButton;
	[SerializeField] private GameObject playButton;

	private void Start()
	{
		RefreshUI();
	}

	private void OnEnable()
	{
		if (LoginManager.Instance != null)
		{
			LoginManager.Instance.OnLoginSuccess += HandleLoginSuccess;
		}
	}

	private void OnDisable()
	{
		if (LoginManager.Instance != null)
		{
			LoginManager.Instance.OnLoginSuccess -= HandleLoginSuccess;
		}
	}

	private void RefreshUI()
	{
		bool loggedIn = LoginManager.Instance != null &&
						LoginManager.Instance.IsLoggedIn;

		loginButton.SetActive(!loggedIn);
		playButton.SetActive(loggedIn);
	}

	private void HandleLoginSuccess(FirebaseUser user)
	{
		RefreshUI();
	}

	public void OnClickSignInAnonymously()
	{
		LoginManager.Instance.AnonymousLogin.SignInAnonymously();
	}

	public void OnClickSignInWithGoogle()
	{
		LoginManager.Instance.GoogleLogin.SignInWithGoogle();
	}

	public void OnClickLogout()
	{
		LoginManager.Instance.Logout();
		RefreshUI();
	}

	public void PlayButton()
	{
		SceneManager.LoadScene("GamePlay");
	}
}