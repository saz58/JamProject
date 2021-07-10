using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Libs
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T _instance;

		public static bool Inited => _instance != null;

		private static readonly object _lock = new object();

		public static T Instance
		{
			get
			{
				if (IsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
							"' already destroyed on application quit." +
							" Won't create again - returning null.");
					return null;
				}
				if (InQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
									 "' already destroyed on application quit." +
									 " Won't create again - returning null. " + QuittingFrame + ":" + Time.frameCount);
					//return null;
				}

				lock (_lock)
				{
					if (_instance != null) return _instance;
					// TODO:
					// check this condition, temporary commented
					//if (QuittingFrame == Time.frameCount)
					//{
					//	Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +
					//	                 "' already destroyed on application quit." +
					//	                 " Won't create again - returning null.");
					//	return null;
					//}
					_instance = Find();

					if (_instance == null)
						_instance = LoadFromResources();

					if (_instance == null)
						_instance = Create();
					return _instance;
				}
			}
		}

		private static T Find()
		{
			var ins = (T)FindObjectOfType(typeof(T));
			if (ins != null)
			{
				SetAttributeData(ins);
				return ins;
			}
			if (FindObjectsOfType(typeof(T)).Length > 1)
				Debug.LogError("[Singleton] Something went really wrong " +
				  " - there should never be more than 1 singleton!" +
				  " Reopening the scene might fix it.");
			return null;
		}

		private static T LoadFromResources()
		{
			var strName = typeof(T).Name;
			var prefab = Resources.Load(strName);
			if (prefab == null)
			{
				//Log.w(strName);
				return null;
			}
			var singleton = Instantiate(prefab);
			SetAttributeData(singleton);
			singleton.name = strName;
			return ((GameObject)singleton).GetComponent<T>();
		}

		protected void AwakeInstance(T ins)
		{
			_instance = ins;
			QuittingFrame = -1;
		}
		private static SingletonAttribute _attribute;
		protected static SingletonAttribute AttributeData
		{
			get
			{
				if (_attribute != null)
					return _attribute;
				if (Attribute.IsDefined(typeof(T), typeof(SingletonAttribute)))
				{
					var attrs = Attribute.GetCustomAttributes(typeof(T), typeof(SingletonAttribute), false);
					foreach (var attr in attrs)
					{
						_attribute = (attr as SingletonAttribute);
						return _attribute;
					}
				}
				_attribute = new SingletonAttribute();
				return _attribute;
			}
		}

		private static void SetAttributeData(Object go)
		{
			if (AttributeData.DontDestroy)
				DontDestroyOnLoad(go);
		}

		private static T Create()
		{
			var singleton = new GameObject(typeof(T).ToString());
			SetAttributeData(singleton);
			return singleton.AddComponent<T>();
		}

		public static bool IsQuitting { get; private set; } = false;
		public static int QuittingFrame { get; private set; } = -1;
		public static bool InQuitting => IsQuitting || QuittingFrame == Time.frameCount;
		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (AttributeData.DontDestroy)
				IsQuitting = true;
			else
				_instance = null;
			QuittingFrame = Time.frameCount;
			//Debug.LogWarning($"[Singleton] OnDestroy {gameObject.name} {AttributeData.DontDestroy}");
		}
	}
}
