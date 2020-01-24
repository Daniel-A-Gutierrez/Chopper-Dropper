using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
    public void PlayGame()
	{
		//Application.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		SceneManager.LoadScene("santi");
	}
	public void QuitGame()
	{
		print("Quiting..");
		Application.Quit();
	}
}
