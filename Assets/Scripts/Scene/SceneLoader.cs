using System;
using System.Collections;
using Libs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
	[Singleton(true)]
	public sealed class SceneLoader : Singleton<SceneLoader>
	{
		public bool InProgress { get; private set; }

		public static string SelectedLevel { get; private set; }

		public static SceneId SceneId { get; private set; } = SceneId.Lobby;

		public event Action<float> ProgressChanged;

		public static void GoTo(SceneId id)
		{
			Debug.LogFormat("[SceneLoader] GoTo {0}", id);
			SceneId = id;
			Instance.StartLoad(id.ToString());
		}

		public static void GoTo(string id)
		{
			Debug.LogFormat("GoTo {0} {1}", id, Instance.InProgress);
			Instance.StartLoad(id);
		}

		private void StartLoad(string id)
		{
			if (id == SelectedLevel && InProgress)
			{
				Debug.LogWarning($"[SceneLoader] Already in progress {id}");
				return;
			}

			if (InProgress)
			{
				Debug.LogError($"[SceneLoader] Change scene during loading {SelectedLevel} to: {id}");
				StopAllCoroutines();
				SelectedLevel = id;
				StartCoroutine(LoadWithoutScreen(id, !IsInit));

				//AlertManager.ShowAlert("Info", $"StartLoad scene fail, have any in progress {id}");
				return;
			}
			SelectedLevel = id;
			StartCoroutine(LoadWithoutScreen(id, !IsInit));
		}

		private float _timeStartLoadScene;
		private AsyncOperation _sceneLoadOperation;
		private IEnumerator LoadWithoutScreen(string sceneName, bool loadAsset)
		{
			_timeStartLoadScene = Time.unscaledTime;
			InProgress = true;
			SceneProgress = 0;
			AssetsProgress = 0.0f;
			ProgressChanged?.Invoke(FullProgress);

			//AlertManager.Instance.ShowLoadingScreen(true);
			//SceneManager.LoadScene("Loading");
			yield return new WaitForEndOfFrame();
			Unload();
			yield return new WaitForEndOfFrame();
			if (loadAsset)
			{
				yield return LoadSceneAssets();
			}
			else
			{
				AssetsProgress = 1.0f;
			}
			SceneProgress = 0.0f;
			yield return new WaitForEndOfFrame();
			_sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName.Trim('\n'));
			_sceneLoadOperation.allowSceneActivation = !loadAsset;
			_sceneLoadOperation.priority = 1;
			while (!_sceneLoadOperation.isDone && _sceneLoadOperation.progress < 0.85f)
			{
				SceneProgress = _sceneLoadOperation.progress;
				ProgressChanged?.Invoke(FullProgress);
				yield return new WaitForEndOfFrame();
			}
			// min delay to load small scenes
			while (Time.unscaledTime < (_timeStartLoadScene + 2.0f))
			{
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			Debug.LogFormat("[SceneLoader] Loaded {0}", sceneName);
			SceneProgress = 1.0f;
			ProgressChanged?.Invoke(FullProgress);
			_sceneLoadOperation.allowSceneActivation = true;
			InProgress = false;
			while (!_sceneLoadOperation.isDone)
			{
				yield return new WaitForEndOfFrame();
			}
			_sceneLoadOperation = null;

			yield return new WaitForEndOfFrame();
			//AlertManager.Instance.ForceCloseLoadingScreen();
		}

		public float SceneProgress { get; private set; }

		public float AssetsProgress { get; private set; }
		private float FullProgress => SceneProgress * 0.4f + AssetsProgress * 0.6f;

		public static bool IsInit => SelectedLevel == "Init";
		public static bool IsBattle => SelectedLevel == SceneId.Battle.ToString();

		private IEnumerator LoadSceneAssets()
		{
			var assetKeys = CacheLoader.AssetsData;
			AddressableHandler.Instance.GetDiffAsync(assetKeys.ItemKeys);

			yield return new WaitForEndOfFrame();
			while (!AddressableHandler.Instance.IsDone)
			{
				AssetsProgress = 0.5f + AddressableHandler.Instance.Progress * 0.8f;
				ProgressChanged?.Invoke(FullProgress);
				yield return new WaitForEndOfFrame();
			}
			CacheLoader.CacheReady = true;
			AssetsProgress = 1.0f;
			ProgressChanged?.Invoke(FullProgress);
		}

		internal static void Restart()
		{
			GoTo(SceneManager.GetActiveScene().name);
		}

		private static void Unload()
		{
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
	}
}
