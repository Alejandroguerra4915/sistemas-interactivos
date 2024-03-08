using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace EndlessSpacePilot
{
	public class GameoverManager : MonoBehaviour
	{
		///***********************************************************************
		/// GameOver Manager Class. 
		///***********************************************************************

		public static GameoverManager instance;
		public Text scoreText;            //reference to score gameobject to modify its text
		public AudioClip menuTap;

        private void Awake()
        {
			instance = this;
        }

        private void Start()
        {
			//SaveScore();
		}

        void Update()
		{
			//Set the new score on the screen
			scoreText.text = PlayerManager.playerScore.ToString();
		}


		///***********************************************************************
		/// Save player score
		///***********************************************************************
		public void SaveScore()
		{
			print("<b>Score Saved!</b>");

			//immediately save the last score
			PlayerPrefs.SetInt("lastScore", PlayerManager.playerScore);
			//check if this new score is higher than saved bestScore.
			//if so, save this new score into playerPrefs. otherwise keep the last bestScore intact.
			int lastBestScore;
			lastBestScore = PlayerPrefs.GetInt("bestScore");
			if (PlayerManager.playerScore > lastBestScore)
			{
              PlayerPrefs.SetInt("bestScore", PlayerManager.playerScore);
			  //GetComponent<ScoreManager>().ActualizarScore(PlayerManager.playerScore);
			}
				
		}


		///***********************************************************************
		/// IPlay audioclip
		///***********************************************************************
		void playSfx(AudioClip _sfx)
		{
			GetComponent<AudioSource>().clip = _sfx;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}

	}
}