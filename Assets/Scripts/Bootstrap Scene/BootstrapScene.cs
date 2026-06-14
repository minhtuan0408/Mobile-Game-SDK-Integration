using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BootstrapScene : MonoBehaviour
{
	[SerializeField] LoginManager loginManager;
	private bool loginReady;
	private void Start()
	{
		SceneManager.LoadScene("Login");

	}

}