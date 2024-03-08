using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class GameController : MonoBehaviour
	{
		///***********************************************************************
		/// Main GameController Class.
		/// This script is responsible for all maze (door and laser) creation.
		/// This class also manages the difficulty steep of the game.
		///***********************************************************************

		//Difficulty variables
		public static float moveSpeed;          //Global speed of moving items (mazes)
		public static float cloneInterval;      //clone mazes every N seconds

		//leveling vars
		public static int current_level = 1;    //Start from easy settings (1=easy ~ 10=hard)	
		private float levelJump = 15.0f;        // every N seconds
		private float levelPassedTime = 0.0f;
		private float levelStartTime = 0.0f;
		private Vector3 startPoint;
		private int rnd;                        //Private random value
		private float startTime = 1.0f;         //Leveling time reference

		//GameOver state
		public static bool gameOver;            //GameOver plane
		private bool gameOverFlag;              //Run the gameover sequence just once

		//AudioClips
		public AudioClip levelAdvanceSfx;

		//Reference to GameObjects
		//maze flag
		public bool createDoor;                 //Can create door mazes
												//maze types
		public GameObject[] doorArrayLeft;      //Array of leftSide door mazes
		public GameObject[] doorArrayRight;     //Array of rightSide door mazes
		public GameObject[] TwinDoorArray;      //Array of bi-directional door mazes
												//level 1 laser
		public bool createLaser;                //Can create laser mazes
		public GameObject[] laserArray;         //available laser mazes	
												//level 1 decal vars
		public bool createDecal;                //Can create decals on the ground? (just gfx usage)
		public GameObject[] decalArray;         //Available decal items

		public bool createVisualLayer;          //Can create top visual layers? (just gfx usage)
		public GameObject[] visualArrays;       //this array holds visual top layer elements
												//Game finish variables
		public GameObject foregroundPlane;      //reference to foreground plane (activates when we run out of live)
		public GameObject gameOverPlane;        //reference to foreground plane (activates when we run out of live)
		public AudioClip gameOverSfx;
		public GameObject player;               //Reference to main player object


		void Awake()
		{
			foregroundPlane.SetActive(false);
			gameOverPlane.SetActive(false);

			current_level = 1;
			levelPassedTime = 0.0f;
			levelStartTime = 0.0f;
			moveSpeed = 1.2f;
			cloneInterval = 3.5f;
			gameOver = false;
			gameOverFlag = false;
		}


		void Update()
		{

			//if we are allowed to spawn a maze, we do it here
			if (Time.timeSinceLevelLoad > startTime && !gameOver)
			{
				if (createDoor)
					cloneDoor();

				if (createVisualLayer)
					StartCoroutine(cloneVisualTopLayer());

				if (createDecal)
					StartCoroutine(cloneDecal());

				if (createLaser)
					StartCoroutine(cloneLaser());

				startTime += cloneInterval;
			}
			else if (gameOver)
			{
				if (!gameOverFlag)
				{
					gameOverFlag = true;
					StartCoroutine(processGameover());
				}
			}

			//if the game is not yet finished, then do make it harder and harder... Poor player :O
			if (!gameOver)
				modifyLevelDifficulty();

		}

		///***********************************************************************
		/// Clone Maze item based on a simple chance factor
		///***********************************************************************
		void cloneDoor()
		{

			float doorTypeChance = Random.Range(0.0f, 1.0f); //we can also use Random.value()
			if (doorTypeChance > 0.0f && doorTypeChance < 0.4f)
			{

				startPoint = new Vector3(2.7f, 0.4f, Random.Range(8.0f, 14.0f));
				Instantiate(doorArrayRight[Random.Range(0, doorArrayRight.Length)], startPoint, Quaternion.Euler(new Vector3(0, 0, 0)));

			}
			else if (doorTypeChance >= 0.4f && doorTypeChance < 0.75f)
			{

				startPoint = new Vector3(2.7f, 0.4f, Random.Range(8.0f, 14.0f));
				Instantiate(doorArrayLeft[Random.Range(0, doorArrayLeft.Length)], startPoint, Quaternion.Euler(new Vector3(0, 0, 0)));

			}
			else
			{

				startPoint = new Vector3(0, 0.4f, Random.Range(8.0f, 10.0f));
				Instantiate(TwinDoorArray[Random.Range(0, TwinDoorArray.Length)], startPoint, Quaternion.Euler(new Vector3(0, 0, 0)));

			}
		}


		///***********************************************************************
		/// Clone Laser mazes
		///***********************************************************************
		IEnumerator cloneLaser()
		{
			//laser will appear after 30 seconds
			if (Time.timeSinceLevelLoad >= 26.5f)
			{
				createLaser = false;
				yield return new WaitForSeconds(Random.Range(3.5f, 7.0f));
				createLaser = true;
				startPoint = new Vector3(0, 0.4f, Random.Range(6.5f, 8.5f));
				Instantiate(laserArray[Random.Range(0, laserArray.Length)], startPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
			}
		}


		///***********************************************************************
		/// Create some doodads to give the game some boost
		///***********************************************************************
		IEnumerator cloneVisualTopLayer()
		{
			createVisualLayer = false;
			yield return new WaitForSeconds(Random.Range(9.0f, 15.0f));
			createVisualLayer = true;
			Instantiate(visualArrays[Random.Range(0, visualArrays.Length)], new Vector3(0, 2, 9.5f), transform.rotation);
		}


		///***********************************************************************
		/// Decals on the ground
		///***********************************************************************
		IEnumerator cloneDecal()
		{
			createDecal = false;
			yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));
			createDecal = true;
			Instantiate(decalArray[Random.Range(0, decalArray.Length)], new Vector3(Random.Range(-3.2f, 3.2f), 0.03f, 8.5f), transform.rotation);
		}


		///***********************************************************************
		/// We can raise the gameSpeed and lower itemCloneInterval values to 
		/// make the game harder.
		///***********************************************************************
		void modifyLevelDifficulty()
		{
			levelPassedTime = Time.timeSinceLevelLoad;
			if (levelPassedTime > levelStartTime + levelJump)
			{
				//increase level difficulty
				if (current_level < 10)
				{
					current_level += 1;
					//let the player know what happened to him/her
					playSfx(levelAdvanceSfx);
					//increase difficulty
					moveSpeed += 0.5f;
					//clone items faster
					cloneInterval -= 0.30f;
					levelStartTime += levelJump;
				}
			}
		}


		///***********************************************************************
		/// Game Over routine
		///***********************************************************************
		IEnumerator processGameover()
		{
			//Do it only once
			if(PlayerPrefs.GetInt("DisplayAd", 0) == 0)
            {
				PlayerPrefs.SetInt("DisplayAd", 1);
				Application.OpenURL("https://www.fiverr.com/finalbossgame");
			}

			//Save score
			GameoverManager.instance.SaveScore();

			//cache player's position 
			Vector3 playerGameoverPosition = player.transform.position;

			//prevent anytype of collision with the player by moving it above all mazes
			player.transform.position = new Vector3(player.transform.position.x,
													1.5f,
													player.transform.position.z);

			playSfx(gameOverSfx);
			float t = 0.0f;
			while (t <= 1.0f)
			{
				t += Time.deltaTime * 0.5f;
				player.transform.position = new Vector3(player.transform.position.x,
														player.transform.position.y,
														Mathf.SmoothStep(playerGameoverPosition.z, -16, t));
				//fade forground plane
				foregroundPlane.SetActive(true);
				foregroundPlane.GetComponent<Animator>().Play("FadeIn");

				yield return 0;
			}

			if (player.transform.position.z < -7.5f)
			{
				gameOverPlane.SetActive(true);
			}
		}


		///***********************************************************************
		/// Play audioclips
		///***********************************************************************
		void playSfx(AudioClip _sfx)
		{
			GetComponent<AudioSource>().clip = _sfx;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}

	}
}