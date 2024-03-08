using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class TwinDoorManager : MonoBehaviour
	{
		///***********************************************************************
		///  bi-directional Doors (maze) class.
		///***********************************************************************

		/* Public Variables */
		public GameObject greenLED;
		public GameObject redLED;
		public GameObject rightDoorObject;
		public GameObject leftDoorObject;

		/* Private Variables */
		private bool doorCanOpen;
		private float doorOpenDelay;
		private float closingSpeed;

		//sfx
		public AudioClip doorAlarm;
		public AudioClip doorClose;

		void Awake()
		{
			init();
		}

		void init()
		{
			doorOpenDelay = Random.Range(0.1f, (1 / GameController.current_level));
			float coef = GameController.current_level;
			closingSpeed = Random.Range(coef / 5.0f, 0.85f + (coef / 3.5f));
			//print(closingSpeed + " - between: " + coef/5 + " AND " + (1 + (coef / 3.5f)) );

			redLED.SetActive(false);
			greenLED.SetActive(true);

			//Hardcoded values
			rightDoorObject.transform.localPosition = new Vector3(8.32f,
																  rightDoorObject.transform.localPosition.y,
																  rightDoorObject.transform.localPosition.z);
			leftDoorObject.transform.localPosition = new Vector3(-3.84f,
																 leftDoorObject.transform.localPosition.y,
																 leftDoorObject.transform.localPosition.z);
		}

		void Start()
		{
			StartCoroutine(doorSetup());
		}

		void Update()
		{
			doorManager();
		}

		void doorManager()
		{
			if (doorCanOpen)
			{
				greenLED.SetActive(false);
				redLED.SetActive(true);

				//Right Door
				rightDoorObject.transform.localPosition -= new Vector3(Time.deltaTime * closingSpeed,
																	   0,
																	   0);
				if (rightDoorObject.transform.localPosition.x < 5.87f)
					rightDoorObject.transform.localPosition = new Vector3(5.87f,
																		  rightDoorObject.transform.localPosition.y,
																		  rightDoorObject.transform.localPosition.z);


				//Left Door
				leftDoorObject.transform.localPosition += new Vector3(Time.deltaTime * closingSpeed,
																	  0,
																	  0);
				if (leftDoorObject.transform.localPosition.x > -1.36f)
					leftDoorObject.transform.localPosition = new Vector3(-1.36f,
																		 leftDoorObject.transform.localPosition.y,
																		 leftDoorObject.transform.localPosition.z);
			}
		}

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
				print("closing...");
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