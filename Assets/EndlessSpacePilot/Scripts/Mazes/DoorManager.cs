using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class DoorManager : MonoBehaviour
	{
		///***********************************************************************
		/// left/right Doors (maze) class.
		///***********************************************************************

		//DIR 1=right/-1=left
		public int doorDir = 1;

		//Leds (Green for open and red for closed state)
		public GameObject greenLED;
		public GameObject redLED;

		//SFXs
		public AudioClip doorAlarm;
		public AudioClip doorClose;
		// Private Variables
		private bool doorCanOpen;       //Flag to open the door just once
		private float doorOpenDelay;        //delay after letting the door to open
		private float closingSpeed;     //door closing speed (the bigger, the harder the game will be)
		private float closingPosition;  //hardcoded value for left/right doors positions

		void Awake()
		{
			init();
		}

		void init()
		{

			//when we advance in the game, doors should move and close faster. 
			//They also should be cloned at a higher position for maximum effectiveness.
			doorOpenDelay = Random.Range(0.1f + (0.4f / GameController.current_level), (1 / GameController.current_level));
			float coef = GameController.current_level;
			closingSpeed = Random.Range(coef / 5.0f, 1 + (coef / 3.0f));
			//print(closingSpeed + " - between: " + coef/5 + " AND " + (1 + (coef / 3.0f)) );

			//let the player know the door is getting closed.
			redLED.SetActive(false);
			greenLED.SetActive(true);

			//right hand doors
			if (doorDir == 1)
			{
				closingPosition = Random.Range(-1, -2.6f);
				transform.localPosition = new Vector3(Random.Range(2.9f, 3.3f),
													  transform.localPosition.y,
													  transform.localPosition.z);
			}
			else
			{ //left hand doors
				closingPosition = Random.Range(-2.85f, -5.0f);
				transform.localPosition = new Vector3(Random.Range(-8.75f, -8.50f),
													  transform.localPosition.y,
													  transform.localPosition.z);
			}

		}

		void Start()
		{
			StartCoroutine(doorSetup());
		}

		void Update()
		{
			doorManager();
		}


		///***********************************************************************
		/// if door can get opened, activate the leds, then move the door to it's
		/// destination, based on it being left or right side.
		///***********************************************************************
		void doorManager()
		{
			if (doorCanOpen)
			{
				greenLED.SetActive(false);
				redLED.SetActive(true);
				if (doorDir == 1)
				{
					transform.localPosition -= new Vector3(Time.deltaTime * closingSpeed,
														   0,
														   0);
					if (transform.localPosition.x < closingPosition)
						transform.localPosition = new Vector3(closingPosition,
															  transform.localPosition.y,
															  transform.localPosition.z);
				}
				else
				{
					transform.localPosition += new Vector3(Time.deltaTime * closingSpeed,
														   0,
														   0);
					if (transform.localPosition.x > closingPosition)
						transform.localPosition = new Vector3(closingPosition,
															  transform.localPosition.y,
															  transform.localPosition.z);
				}
			}
		}


		///***********************************************************************
		/// Blink door's led several times
		///***********************************************************************
		private bool playOnce = true;
		IEnumerator doorSetup()
		{
			yield return new WaitForSeconds(doorOpenDelay);
			playSfx(doorAlarm);

			for (int i = 0; i < 6; i++)
			{
				yield return new WaitForSeconds(0.1f);
				greenLED.GetComponent<Renderer>().enabled = !greenLED.GetComponent<Renderer>().enabled;
			}

			doorCanOpen = true;
			//play sfx
			if (playOnce)
			{
				playSfx(doorClose);
				playOnce = false;
			}
		}

		void playSfx(AudioClip _sfx)
		{
			GetComponent<AudioSource>().clip = _sfx;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}
	}
}