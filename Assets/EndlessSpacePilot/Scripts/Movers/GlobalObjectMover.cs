using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class GlobalObjectMover : MonoBehaviour
	{
		///***********************************************************************
		/// This class will move the objects out of scene and then destroys them.
		///***********************************************************************

		public float speed = 1.9f;
		public float destroyThreshold = -9.0f;

		void Update()
		{
			//Scroll down the object
			transform.position -= new Vector3(0,
											  0,
											  Time.deltaTime * GameController.moveSpeed * speed);
			//Destroy it
			if (transform.position.z < destroyThreshold)
				Destroy(gameObject);
		}
	}
}