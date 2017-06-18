using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public class ScenesController : MonoBehaviour
	{
		public void SetScene(string sceneName)
		{
			SceneManager.LoadScene("Scenes/" + sceneName);
		}
	}

}
