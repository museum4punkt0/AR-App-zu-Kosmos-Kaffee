/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
/// 
using System;

namespace LostInTheGarden.Utils
{
	public class Singleton<T>
	{
		protected static T _instance;

		protected static object _lock = new object();


		public Singleton() {

		}

		public static T Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = Activator.CreateInstance<T>();
					}
					return _instance;
				}
			}
		}
	}
}