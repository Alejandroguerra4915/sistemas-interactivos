using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class LaserManagar : MonoBehaviour
	{
		///***********************************************************************
		/// Laser (maze) class.
		///***********************************************************************

		private float laserOnTime;
		private float laserOffTime;
		private bool canLaserStart;

		//sfx
		public AudioClip laserActive;

		void Start()
		{
			laserOnTime = 1.25f;
			laserOffTime = 2.0f;
			canLaserStart = true;
		}

		void Update()
		{
			if (canLaserStart)
			{
				StartCoroutine(laserManager());
				playSfx(laserActive);
				canLaserStart = false;
			}
		}

		IEnumerator laserManager()
		{
			GetComponent<Renderer>().enabled = true;
			GetComponent<Collider>().enabled = true;
			yield return new WaitForSeconds(laserOnTime);

			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
			yield return new WaitForSeconds(laserOffTime);
			canLaserStart = true;
		}

		void playSfx(AudioClip _sfx)
		{
			GetComponent<AudioSource>().clip = _sfx;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}

	}
}