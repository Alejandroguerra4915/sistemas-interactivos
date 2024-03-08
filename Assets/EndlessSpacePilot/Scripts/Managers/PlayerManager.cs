using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.Networking;



namespace EndlessSpacePilot
{
    public class PlayerManager : MonoBehaviour
    {
        public Sprite[] availableHealthIcons;
        public int playerHealth = 3;
        public Image[] healthIcons;

        public static int playerScore = 0;

        public AudioClip eatSfx;
        public AudioClip hitSfx;

        public static bool reborn = false;

        public GameObject sparksFX;
        public Text scoreTextDynamic;

        public handler authManager; // Reference to the authentication manager

        private const string url = "https://sid-restapi.onrender.com"; // Assuming url is constant

        void Update()
        {
            if (!GameController.gameOver)
            calculateScore();

            StartCoroutine(Highscore(JsonUtility.ToJson(new HighscoreData { username = authManager.Username, score = playerScore })));
        }

        IEnumerator Highscore(string json)
        {
            UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
            request.method = "PATCH";
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-token", authManager.Token); // Accessing token from authentication manager

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
        }

        void calculateScore()
		{
			if (!PauseManager.isPaused)
			{
				playerScore += (GameController.current_level * (int)Mathf.Log(GameController.current_level + 1, 2));
				scoreTextDynamic.text = playerScore.ToString();
			}
		}

		///***********************************************************************
		/// Process and show player's health on screen
		///***********************************************************************
		void monitorPlayerHealth()
		{
			//Limiters
			if (playerHealth > 3)
				playerHealth = 3;
			if (playerHealth < 0)
				playerHealth = 0;
            Debug.Log("Player health: " + playerHealth);

            //show health icons
            for (int i = 0; i < playerHealth; i++)
			{
				//healthIcons[i].GetComponent<Renderer>().enabled = true;
				healthIcons[i].sprite = availableHealthIcons[1];
			}

			//hide lost health icons
			for (int j = playerHealth; j < 3; j++)
			{
				//healthIcons[j].GetComponent<Renderer>().enabled = false;
				healthIcons[j].sprite = availableHealthIcons[0];
			}
			

			//check for gameover state
			if (playerHealth <= 0)
			{
				GameController.gameOver = true;
				Debug.Log("Game Over");
				return;
			}
		}

		///***********************************************************************
		/// Blink the ship after a collision occured
		///***********************************************************************
		IEnumerator blinkAfterhit()
		{
            Debug.Log("Blinking after hit...");
            //activate blink state
            reborn = true;
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
												GetComponent<Renderer>().material.color.g,
												GetComponent<Renderer>().material.color.b,
												0.25f);
			for (int i = 0; i < 10; i++)
			{
				yield return new WaitForSeconds(0.1f);
				GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
												GetComponent<Renderer>().material.color.g,
												GetComponent<Renderer>().material.color.b,
												0.25f);
				yield return new WaitForSeconds(0.1f);
				GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
												GetComponent<Renderer>().material.color.g,
												GetComponent<Renderer>().material.color.b,
												0.85f);
			}
			reborn = false;
            Debug.Log("Blinking after hit finished.");
            yield break;
		}

		///***********************************************************************
		/// Collision managements
		///***********************************************************************
		void OnTriggerEnter(Collider other)
		{
            Debug.Log("Trigger entered: " + other.gameObject.name);
            //Normal state
            if (!reborn)
			{
				switch (other.gameObject.tag)
				{
					case "helperPlusLive":
						playSfx(eatSfx);
						playerHealth++;
						monitorPlayerHealth();
						Destroy(other.gameObject);
						break;

					default:
						playSfx(hitSfx);
						playerHealth--;
						

                        Debug.Log("Player health after hit: " + playerHealth);
                        monitorPlayerHealth();
                        //just for Android & iOS
#if UNITY_IPHONE || UNITY_ANDROID
					Handheld.Vibrate();
#endif

                        StartCoroutine(blinkAfterhit());
						break;
				}
			}
			else
			{ //If we collide with something while blinking...
			  //always eat good things ;)
				switch (other.gameObject.tag)
				{
					case "helperPlusLive":
						playSfx(eatSfx);
						playerHealth++;
						monitorPlayerHealth();
						Destroy(other.gameObject);
						break;
				}
			}
		}


		///***********************************************************************
		/// Make some hit particle effects
		///***********************************************************************
	

		void playSfx(AudioClip _sfx)
		{
			GetComponent<AudioSource>().clip = _sfx;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}

	}
    [System.Serializable]
    public class HighscoreData
    {
        public string username;
        public int score;
    }
}