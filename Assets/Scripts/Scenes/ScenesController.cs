using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
	public class ScenesController : MonoBehaviour
	{
		public void SetMain()
		{
			SceneManager.LoadScene("Scenes/Main");
		}
	}

}
