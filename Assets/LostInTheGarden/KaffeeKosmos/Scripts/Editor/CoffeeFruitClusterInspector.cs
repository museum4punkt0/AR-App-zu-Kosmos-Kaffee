using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LostInTheGarden.KaffeeKosmos.Editor
{
	[CustomEditor(typeof(CoffeeFruitCluster))]
	public class CoffeeFruitClusterInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			CoffeeFruitCluster myTarget = (CoffeeFruitCluster)target;
			if(GUILayout.Button("Create"))
			{
				myTarget.CreateFruits(true);
			}
		}
	}
}