using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GT.UI
{
    public sealed class UIScreenController : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private Camera overlayCamera = default;
        [SerializeField] private RectTransform worldContainer = default;
        [SerializeField] private RectTransform overlayContainer = default;
        [SerializeField] private RectTransform overlaySysContainer = default;

        public static UIScreenController Instance;
        private LoadingScreen _loadingScreen;
        private Dictionary<string, BaseScreen> _screens;

        public Camera UICamera => overlayCamera;

        public GameObject ForceSelectGameObject
        {
            set => eventSystem.SetSelectedGameObject(value);
        }

        private void Awake()
        {
            Instance = this;
            _screens = new Dictionary<string, BaseScreen>();
        }

        private BaseScreen GetScreen(string key)
        {
            return _screens.TryGetValue(key, out var screen) ? screen : null;
        }

        public void SetRenderer(byte index)
        {
            var cameraData =
                overlayCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            cameraData.SetRenderer(index);
        }


        public void Create<T>(System.Action<T> created = null,
            ParentUIContainerType parent = ParentUIContainerType.Overlay) where T : BaseScreen
        {
            var key = typeof(T).Name;
            var screen = GetScreen(key) as T;
            if (screen != null)
            {
                created?.Invoke(screen);
                Debug.Log($"UI Screen {key} already exists");
                return;
            }

            RectTransform rt = overlayContainer;
            switch (parent)
            {
                case ParentUIContainerType.OverlaySystem:
                    rt = overlaySysContainer;
                    break;
                case ParentUIContainerType.Camera:
                    rt = worldContainer;
                    break;
            }

            StartCoroutine(InstantiateAsset());

            System.Collections.IEnumerator InstantiateAsset()
            {
                UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> handle =
                    UnityEngine.AddressableAssets.Addressables.InstantiateAsync(key, rt);
                yield return handle;

                if (handle.Result != null)
                {
                    screen = handle.Result.GetComponent<T>();
                    screen.DestroyResource = () =>
                    {
                        UnityEngine.AddressableAssets.Addressables.Release(handle);
                        UnityEngine.AddressableAssets.Addressables.ReleaseInstance(screen.gameObject);
                        _screens.Remove(key);
                        Debug.Log($"Released UI screen {key}");
                    };
                    _screens.Add(key, screen);
                    created?.Invoke(screen);
                    Debug.Log($"Instantiated UI screen {key}");
                }
                else
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                    Debug.LogError($"Failed instantiate screen: {key}");
                }
            }
        }

        public void GetLoadingScreen(System.Action<LoadingScreen> onDisplay)
        {
            Create<LoadingScreen>(screen =>
            {
                _loadingScreen = screen;
                _loadingScreen.Open(() => { onDisplay?.Invoke(_loadingScreen); });
            }, ParentUIContainerType.OverlaySystem);
        }

        public void HideLoadingScreen(System.Action complete = null)
        {
            if (_loadingScreen != null)
                _loadingScreen.Close(complete);
        }

        public BaseScreen GetScreenByName<T>() where T : BaseScreen
        {
            var key = typeof(T).Name;
            foreach (var kv in _screens)
            {
                if (kv.Key == key)
                    return kv.Value;
            }

            return null;
        }

        public void DestroyScreen<T>() where T : BaseScreen
        {
            var key = typeof(T).Name;
            foreach (var kv in _screens)
            {
                if (kv.Key == key)
                    kv.Value.Close(null);
            }
        }
    }
}