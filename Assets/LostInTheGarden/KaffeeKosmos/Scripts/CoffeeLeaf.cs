using LostInTheGarden.KaffeeKosmos.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos
{
	public class CoffeeLeaf : MonoBehaviour
	{

		public TreeConfigData config;
		public float attachTime;
		public float attachedPoint;
		public float sizeScale = 1;
		public float timeToDie = 1;
		public float branchGrowth = 0;
		public bool isBranch = false;
		public bool isTopLeaf = false;

		public float localTime = 0;
		public float localRealTime = 0;

		public float treeLeafScaleFactor = 1;

		private float leafHealth = 1f;
		private bool isDead = false;
		private MeshRenderer leafRenderer;
		private float healthDecay = 1;

		void Start()
		{
			transform.localScale = new Vector3(0, 0, 0);
			leafRenderer = GetComponentInChildren<MeshRenderer>();
			healthDecay = Random.Range(3f, 7f);
		}


		public void Grow(float dt, float unscaledDt, float health)
		{
			localTime += dt;
			localRealTime += unscaledDt;
			leafHealth = Mathf.MoveTowards(leafHealth, health, unscaledDt * healthDecay);

			if (leafHealth < 0.2f)
			{
				isDead = true;
			}

			if (isDead)
			{
				leafRenderer.transform.position = leafRenderer.transform.position + new Vector3(0, -unscaledDt * 7f, 0);
				leafRenderer.transform.Rotate(4000f * unscaledDt, 2000f * unscaledDt, 0);
				leafRenderer.transform.localScale = Vector3.MoveTowards(leafRenderer.transform.localScale, Vector3.zero, unscaledDt * 50f);
				if (leafRenderer.transform.position.y < 0)
				{
					leafRenderer.enabled = false;
				}
				return;
			}

			if (isBranch)
			{
				var leafColor = config.leafColorAge.Evaluate(localRealTime / config.leafColorTime);
				var addMulFactor = config.leafColorAgeAffMultFactor.Evaluate(localRealTime / config.leafColorTime);
				CoffeePlant.Instance.LeafMaterialBlock.SetColor("_Color", leafColor);
				CoffeePlant.Instance.LeafMaterialBlock.SetFloat("_AddMul", addMulFactor);
			}
			else
			{
				CoffeePlant.Instance.LeafMaterialBlock.SetColor("_Color", Color.white);
				CoffeePlant.Instance.LeafMaterialBlock.SetFloat("_AddMul", 0);
			}
			CoffeePlant.Instance.LeafMaterialBlock.SetFloat("_HealthFactor", leafHealth);
			if (leafRenderer != null)
			{
				leafRenderer.SetPropertyBlock(CoffeePlant.Instance.LeafMaterialBlock);
			}

			float localScale = 1f;
			if (isBranch)
			{
				localScale = config.branchLeafGrowth.Evaluate(localTime);
			}
			else
			{
				localScale = config.leafGrowth.Evaluate(localTime);
			}

			if (isTopLeaf)
			{
				treeLeafScaleFactor = 0;
			}
			else
			{
				treeLeafScaleFactor = Mathf.MoveTowards(treeLeafScaleFactor, 1f, dt * 5f);
			}


			var treeScale = Mathf.Lerp(0.1f, config.treeLeafGrowth.Evaluate(CoffeePlant.Instance.PlantTime), treeLeafScaleFactor);
			var scale = localScale * treeScale * sizeScale;
			transform.localScale = Vector3.one * scale;
		}

		public void Die()
		{
			isDead = true;
		}

	}
}