using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LostInTheGarden.KaffeeKosmos.Editor
{
	[CustomEditor(typeof(CoffeePlant))]
	public class CoffeePlantInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			CoffeePlant myTarget = (CoffeePlant)target;
			if(GUILayout.Button("Grow"))
			{
				myTarget.TestGrow();
			}
		}
	}
}