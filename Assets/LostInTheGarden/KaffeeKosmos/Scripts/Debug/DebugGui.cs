using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LostInTheGarden.KaffeeKosmos.Debug
{
	public class DebugGui : MonoBehaviour 
	{

		public CoffeePlant tree;
		public Text output;
		public Slider fruitValueSlider;
		public Text fruitValueOutput;
		public Text fruitOutput;

		private int growthFactor = -1;
		private int health = 1;

		private void Start()
		{
			tree.FruitRatio = () =>
			{
				return fruitValueSlider.value;
			};

			tree.PlantFinishedGrowing += () => {
				var totalFruits = CoffeeFruitCluster.NumberOfFruits + CoffeeFruitCluster.SkippedFruits;
				fruitOutput.text = "Fruits:" + CoffeeFruitCluster.NumberOfFruits +
				" total:" + totalFruits +
				" ratio:" + ((float)CoffeeFruitCluster.NumberOfFruits / (float)totalFruits);
			};
		}

		void LateUpdate() 
		{
			var trunk1Time = tree.Trunks[0].Growth;
			var trunk2Time = tree.Trunks[1].Growth;
			string outputString = "Trunk1Time:" + trunk1Time + "\n" + 
				"Trunk2Time:" + trunk2Time + "\n" + 
				"Branch1Time:" + (tree.Trunks[0].Branches.Count > 0 ? tree.Trunks[0].Branches[0].LocalTime: 0);
			output.text = outputString;

			if(growthFactor > 0)
			{
				var dt = Time.deltaTime / 60f;
				for (int i = 0; i < growthFactor; i++)
				{
					tree.Grow(dt, health);
				}
			}
		}

		public void ResetPlant()
		{
			CoffeePlant.Instance.ResetPlant();
		}

		public void StartGrow(int factor)
		{
			growthFactor = factor;
			health = 1;
		}


		public void StartBadGrow(int factor)
		{
			growthFactor = factor;
			health = 0;
		}

		public void EndGrow()
		{
			growthFactor = -1;
		}

		public void SliderDataChanged()
		{
			fruitValueOutput.text = fruitValueSlider.value.ToString("0.00");
		}

		public void SetLowFi()
		{
			CoffeePlant.Instance.IsLowQuality = true;
		}
	}
}