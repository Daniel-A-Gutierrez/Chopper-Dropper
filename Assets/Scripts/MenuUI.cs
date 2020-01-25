using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
	public static bool isGamePaused = false;
	public GameObject pauseMenuUI;

	public void Start()
	{
		GetComponent<AudioManager>().Play("music");
	}
    public void PlayGame(string level_to_load)
	{
		//Application.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
		GetComponent<AudioManager>().Play("button_click");
		SceneManager.LoadScene(level_to_load);
		
	}
	public void QuitGame()
	{
		print("Quiting..");
		GetComponent<AudioManager>().Play("button_click");
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
