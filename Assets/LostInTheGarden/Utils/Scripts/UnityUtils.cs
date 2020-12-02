using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LostInTheGarden.Utils
{
	public static class UnityUtils
    {
        public static T FindComponentInParents<T>(GameObject gameObject) where T : Component
        {
            Transform parent = gameObject.transform.parent;
            if (parent != null)
            {
                T component = parent.gameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
                return FindComponentInParents<T>(parent.gameObject);

            }
            return default(T);
        }
        public static T FindComponentInParents<T>(this Component gameObject) where T : Component
        {
            Transform parent = gameObject.transform.parent;
            if (parent != null)
            {
                T component = parent.gameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
                return FindComponentInParents<T>(parent.gameObject);

            }
            return default(T);
        }

        /// <summary>
        /// Gets or add a component. Usage example:
        /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }
            return result;
        }

        public static T GetOrAddComponent<T>(this GameObject child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.AddComponent<T>();
            }
            return result;
        }

        public static void ExecuteSelfAndChilds<T>(GameObject target, BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
        {
            ExecuteEvents.Execute<T>(target, eventData, functor);

            foreach (Transform child in target.transform)
            {
                ExecuteEvents.Execute<T>(child.gameObject, eventData, functor);
                ExecuteChilds<T>(child.gameObject, eventData, functor);
            }
        }

        public static void ExecuteChilds<T>(GameObject target, BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
        {
            foreach (Transform child in target.transform)
            {
                ExecuteEvents.Execute<T>(child.gameObject, eventData, functor);
                ExecuteChilds<T>(child.gameObject, eventData, functor);
            }
        }

        public static List<GameObject> FindGameObjectsOnLayer(int layer)
        {
            var result = new List<GameObject>();
            var all = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach (var go in all)
            {
                if (go.layer == layer)
                {
                    result.Add(go);
                }
            }
            return result;
        }

        public static void SetAllChildrenStatic(Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.isStatic = true;
                SetAllChildrenStatic(child);
            }
        }
        public static void SetAllChildrenNonStatic(Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.isStatic = false;
                SetAllChildrenStatic(child);
            }
        }

        public static void SetObjectAndAllChildrenToLayer(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            SetAllChildrenToLayer(transform, layer);
        }

        public static void SetAllChildrenToLayer(Transform transform, int layer)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.layer = layer;
                SetAllChildrenToLayer(child, layer);
            }
        }



        public static void AddComponentToAllMeshes<T>(Transform transform) where T : Component
        {
            if (transform.GetComponent<MeshFilter>() != null)
            {
                transform.GetOrAddComponent<T>();
            }
            for (var i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                AddComponentToAllMeshes<T>(child);
            }
        }
        public static Texture3D Cook3DTexture(Texture2D sourceTexture)
        {
            var resultTexture = new Texture3D(sourceTexture.height, sourceTexture.height, sourceTexture.height, TextureFormat.ARGB32, false);
            resultTexture.filterMode = FilterMode.Bilinear;
            resultTexture.wrapMode = TextureWrapMode.Clamp;

            var pixels = sourceTexture.GetPixels();
            resultTexture.SetPixels(pixels);
            resultTexture.Apply();

            return resultTexture;
        }

        public static List<T> GetComponentsInObjectAndChildren<T>(this GameObject gameObject)
        {
            List<T> returnList = new List<T>();
            returnList.AddRange(gameObject.GetComponentsInChildren<T>());
            T myComponent = gameObject.GetComponent<T>();
            if(myComponent != null)
            {
                returnList.Add(myComponent);
            }
            return returnList;
        }

		// see https://forum.unity3d.com/threads/calculatefrustumplanes-without-allocations.371636/
		private static System.Action<Plane[], Matrix4x4> calculateFrustumPlanesImp;
		public static void NonGarbageCalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix, Plane[] planes)
		{
			if (calculateFrustumPlanesImp == null)
			{
				var method = typeof(GeometryUtility).GetMethod("Internal_ExtractPlanes", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new System.Type[] { typeof(Plane[]), typeof(Matrix4x4) }, null);
				if (method == null) throw new System.Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");

				calculateFrustumPlanesImp = System.Delegate.CreateDelegate(typeof(System.Action<Plane[], Matrix4x4>), method) as System.Action<Plane[], Matrix4x4>;
				if (calculateFrustumPlanesImp == null) throw new System.Exception("Failed to reflect internal method. Your Unity version may not contain the presumed named method in GeometryUtility.");
			}

			calculateFrustumPlanesImp(planes, worldToProjectionMatrix);
		}

		public static void NonGarbageCalculateFrustumPlanes(Camera camera, Plane[] planes)
		{
			NonGarbageCalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix, planes);
		}

		public static List<GameObject> GetAllChildren(this GameObject go)
		{
			List<GameObject> gameObjects = new List<GameObject>();
			gameObjects.Add(go);

			foreach (Transform child in go.transform)
			{
				gameObjects.AddRange(child.gameObject.GetAllChildren());
			}
			return gameObjects;
		}

		public static HashSet<Material> GetMaterialsInSubtree(GameObject tree)
		{
			var meshRenderers = tree.GetComponentsInChildren<MeshRenderer>();
			var materialsSet = new HashSet<Material>();
			foreach (var renderer in meshRenderers)
			{
				foreach (var material in renderer.sharedMaterials)
				{
					materialsSet.Add(material);
				}
			}
			return materialsSet;
		}

		// Find components by material
		// i.e. find the components attached to a gameobject that has a Renderer which has <needle> set as its material
		public static List<T> FindComponentsByMaterial<T>(this GameObject tree, Material needle, bool includeInactive = false)
		{
			var resultList = new List<T>();
			foreach (var meshRenderer in tree.GetComponentsInChildren<Renderer>(includeInactive))
			{
				if (meshRenderer.sharedMaterial == needle)
				{
					var component = meshRenderer.GetComponent<T>();
					if (component != null)
					{
						resultList.Add(component);
					}
				}
			}

			return resultList;
		}

		public class ResourcefullScrollView : System.IDisposable
		{
			public ResourcefullScrollView(ref Vector2 scrollPosition, bool alwaysShowHorizontal = false, bool alwaysShowVertical = false, GUILayoutOption[] options = null)
			{
				if (options == null)
				{
					options = new GUILayoutOption[] { GUILayout.ExpandHeight(true) };
                }

				scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
			}

			public void Dispose()
			{
				GUILayout.EndScrollView();
			}
		}

		public static void MakeVerticalSelectableList(Transform parent, bool includeInactive)
		{
			Selectable[] buttons = parent.GetComponentsInChildren<Selectable>(includeInactive);
			for(int i = 0; i < buttons.Length; i++)
			{
				Navigation nav = new Navigation();
				nav.mode = Navigation.Mode.Explicit;
				nav.selectOnUp = buttons[MathUtils.Mod((i - 1), buttons.Length)];
				nav.selectOnDown = buttons[MathUtils.Mod((i + 1), buttons.Length)];
				buttons[i].navigation = nav;
			}
		}

		public static void MakeVerticalSelectableList(List<Selectable> buttons)
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				Navigation nav = new Navigation();
				nav.mode = Navigation.Mode.Explicit;
				nav.selectOnUp = buttons[MathUtils.Mod((i - 1), buttons.Count)];
				nav.selectOnDown = buttons[MathUtils.Mod((i + 1), buttons.Count)];
				buttons[i].navigation = nav;
			}
		}
		public static void MakeHorizontalSelectableList(List<Selectable> buttons)
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				Navigation nav = new Navigation();
				nav.mode = Navigation.Mode.Explicit;
				nav.selectOnLeft = buttons[MathUtils.Mod((i - 1), buttons.Count)];
				nav.selectOnRight = buttons[MathUtils.Mod((i + 1), buttons.Count)];
				buttons[i].navigation = nav;
			}
		}
		public static void MakeHorizontalSelectableList(Transform parent, bool includeInactive)
		{
			Selectable[] buttons = parent.GetComponentsInChildren<Selectable>(includeInactive);
			for (int i = 0; i < buttons.Length; i++)
			{
				Navigation nav = new Navigation();
				nav.mode = Navigation.Mode.Explicit;
				nav.selectOnLeft = buttons[MathUtils.Mod((i - 1), buttons.Length)];
				nav.selectOnRight = buttons[MathUtils.Mod((i + 1), buttons.Length)];
				buttons[i].navigation = nav;
			}
		}

		public static AnimationCurve Clone(this AnimationCurve curve)
		{
			var clone = new AnimationCurve();
			
			clone.keys = curve.keys;
			clone.postWrapMode = curve.postWrapMode;
			clone.preWrapMode = curve.preWrapMode;

			return clone;
		}

		public static long TotalMilliseconds(this DateTime time)
		{
			return (long)(time - new DateTime(1970, 1, 1)).TotalMilliseconds;
		}


	}
}
