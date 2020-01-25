using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
	public static bool isGamePaused = false;
	public GameObject pauseMenuUI;
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

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if (isGamePaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}
	void Pause()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		isGamePaused = true;
	}
	void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		isGamePaused = false;
	}
}
