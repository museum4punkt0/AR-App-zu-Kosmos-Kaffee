using LostInTheGarden.KaffeeKosmos.Data;
using LostInTheGarden.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public class CoffeeTrunk : MonoBehaviour
	{
		public SkinnedMeshRenderer mesh;
		public TreeConfigData config;
		public float iteration = 0;
		public float delay = 0;
		public float attachTime;
		public float attachedPoint;
		public float branchDirection;
		public Vector2 branchAngleOffset;

		private CoffeeLeaf[] topLeaves = new CoffeeLeaf[2];

		private float leafAngle = 0;
		
		private float localTime = 0;

		private float growthDeltaTillNextLeaf = 0;
		private float growthDeltaTillNextBranch = 0.2f;

		private List<CoffeeLeaf> leafes = new List<CoffeeLeaf>();
		private List<CoffeeBranch> branches = new List<CoffeeBranch>();


		private Vector3[] splineData;
		private CatmullRom spline;

        private bool didGrowCrown = false;
		
		private float lastGrowth = 0;

		public void SetData(Vector3[] splineData)
		{
			this.splineData = splineData;
			this.spline = new CatmullRom(splineData, 2, false);
			growthDeltaTillNextBranch = config.trunkFirstBranchTime;
		}
		
		private void CreateLeaf()
		{
			for(int i = 0; i < 2; i++)
			{
				if(topLeaves[i] != null)
				{
					topLeaves[i].isTopLeaf = false;
					leafes.Add(topLeaves[i]);
				}
				if (CoffeePlant.Instance.IsLowQuality)
				{
					topLeaves[i] = Instantiate(config.leafLow);
				}
				else
				{
					topLeaves[i] = Instantiate(config.leaf);
				}
				topLeaves[i].transform.SetParent(transform, false);
				topLeaves[i].transform.localPosition = GetSplinePoint(lastGrowth);
				var leafYRotationOffset = Random.Range(-config.trunkLeafYAxisVariation / 2f, config.trunkLeafYAxisVariation / 2f);
				topLeaves[i].transform.localRotation = Quaternion.Euler(0, leafAngle + leafYRotationOffset + (i == 1 ? 180 : 0), 0);
				topLeaves[i].config = config;
				topLeaves[i].sizeScale = Random.Range(config.leafScale.x, config.leafScale.y);
				topLeaves[i].isTopLeaf = true;
			}
			leafAngle += 90;
			
		}


		private void CreateBranch()
		{

			var angle = 0f;
			for (int i = 0; i < 4; i++)
            {
                float attachOffset = Random.Range(-0.01f, 0.1f);
                var attachTime = lastGrowth + attachOffset;
                if(attachTime > 0.99f)
                {
                    continue;
                }

                var branch = Instantiate(config.branch);
				branch.name = "Branch";
				branch.transform.SetParent(transform, false);
				branch.attachTime = attachTime;
				branch.attachedPoint = branch.attachTime;
				branch.transform.localPosition = GetSplinePoint(branch.attachTime);
				var branchXRotation = -Random.Range(config.branchXRotation.x, config.branchXRotation.y);
				var branchYRotation = Random.Range(-45, 45f) + angle;
				branchYRotation = Mathf.MoveTowardsAngle(branchYRotation, branchDirection, Random.Range(branchAngleOffset.x, branchAngleOffset.y));
				branch.transform.localRotation = Quaternion.Euler(branchXRotation, branchYRotation, 0);
				branch.iteration = iteration + 1;
				branch.delay = Random.Range(config.trunkBranchDelay.x, config.trunkBranchDelay.y) + attachOffset;
				branch.branchGrowth = config.branchGrowth;
				branch.SetData();
				branch.config = config;
				branches.Add(branch);
				angle += 90f;
			}
        }

        private void CreateCrown()
        {

            var angle = 0f;
            for (int i = 0; i < 2; i++)
            {

                var branch = Instantiate(config.branch);
                branch.name = "Branch";
                branch.transform.SetParent(transform, false);
                branch.attachTime = 1f;
                branch.attachedPoint = 1f;
                branch.transform.localPosition = GetSplinePoint(branch.attachTime);
                var branchXRotation = -Random.Range(config.branchXRotation.x, config.branchXRotation.y);
                var branchYRotation = Random.Range(-45, 45f) + angle;
                //branchYRotation = Mathf.MoveTowardsAngle(branchYRotation, branchDirection, Random.Range(branchAngleOffset.x, branchAngleOffset.y));
                branch.transform.localRotation = Quaternion.Euler(branchXRotation, branchYRotation, 0);
                branch.iteration = iteration + 1;
                branch.delay = Random.Range(config.trunkBranchDelay.x, config.trunkBranchDelay.y);
                branch.branchGrowth = config.branchGrowth;
                branch.SetData();
                branch.config = config;
                branches.Add(branch);
                angle += 180f;
            }
        }


        public void Grow(float dt, float health)
		{

			if(delay > 0)
			{
				delay -= dt;
				return;
			}

			localTime += dt;
			var growth = config.trunkGrowth.Evaluate(localTime);
			var growthDelta = growth - lastGrowth;
			lastGrowth = growth;

			mesh.SetBlendShapeWeight(0, growth * 100f);

			growthDeltaTillNextLeaf -= growthDelta;
			if(growthDeltaTillNextLeaf < 0)
			{
				CreateLeaf();
				growthDeltaTillNextLeaf = config.trunkLeafDeltaTimeCurve.Evaluate(growth);
			}

			growthDeltaTillNextBranch -= growthDelta;
			if (growthDeltaTillNextBranch < 0 && iteration == 0)
			{
				CreateBranch();
                growthDeltaTillNextBranch = Random.Range(config.trunkBranchDeltaTime.x, config.trunkBranchDeltaTime.y) * MathUtils.MapAndClamp(0.5f, 1f, 1, 0.1f, growth);
                //growthDeltaTillNextBranch = Random.Range(config.trunkBranchDeltaTime.x, config.trunkBranchDeltaTime.y);
            }

            if (growth >= 1f && !didGrowCrown)
            {
                CreateCrown();
                didGrowCrown = true;
            }

			foreach (var leaf in topLeaves)
			{
				leaf.Grow(dt, dt, health);
				leaf.attachTime = growth;
				leaf.attachedPoint = growth;
				leaf.transform.localPosition = GetSplinePoint(growth);
			}

			List<CoffeeLeaf> leafesToDestroy = new List<CoffeeLeaf>();

			foreach (var leaf in leafes)
			{
				leaf.Grow(dt, dt, health);
				leaf.attachedPoint += (1f - leaf.attachedPoint) * growthDelta * config.attachedGrowthScale;
				leaf.transform.localPosition = GetSplinePoint(leaf.attachedPoint);
				if (leaf.localTime > config.trunkLeafDestroyTime)
				{
					leaf.Die();
				}
			}

			foreach (var leaf in leafesToDestroy)
			{
				leafes.Remove(leaf);
				if(Application.isPlaying)
				{
					Destroy(leaf.gameObject);
				}else
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
		}

		private Vector3 GetSplinePoint(float time)
		{
			return spline.GetPoint(Mathf.Clamp(time, 0, 1)).position;
		}

		public float Growth
		{
			get { return lastGrowth; }
		}

		public List<CoffeeBranch> Branches
		{
			get { return branches; }
		}
	}
}