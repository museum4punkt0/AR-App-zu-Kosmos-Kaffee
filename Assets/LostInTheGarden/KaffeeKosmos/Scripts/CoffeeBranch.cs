using LostintheGarden.DebugUtils;
using LostInTheGarden.KaffeeKosmos.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public class CoffeeBranch : MonoBehaviour
	{
		public SkinnedMeshRenderer mesh;
		public TreeConfigData config;
		public float iteration = 0;
		public float delay = 0;
		public float attachTime;
		public float attachedPoint;
		public float branchScale = 1f;
		public AnimationCurve branchGrowth;

		private CoffeeLeaf[] topLeaves = new CoffeeLeaf[2];

		private float leafAngle = 0;

		private float localTime = 0;

		private float grwothDeltaTillNextLeaf = 0;
		private float growthDeltaTillNextBranch = 0.5f;
		private float growthDeltaTillNextFruitCluster = 0.3f;

		private List<CoffeeLeaf> leafes = new List<CoffeeLeaf>();
		private List<CoffeeBranch> branches = new List<CoffeeBranch>();
		private List<CoffeeFruitCluster> fruitClusters = new List<CoffeeFruitCluster>();


		private Vector3[] splineData;
		private CatmullRom spline;

		private float lastGrowth = 0;



		public void SetData()
		{
			Vector3[] splineData = CoffeeUtils.ParsePath(path);

			for (int i = 0; i < splineData.Length; i++)
			{
				splineData[i] = splineData[i] * branchScale;
			}
			this.splineData = splineData;
			this.spline = new CatmullRom(splineData, 2, false);

			growthDeltaTillNextBranch = config.branchTwigDeltaTime;
			growthDeltaTillNextFruitCluster = Random.Range(config.branchFirstFruitClusterDeltaTime.x, config.branchFirstFruitClusterDeltaTime.y) ;
		}

		private void CreateLeaf()
		{
			for (int i = 0; i < 2; i++)
			{
				if (topLeaves[i] != null)
				{
					leafes.Add(topLeaves[i]);
				}
				if(CoffeePlant.Instance.IsLowQuality)
				{
					topLeaves[i] = Instantiate(config.leafLow);
				}
				else
				{
					topLeaves[i] = Instantiate(config.leaf);
				}
				topLeaves[i].transform.SetParent(transform, false);
				topLeaves[i].transform.localPosition = GetSplinePoint(lastGrowth);
				topLeaves[i].isBranch = true;
				var leafXRotation = Random.Range(-config.leafMaxXRotation, config.leafMaxXRotation);
				var leafZRotation = Random.Range(-config.leafMaxZRotation, config.leafMaxZRotation);
				topLeaves[i].transform.localRotation = Quaternion.Euler(leafXRotation, (i == 1 ? 90 : 270), leafZRotation);
				topLeaves[i].config = config;
				topLeaves[i].sizeScale = Random.Range(config.leafScale.x, config.leafScale.y);
				topLeaves[i].timeToDie = Random.Range(config.branchLeafDeathTime.x, config.branchLeafDeathTime.y);
			}
		}


		private void CreateBranch()
		{
			var branch = Instantiate(config.branch);
			branch.name = "Branch";
			branch.branchGrowth = config.twigGrowth;
			branch.transform.SetParent(transform, false);
			branch.transform.localPosition = GetSplinePoint(localTime);
			var flip = (Random.Range(0, 2) * 2) - 1;
			var twigXRotation = Random.Range(config.twigXRotation.x, config.twigXRotation.y);
			var twigYRotation = Random.Range(config.twigYRotation.x, config.twigYRotation.y);
			branch.transform.localRotation = Quaternion.Euler(twigXRotation, twigYRotation * flip, 0);
			branch.iteration = iteration + 1;
			branch.delay = Random.Range(config.twigDelay.x, config.twigDelay.y);
			branch.SetData();
			branch.attachTime = lastGrowth;
			branch.attachedPoint = lastGrowth;
			branch.config = config;
			branches.Add(branch);
		}

		private void CreateFruitCluster()
		{
			var fruitCluster = Instantiate(config.fruitCluster);
			fruitCluster.name = "Fruits";
			fruitCluster.growthpoint = lastGrowth;
			fruitCluster.transform.SetParent(transform, false);
			fruitCluster.transform.localPosition = GetSplinePoint(lastGrowth);
			fruitCluster.transform.localRotation = Quaternion.Euler(0, 90, 0);
			fruitCluster.transform.localScale = Vector3.one * 0.5f;
			// fruitCluster.CreateFruits(false);
			fruitClusters.Add(fruitCluster);
		}
		
		public void Grow(float dt, float health)
		{

			if (delay > 0)
			{
				delay -= dt;
				return;
			}
			
			localTime += dt;

			var growth = branchGrowth.Evaluate(localTime);
			var growthDelta = growth - lastGrowth;
			lastGrowth = growth;

			mesh.SetBlendShapeWeight(0, growth * 100f);

			
			
			grwothDeltaTillNextLeaf -= growthDelta;
			if (grwothDeltaTillNextLeaf < 0)
			{
				CreateLeaf();
				grwothDeltaTillNextLeaf = config.branchLeafDeltaTime;
			}
			
			
			growthDeltaTillNextBranch -= growthDelta;
			if (growthDeltaTillNextBranch < 0 && iteration < 2)
			{
				CreateBranch();
				growthDeltaTillNextBranch = 10000f;
			}

			growthDeltaTillNextFruitCluster -= growthDelta;
			

			if (growthDeltaTillNextFruitCluster < 0 && iteration < 2 && CoffeePlant.Instance.PlantTime < config.fruitGrowthStartTime)
			{
				if(growth < 0.6f)
				{
					CreateFruitCluster();
				}
				growthDeltaTillNextFruitCluster = Random.Range(config.branchFruitClusterDeltaTime.x, config.branchFruitClusterDeltaTime.y);
			}

			foreach (var leaf in topLeaves)
			{
				leaf.Grow(growthDelta, dt, health);
				leaf.attachTime = growth;
				leaf.attachedPoint = growth;
				leaf.branchGrowth += growthDelta;
				leaf.transform.localPosition = GetSplinePoint(growth);
			}

			List<CoffeeLeaf> leafesToDestroy = new List<CoffeeLeaf>();
			foreach (var leaf in leafes)
			{
				leaf.Grow(growthDelta, dt, health);
				leaf.attachedPoint += (1f - leaf.attachedPoint) * growthDelta * config.attachedGrowthScale;
				leaf.branchGrowth += growthDelta;
				leaf.transform.localPosition = GetSplinePoint(leaf.attachedPoint);
				if (leaf.branchGrowth > leaf.timeToDie)
				{
					//leafesToDestroy.Add(leaf);
					leaf.Die();
				}
			}

			foreach (var leaf in leafesToDestroy)
			{
				leafes.Remove(leaf);
				if (Application.isPlaying)
				{
					Destroy(leaf.gameObject);
				}
				else
				{
					DestroyImmediate(leaf.gameObject);
				}
			}

			foreach (var branch in branches)
			{
				branch.Grow(dt, health);
				branch.attachedPoint += (1f - branch.attachedPoint) * growthDelta * config.attachedGrowthScale;
				branch.transform.localPosition = GetSplinePoint(branch.attachedPoint);
			}

			foreach(var fruitCluster in fruitClusters)
			{
				fruitCluster.Grow(dt);
			}

			transform.Rotate(40f * growthDelta, 0, 0);
		}

		private Vector3 GetSplinePoint(float time)
		{
			return spline.GetPoint(Mathf.Clamp(time, 0, 1)).position;
		}

		public float LocalTime
		{
			get { return localTime; }
		}

		private static string path = @"[0.0, 0.0, 0.0]
[-0.09455171112672246, 0.08343752529365667, 2.091789211454727]
[-0.37971237737132707, -0.155971545320855, 4.166231676453926]
[-0.5437846012188026, -0.49086335097955036, 6.225362804167491]
[-0.8661512293804716, -0.8691624998734075, 8.263671878851573]
[-1.0207403077283117, -0.8688846108239103, 10.364363362364246]
[-0.7038615208211456, -1.2949120789133508, 12.384872690274163]
[-0.8570759865542139, -2.049442475222461, 14.369591393971145]
[-0.6845838024506359, -3.1292174716879178, 16.070024287741383]
[-0.35140846686378574, -4.192038337190296, 17.732485441378067]
[0.08643727232615173, -5.759620587043035, 19.246596115594578]";
	}
}
