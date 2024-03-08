using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class LaserHolderMover : MonoBehaviour
	{
		///***********************************************************************
		/// Move the mazes (Lasers) outside of game view and destroy them.
		///***********************************************************************

		private float laserSpeed;
		private int destroyPosition;

		void Awake()
		{
			laserSpeed = 0.7f + Random.Range(-0.3f, 0.3f);
			destroyPosition = -7;
		}

		void Update()
		{
			movementManager();
		}

		void movementManager()
		{
			transform.position -= new Vector3(0,
											  0,
											  Time.deltaTime * GameController.moveSpeed * laserSpeed);
			if (transform.position.z < destroyPosition)
				Destroy(gameObject);
		}
	}
}