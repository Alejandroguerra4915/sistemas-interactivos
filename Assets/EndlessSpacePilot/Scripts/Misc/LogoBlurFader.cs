using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class LogoBlurFader : MonoBehaviour
	{
		//**************************************************************************
		// Animate the mainMenu Logo by modifying it's alpha value.
		//**************************************************************************

		private float animationRate = 1.8f;
		private float animationDelay = 1.0f;

		void Start()
		{
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
												GetComponent<Renderer>().material.color.g,
												GetComponent<Renderer>().material.color.b,
												0);
		}

		void Update()
		{
			if (Time.time > animationDelay)
			{
				animationDelay = Time.time + animationRate;
				StartCoroutine(pingPongBlur());
			}
		}

		IEnumerator pingPongBlur()
		{
			float t = 0.0f;
			while (t <= 1.0f)
			{
				t += Time.deltaTime * 2.0f;
				GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
													GetComponent<Renderer>().material.color.g,
													GetComponent<Renderer>().material.color.b,
													t);
				yield return 0;
			}
			if (t >= 0)
			{
				float t2 = 1.0f;
				while (t2 >= 0.0f)
				{
					t2 -= Time.deltaTime * 1.0f;
					GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r,
														GetComponent<Renderer>().material.color.g,
														GetComponent<Renderer>().material.color.b,
														t2);
					yield return 0;
				}
			}
		}

	}
}
