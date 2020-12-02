using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostInTheGarden.KaffeeKosmos.Data
{
	public class TreeConfigData : ScriptableObject 
	{
		[Header("Plant")]
		[Range(0f,1f)]
		public float attachedGrowthScale = 0.5f;
		[Range(0f, 1f)]
		public float secondTrunkDelay = 0.2f;

		[Header("Trunk")]
		public CoffeeTrunk trunk;
		public CoffeeTrunk trunk2;
        public CoffeeTrunk trunk3;
		[Range(0f, 360f)]
		public float trunk1BranchDirection;
		[MinMaxSlider(0, 90f)]
		public Vector2 trunk1BranchAngleOffset = new Vector2(0f, 0f);
		[Range(0f, 360f)]
		public float trunk2BranchDirection;
		[MinMaxSlider(0, 90f)]
		public Vector2 trunk2BranchAngleOffset = new Vector2(15f, 45f);
		[Range(0f, 360f)]
		public float trunk3BranchDirection;
		[MinMaxSlider(0, 90f)]
		public Vector2 trunk3BranchAngleOffset = new Vector2(15f, 45f);
		public AnimationCurve trunkGrowth;
		public AnimationCurve trunkLeafDeltaTimeCurve;
		[Range(0f, 90f)]
		public float trunkLeafYAxisVariation = 10f;
		[Range(0.2f, 1f)]
		public float trunkLeafDestroyTime = 0.7f;
		[MinMaxSlider(0, 0.2f)]
		public Vector2 trunkBranchDelay = new Vector2(0f,0.1f);
		[MinMaxSlider(0, 0.3f)]
		public Vector2 trunkBranchDeltaTime = new Vector2(0.01f, 0.02f);
		[Range(0.0f, 0.5f)]
		public float trunkFirstBranchTime = 0.3f;

		[Header("Branch")]
		public CoffeeBranch branch;
		public AnimationCurve branchGrowth;
		public AnimationCurve twigGrowth;
		[MinMaxSlider(0, 90f)]
		public Vector2 branchXRotation = new Vector2(30f, 40f);
		[Range(0.1f, 1f)]
		public float branchLeafDeltaTime = 0.25f;
		[MinMaxSlider(0, 1f)]
		public Vector2 branchLeafDeathTime = new Vector2(0.4f, 1f);
		[Range(0.1f, 0.7f)]
		public float branchTwigDeltaTime = 0.5f;
		[MinMaxSlider(0.1f, 0.7f)]
		public Vector2 branchFirstFruitClusterDeltaTime;
		[MinMaxSlider(0.1f, 0.7f)]
		public Vector2 branchFruitClusterDeltaTime;

		[Header("Twig")]
		[MinMaxSlider(0, 90f)]
		public Vector2 twigXRotation = new Vector2(30f, 30f);
		[MinMaxSlider(0, 90f)]
		public Vector2 twigYRotation = new Vector2(10, 20f);
		[MinMaxSlider(0, 0.05f)]
		public Vector2 twigDelay = new Vector2(0.01f, 0.02f);

		[Header("Leaf")]
		public CoffeeLeaf leaf;
		public CoffeeLeaf leafLow;
		public AnimationCurve treeLeafGrowth;
		public AnimationCurve leafGrowth;
		public AnimationCurve branchLeafGrowth;
		[Range(0f, 30f)]
		public float leafMaxXRotation = 20f;
		[Range(0f, 60f)]
		public float leafMaxZRotation = 45f;
		[MinMaxSlider(0f, 2f)]
		public Vector2 leafScale = new Vector2(0.45f, 1f);
		[Range(0, 0.5f)]
		public float leafColorTime;
		public Gradient leafColorAge;
		public AnimationCurve leafColorAgeAffMultFactor;

		[Header("Fruits")]
		public Transform flower;
		public Transform fruit;
		public Transform flowerLow;
		public Transform fruitLow;
		public CoffeeFruitCluster fruitCluster;
		public AnimationCurve flowerGrowth;
		public AnimationCurve fruitGrowth;
		public Gradient fruitColors;
		public float flowerGrowthTime;
		public float fruitGrowthTime;
		public float fruitGrowthStartTime;
	}
}