using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace EndlessSpacePilot
{
	public class MenuManager : MonoBehaviour
	{
		///*************************************************************************///
		/// Main Menu Buttons Controller.
		///*************************************************************************///

		private int bestScore;
		private int lastScore;
		public Text bestScoreText;     //we just need the textmesh component
		public Text lastScoreText;     // ""       ""        ""       ""
		private int controlType = 0;         // 0=Tilt , 1=Touch
		public Text controlTypeText;
		public AudioClip menuTap;


		void Awake()
		{
			//PlayerPrefs.DeleteAll();

			Time.timeScale = 1f;
			AudioListener.volume = 1.0f;

			bestScore = PlayerPrefs.GetInt("bestScore");
			bestScoreText.text = bestScore.ToString();

			lastScore = PlayerPrefs.GetInt("lastScore");
			lastScoreText.text = lastScore.ToString();

			//fetch previous controlType set by player, instead of resetting it everytime
			controlType = PlayerPrefs.GetInt("controlType");
			if (controlType == 0)
			{
				controlTypeText.text = "Control: Tilt";
			}
			else
			{
				controlTypeText.text = "Control: Touch";
			}
		}

		void Start()
		{
			//prevent screenDim in handheld devices
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}


		///***********************************************************************
		/// play audio clip
		///***********************************************************************
		void playSfx(AudioClip _sfx)
		{
			GetComponent<AudioSource>().clip = _sfx;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}


		public void ClickOnMoreGamesButton()
        {
			Application.OpenURL("https://www.finalbossgame.com/");
        }


		public void ClickOnControlTypeButton()
		{
			playSfx(menuTap);

			if (controlType == 0)
			{
				controlType = 1;
				PlayerPrefs.SetInt("controlType", controlType);
				controlTypeText.text = "Control: Touch";
			}
			else
			{
				controlType = 0;
				PlayerPrefs.SetInt("controlType", controlType);
				controlTypeText.text = "Control: Tilt";
			}
		}

		public void ClickOnStartButton()
		{
			SceneManager.LoadScene("Game");
		}

		public void ClickOnExitButton()
		{
			Application.Quit();
		}

	}
}