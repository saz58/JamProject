using UnityEngine;
using Cinemachine;
using GT.UI;
using Scene;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Camera _camera;
    [Range(0, 1)] [SerializeField] private float _alias = 0.1f;

    public Camera Camera => _camera;
    public float Aspect => Screen.width / Screen.height;
    public float Hfov => Vfov * Screen.width / Screen.height;
    public float Vfov => Camera.orthographicSize * 2 * (1 + _alias);

    public Vector3 CamBL => Camera.transform.position - Vector3.right * Hfov * 0.5f - Vector3.up * Vfov * 0.5f;
    public Vector3 CamTR => Camera.transform.position + Vector3.right * Hfov * 0.5f + Vector3.up * Vfov * 0.5f;

    private BattleScene _battleScene;

    public void Setup(BattleScene battleScene)
    {
        _battleScene = battleScene;
        //_virtualCamera.Follow = null;
        //_virtualCamera.LookAt = null;
    }

    public bool IsInCameraView(BackItem block)
    {
        if (block.TopRightCorner.x <= CamBL.x)
            return false;
        if (block.BottomLeftCorner.x >= CamTR.x)
            return false;
        if (block.TopRightCorner.y <= CamBL.y)
            return false;
        if (block.BottomLeftCorner.y > CamTR.y)
            return false;
        return true;
    }
    
    public void RegisterOverlayCamera()
    {
        var cameraData = _camera.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(UIScreenController.Instance.UICamera);

        if (UIScreenController.Instance.UICamera.gameObject.activeSelf == false)
            UIScreenController.Instance.UICamera.gameObject.SetActive(true);
    }
}
