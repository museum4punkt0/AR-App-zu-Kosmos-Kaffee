using LostInTheGarden.KaffeeKosmos.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public class CoffeeFruitCluster : MonoBehaviour
	{

		public TreeConfigData config;
		public float angleDelta = 10;
		public float xDelta = 0.003f;
		public float clusterRadius = 0.02f;
		public int numFruits = 30;
		public int numFlowers = 30;
		public float growthpoint = 0;

		private List<Transform> flowers = new List<Transform>();
		private List<Transform> fruits = new List<Transform>();
		private List<MeshRenderer> fruitRenderers = new List<MeshRenderer>();
		private List<float> timeOffsets = new List<float>();
		private List<float> stopAtValue = new List<float>();
		private bool isGenerated = false;

		public static int NumberOfFruits = 0;
		public static int SkippedFruits = 0;


		private float localTime = 0;

		void Start()
		{
		}


		public void CreateFruits(bool createFull)
		{

			if (config == null)
			{
				return;
			}

			timeOffsets.Clear();

			if (transform.childCount > 0)
			{
				int childs = transform.childCount;
				for (int i = childs - 1; i >= 0; i--)
				{
					if (!Application.isPlaying)
					{
						GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
					}
					else
					{
						GameObject.Destroy(transform.GetChild(i).gameObject);

					}
				}
			}

			GameObject root = new GameObject();
			root.name = "Root";
			root.transform.SetParent(transform, false);
			float angle = 0;
			float xPosition = 0;
			for (int i = 0; i < numFruits; i++)
			{
				var randomVal = Random.value;
				if(!createFull)
				{
					if (randomVal > CoffeePlant.Instance.FruitFactor)
					{
						SkippedFruits++;
						continue;
					}
					NumberOfFruits++;
				}
				var pos = new Vector3(xPosition, Mathf.Sin(angle) * clusterRadius, Mathf.Cos(angle) * clusterRadius);

				Transform fruit;
				if (!createFull && CoffeePlant.Instance.IsLowQuality)
				{
					fruit = Instantiate(config.fruitLow);
				}
				else
				{
					fruit = Instantiate(config.fruit);
				}
				fruit.name = "Fruit " + i;
				fruit.SetParent(root.transform, false);
				fruit.transform.localPosition = pos;
				fruit.transform.localRotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 0, 0);
				if (createFull)
				{
					fruit.transform.localScale = Vector3.zero;
				}
				fruits.Add(fruit);

				var fruitRenderer = fruit.GetComponentInChildren<MeshRenderer>();
				fruitRenderers.Add(fruitRenderer);

				if (CoffeePlant.Instance != null && CoffeePlant.Instance.FruitMaterialBlock != null)
				{
					CoffeePlant.Instance.FruitMaterialBlock.SetColor("_Color", config.fruitColors.Evaluate(0));
					fruitRenderer.SetPropertyBlock(CoffeePlant.Instance.FruitMaterialBlock);
				}

				angle += angleDelta * Mathf.Deg2Rad;
				xPosition += xDelta;
				timeOffsets.Add(Random.Range(0, 0.0f));
				stopAtValue.Add(Random.Range(growthpoint + 0.2f, growthpoint + 0.5f));
			}

			angle = 0;
			xPosition = 0;
			var activeFlowerCount = numFlowers;
			if(CoffeePlant.Instance.IsLowQuality)
			{
				activeFlowerCount = numFlowers / 2;
			}
			var flowerFruitRato = (float)activeFlowerCount / (float)numFruits;
			for (int i = 0; i < activeFlowerCount; i++)
			{
				var randomVal = Random.value;
				if (!createFull)
				{
					if (randomVal > CoffeePlant.Instance.FruitFactor)
					{
						SkippedFruits++;
						continue;
					}
					NumberOfFruits++;
				}
				var pos = new Vector3(xPosition, Mathf.Sin(angle) * clusterRadius, Mathf.Cos(angle) * clusterRadius);
				
				Transform flower;
				if (!createFull && CoffeePlant.Instance.IsLowQuality)
				{
					flower = Instantiate(config.flowerLow);
				}
				else
				{
					flower = Instantiate(config.flower);
				}
				flower.SetParent(root.transform, false);
				flower.transform.localPosition = pos;
				flower.transform.localScale = Vector3.zero;
				flower.transform.localRotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 0, 0);
				if (createFull)
				{
					flower.transform.localScale = Vector3.one;
				}
				flowers.Add(flower);

				angle += angleDelta * Mathf.Deg2Rad * 1.4f / flowerFruitRato;
				xPosition += xDelta / flowerFruitRato; ;
			}

			root.transform.localPosition = new Vector3(-xPosition / 2f, 0, 0);
		}

		public void Grow(float dt)
		{
			if (CoffeePlant.Instance.PlantTime < config.fruitGrowthStartTime)
			{
				return;
			}
			else if (!isGenerated)
			{
				CoffeePlant.Instance.FetchFruitRatio();
				CreateFruits(false);
				isGenerated = true;
			}

			localTime += dt;

			for (int i = 0; i < flowers.Count; i++)
			{
				var flower = flowers[i];
				var flowerTime = localTime;
				var flowerScaleValue = config.flowerGrowth.Evaluate(flowerTime / config.flowerGrowthTime);
				var flowerScale = new Vector3(flowerScaleValue, flowerScaleValue, flowerScaleValue);
				flower.localScale = flowerScale;
			}

			for (int i = 0; i < fruits.Count; i++)
			{
				var fruit = fruits[i];
				var fruitTime = localTime;
				var fruitScaleValue = config.fruitGrowth.Evaluate((fruitTime - (config.flowerGrowthTime / 2)) / config.fruitGrowthTime);
				var fruitScale = new Vector3(fruitScaleValue, fruitScaleValue, fruitScaleValue);
				fruit.localScale = fruitScale;


				if (CoffeePlant.Instance != null && CoffeePlant.Instance.FruitMaterialBlock != null)
				{
					var value = (fruitTime - (config.flowerGrowthTime / 2)) / config.fruitGrowthTime;
					value = Mathf.Min(stopAtValue[i], value);
					var fruitColor = config.fruitColors.Evaluate(value);
					if(CoffeePlant.Instance.IsLowQuality)
					{
						fruitColor = fruitColor * 0.7f;
					}
					CoffeePlant.Instance.FruitMaterialBlock.SetColor("_Color", fruitColor);
					fruitRenderers[i].SetPropertyBlock(CoffeePlant.Instance.FruitMaterialBlock);
				}
			}
		}


		IEnumerator Destroy(GameObject go)
		{
			yield return new WaitForEndOfFrame();
			DestroyImmediate(go);
		}
	}
}