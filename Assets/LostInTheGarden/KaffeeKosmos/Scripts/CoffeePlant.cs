using LostInTheGarden.KaffeeKosmos.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public class CoffeePlant : MonoBehaviour
	{
		public delegate float FruitRatioCallback();
		public TreeConfigData config;
		public int testSteps = 1000;
		public int speedupFactor = 20;

		private List<CoffeeTrunk> trunks = new List<CoffeeTrunk>();

		private float localTime = 0;

		private MaterialPropertyBlock fruitMaterialBlock;
		private MaterialPropertyBlock leafMaterialBlock;
		private float fruitFactor;
		public FruitRatioCallback FruitRatio;

		public event Action PlantFinishedGrowing;
		private bool plantFinishedGrowingFired = false;

		private bool receivedFruitRatio = false;

		void Start()
		{
			fruitMaterialBlock = new MaterialPropertyBlock();
			leafMaterialBlock = new MaterialPropertyBlock();
			ResetPlant();
		}

		public void ResetPlant()
		{
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
			localTime = 0;
			trunks.Clear();
			receivedFruitRatio = false;
			plantFinishedGrowingFired = false;
			instance = this;
			UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
			CoffeeFruitCluster.NumberOfFruits = 0;
			Vector3[] trunkData = CoffeeUtils.ParsePath(trunkPath);
			Vector3[] trunk2Data = CoffeeUtils.ParsePath(trunk2Path);
			Vector3[] trunk3Data = CoffeeUtils.ParsePath(trunk2Path);

			for (int i = 0; i < trunk3Data.Length; i++)
			{
				trunk3Data[i].y -= 0.1f;
			}

			var trunk = Instantiate(config.trunk);
			trunk.name = "Trunk";
			trunk.transform.SetParent(transform, false);
			trunk.SetData(trunkData);
			trunk.config = config;
			trunk.branchDirection = config.trunk1BranchDirection;
			trunk.branchAngleOffset = config.trunk1BranchAngleOffset;
			trunks.Add(trunk);

			var trunk2 = Instantiate(config.trunk2);
			trunk2.name = "Trunk2";
			trunk2.transform.SetParent(transform, false);
			trunk2.delay = config.secondTrunkDelay;
			trunk2.SetData(trunk2Data);
			trunk2.branchDirection = config.trunk2BranchDirection;
			trunk2.branchAngleOffset = config.trunk2BranchAngleOffset;
			trunk2.config = config;

			trunks.Add(trunk2);


			var trunk3 = Instantiate(config.trunk3);
			trunk3.name = "Trunk3";
			trunk3.transform.SetParent(transform, false);
			trunk3.delay = config.secondTrunkDelay;
			trunk3.SetData(trunk3Data);
			trunk3.config = config;
			trunk3.branchDirection = config.trunk3BranchDirection;
			trunk3.branchAngleOffset = config.trunk3BranchAngleOffset;
			trunk3.transform.localRotation = Quaternion.Euler(0, 60, 0);
			trunks.Add(trunk3);
		}

		public void TestGrow()
		{
			ResetPlant();

			var dt = 1f / (float)testSteps;
			for (int i = 0; i < testSteps; i++)
			{
				localTime += dt;
				foreach (var trunk in trunks)
				{
					trunk.Grow(dt, 0.5f);
				}
			}
#if UNITY_EDITOR
			UnityEditor.Selection.activeObject = null;
#endif
		}

		public void FetchFruitRatio()
		{
			if(!receivedFruitRatio)
			{
				receivedFruitRatio = true;
				if(FruitRatio != null) {
					fruitFactor = FruitRatio();
				}
			}
		}

		void Update()
		{
		}

		public void Grow(float dt, float health)
		{
			if (localTime > 1)
			{
				if(!plantFinishedGrowingFired)
				{
					plantFinishedGrowingFired = true;
					UnityEngine.Debug.Log("Fruits:" + CoffeeFruitCluster.NumberOfFruits + ", Skipped Fruits:" + CoffeeFruitCluster.SkippedFruits);
					if(PlantFinishedGrowing != null)
					{
						PlantFinishedGrowing();
					}
				}
				return;
			}
			localTime += dt;
			if (localTime > 1)
			{
				return;
			}
			foreach (var trunk in trunks)
			{
				trunk.Grow(dt, health);
			}
		}

		public float FruitFactor
		{
			get { return fruitFactor; }
		}

		public float PlantTime
		{
			get { return localTime; }
		}

		public List<CoffeeTrunk> Trunks
		{
			get { return trunks; }
		}

		public MaterialPropertyBlock FruitMaterialBlock
		{
			get { return fruitMaterialBlock; }
		}

		public MaterialPropertyBlock LeafMaterialBlock
		{
			get { return leafMaterialBlock; }
		}

		public bool IsLowQuality
		{
			get; set;
		}

		private static CoffeePlant instance;

		public static CoffeePlant Instance
		{
			get { return instance; }
		}




		private static string trunkPath = @"[0.0, 0.0, 0.0]
[-0.8323433509441214, 10.562026844890257, -0.41324851249565175]
[-2.469382774915368, 21.014694090765456, -1.1795925067066115]
[-3.3040403519204826, 31.563456089845523, -1.808096880440723]
[-3.4938897631820875, 42.11252766860769, -2.8394723974052747]
[-4.019383118889627, 52.67131987568027, -3.4939857852383733]
[-5.773477186891775, 63.076686970619335, -2.466106084907023]
[-7.343238504938152, 73.50026166880589, -2.6347265760313188]
[-9.97321395890356, 83.53965854167211, -4.653746172844438]
[-11.298888859735685, 93.98499595378985, -5.696000198881805]
[-10.148518671977403, 104.50200000000001, -5.56282]";


		private static string trunk2Path = @"0.0	0.1	0.0
0.00233893	0.129274	0.00509532
0.00651582	0.15834	0.0108949
0.0124378	0.187231	0.0172088
0.0195612	0.21602	0.0237649
0.0270814	0.244798	0.0302665
0.0341226	0.273651	0.0364505
0.0399275	0.302648	0.0421446
0.0440473	0.331815	0.0473256
0.0465309	0.361121	0.0521768
0.0481072	0.39046	0.057119
0.049298	0.420026	0.06077
0.05	0.44993	0.0624738
0.0502486	0.480052	0.0629724
0.0500311	0.510254	0.063116
0.0492706	0.540403	0.0637768
0.0478094	0.570375	0.0657625
0.045393	0.600079	0.0697306
0.0416533	0.629468	0.0761023
0.0360928	0.65855	0.0849756
0.0282645	0.687439	0.0957989
0.0217025	0.716821	0.103132
0.0177807	0.746867	0.105461
0.0161677	0.777455	0.103658
0.0164322	0.808433	0.098811
0.0180515	0.839621	0.0922105
0.0204194	0.870813	0.085337
0.0228544	0.90178	0.079849
0.0246077	0.932268	0.0775701
0.0248711	0.962003	0.080476";

	}
}