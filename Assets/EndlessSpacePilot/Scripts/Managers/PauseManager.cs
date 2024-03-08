using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace EndlessSpacePilot
{
	public class PauseManager : MonoBehaviour
	{
		//***************************************************************************//
		// This class manages pause and unpause states.
		//***************************************************************************//
		public static bool isPaused;
		private float savedTimeScale;
		public GameObject pausePlane;

		enum Page
		{
			PLAY, PAUSE
		}
		private Page currentPage = Page.PLAY;

		void Awake()
		{
			isPaused = false;

			Time.timeScale = 1.0f;

			if (pausePlane)
				pausePlane.SetActive(false);
		}

		void Update()
		{
			//optional pause
			if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape))
			{
				//PAUSE THE GAME
				switch (currentPage)
				{
					case Page.PLAY:
						PauseGame();
						break;
					case Page.PAUSE:
						UnPauseGame();
						break;
					default:
						currentPage = Page.PLAY;
						break;
				}
			}

			//debug restart
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}


		void PauseGame()
		{
			print("Game in Paused...");
			isPaused = true;
			savedTimeScale = Time.timeScale;
			Time.timeScale = 0;
			AudioListener.volume = 0;
			if (pausePlane)
				pausePlane.SetActive(true);
			currentPage = Page.PAUSE;
		}

		void UnPauseGame()
		{
			print("Unpause");
			isPaused = false;
			Time.timeScale = savedTimeScale;
			AudioListener.volume = 1.0f;
			if (pausePlane)
				pausePlane.SetActive(false);
			currentPage = Page.PLAY;
		}


		public void ClickOnPauseButton()
        {
			PauseGame();
        }


		public void ClickOnResumeButton()
		{
			UnPauseGame();
		}

		public void ClickOnMenuButton()
		{
			SceneManager.LoadScene("Menu");
		}

		public void ClickOnRestartButton()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

	}
}