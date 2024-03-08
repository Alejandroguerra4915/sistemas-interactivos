using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class HelperController : MonoBehaviour
	{
		///***********************************************************************
		/// Main Helper class.
		/// This script will clone helper items (+1 life) for player to eat and restore life.
		///***********************************************************************

		//Difficulty
		public static float helperSpeed = 0.95f;        //helper screll speed on screen
		private float helperCloneIntervalMin = 10.0f;   //Min
		private float helperCloneIntervalMax = 20.0f;   //Max

		//Pool of helper objects
		public GameObject[] helpers;

		//start point x,y,z
		private Vector3 startPoint;
		private float cloneLeftLimit = -3.0f;
		private float cloneRightLimit = 3.0f;
		private float rnd = 0;
		private float startTime = 0;

		void Start()
		{
			rnd = Random.Range(helperCloneIntervalMin, helperCloneIntervalMax);
		}

		void Update()
		{
			if (Time.timeSinceLevelLoad > rnd + startTime)
			{
				cloneHelper();
				startTime += rnd;
				rnd = Random.Range(helperCloneIntervalMin, helperCloneIntervalMin);
			}
		}

		void cloneHelper()
		{
			startPoint = new Vector3(Random.Range(cloneLeftLimit, cloneRightLimit), 0.52f, 6.5f);
			Instantiate(helpers[Random.Range(0, helpers.Length)], startPoint, transform.rotation);
		}
	}
}