using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class PlayerController : MonoBehaviour
	{
		///***********************************************************************
		/// PlayerController class
		/// This class is responsible to move the player by touch or by device acceleraion sensors
		///***********************************************************************

		//Player Control Type (tilt or touch)
		public static int controlType; //0=tilt , 1=touch

		//Distance between player and user's finger
		private int fingerOffset = 100;

		//Private internal variables
		private float xVelocity = 0.0f;
		private float zVelocity = 0.0f;
		private float speed = 23.0f;
		private Vector3 dir = Vector3.zero;
		private Vector3 screenToWorldVector;

		void Awake()
		{
			//fetch user defined controlType
			controlType = PlayerPrefs.GetInt("controlType");
		}

		void Start()
		{
			// Y offset for player
			transform.position = new Vector3(transform.position.x,
											 0.5f,
											 transform.position.z);
		}
		void Update()
		{
			if (!GameController.gameOver)
			{

				if (controlType == 0)
					tiltControl();
				else
					touchControl();

				//this is just for debug and play in PC and SHOULD be commented at final build
				//this can also be used to override control type for WebPlayer and Standalone...
				if (Application.isEditor)
				{
					screenToWorldVector = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + fingerOffset, 10));
					float editorX = Mathf.SmoothDamp(transform.position.x, screenToWorldVector.x, ref xVelocity, 0.1f);
					float editorZ = Mathf.SmoothDamp(transform.position.z, screenToWorldVector.z, ref zVelocity, 0.1f);
					transform.position = new Vector3(editorX, transform.position.y, editorZ);
				}

				//offset for player
				transform.position = new Vector3(transform.position.x,
												 0.5f,
												 transform.position.z);

				//prevent player from exiting the view (downside)
				if (transform.position.z < -5)
					transform.position = new Vector3(transform.position.x,
													 transform.position.y,
													 -5);

				//prevent player from exiting the view (Upside)
				if (transform.position.z > 2.9f)
					transform.position = new Vector3(transform.position.x,
													 transform.position.y,
													 2.9f);

				//left/right movement limiter
				if (transform.position.x > 2.9f)
					transform.position = new Vector3(2.9f,
													 transform.position.y,
													 transform.position.z);

				if (transform.position.x < -2.9f)
					transform.position = new Vector3(-2.9f,
													 transform.position.y,
													 transform.position.z);
			}
		}

		///***********************************************************************
		/// Control playerShip's position by acceleration sensors
		///***********************************************************************
		void tiltControl()
		{
			dir.x = -Input.acceleration.x;
			dir.z = -Input.acceleration.y - 0.45f;
			if (dir.sqrMagnitude > 1)
				dir.Normalize();
			dir *= Time.deltaTime;
			transform.Translate(dir * speed);
		}


		///***********************************************************************
		/// Control playerShip's position with touch position parameters
		///***********************************************************************
		void touchControl()
		{
			if (Input.touchCount > 0 || Input.GetMouseButton(0))
			{
				screenToWorldVector = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + fingerOffset, 10));
				float touchX = Mathf.SmoothDamp(transform.position.x, screenToWorldVector.x, ref xVelocity, 0.1f);
				float touchZ = Mathf.SmoothDamp(transform.position.z, screenToWorldVector.z, ref zVelocity, 0.1f);
				transform.position = new Vector3(touchX, transform.position.y, touchZ);
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