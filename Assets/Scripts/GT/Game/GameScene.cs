using GT.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GT.Game
{
    public class GameScene : MonoBehaviour
    {
        // Game scene enter point.
        [SerializeField] private Camera baseCamera = default;

        private void Awake()
        {
            RegisterOverlayCamera();
        }

        //todo: Move to camera script.
        private void RegisterOverlayCamera()
        {
            var cameraData = baseCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(UIScreenController.Instance.UICamera);

            if (UIScreenController.Instance.UICamera.gameObject.activeSelf == false)
                UIScreenController.Instance.UICamera.gameObject.SetActive(true);
        }
    }
}