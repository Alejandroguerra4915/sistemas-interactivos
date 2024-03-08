using UnityEngine;
using System.Collections;

namespace EndlessSpacePilot
{
	public class TextureScroller : MonoBehaviour
	{
		///***********************************************************************
		/// This script will scroll the background texture of the main game.
		/// It also carefully sync the speed with GameController's global speed to 
		/// avoid undesired effects, like when objects are sliding on the background.
		///***********************************************************************

		private float offset;
		private float damper = 0.004f;
		public float coef = 1;

		void LateUpdate()
		{
			offset += GameController.moveSpeed * damper * Time.deltaTime * coef;
			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset));
		}
	}
}