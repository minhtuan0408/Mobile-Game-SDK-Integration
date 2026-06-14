using Firebase.Auth;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
	[SerializeField] private GameObject loginPanel;

	private void OnEnable()
	{
		LoginManager.Instance.OnLoginSuccess += HandleLoginSuccess;
	}

	private void OnDisable()
	{
		LoginManager.Instance.OnLoginSuccess -= HandleLoginSuccess;
	}

	private void HandleLoginSuccess(FirebaseUser user)
	{
		loginPanel.SetActive(false);

		SceneManager.LoadScene("GamePlay");
	}

	public void OnClickSignInAnonymously()
	{
		LoginManager.Instance.AnonymousLogin.SignInAnonymously();
	}
	public void OnClickSignInWithGoogle()
	{
		LoginManager.Instance.GoogleLogin.SignInWithGoogle();
	}
}