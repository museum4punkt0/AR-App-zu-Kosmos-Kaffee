using LostInTheGarden.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public class TestCamera : MonoBehaviour 
	{
		public GameObject tree;
		public float posTau = 1f;
		private float activePos = 0;
		private Vector3 lookAt = Vector3.zero;
		private Camera myCamera;

		void Start()
		{
			myCamera = GetComponent<Camera>();
		}

		Bounds CalculateBounds(GameObject go)
		{
			Bounds b = new Bounds(go.transform.position, Vector3.zero);
			Object[] rList = go.GetComponentsInChildren(typeof(Renderer));
			foreach (Renderer r in rList)
			{
				b.Encapsulate(r.bounds);
			}
			return b;
		}

		void FocusCameraOnGameObject(Camera c, GameObject go)
		{
			Bounds b = CalculateBounds(go);
			Vector3 max = b.size;
			float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
			radius /= 2f;
			float dist = radius / (Mathf.Sin(c.fieldOfView * Mathf.Deg2Rad / 2f));
			Vector3 viewDir = new Vector3(0, 1f, 3f);
			Vector3 pos = viewDir.normalized * dist + b.center;
			c.transform.position = MathUtils.ExponentialDecayFilter(c.transform.position, pos, posTau, Time.deltaTime);
			lookAt = MathUtils.ExponentialDecayFilter(lookAt, b.center, posTau, Time.deltaTime);
			c.transform.LookAt(lookAt);
		}

		void Update() 
		{
			FocusCameraOnGameObject(myCamera, tree);
		}
	}
}