using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraStacker : MonoBehaviour
{
    private void Start()
    {

        var mainCamera = Camera.main;
        var cameraData = mainCamera.GetUniversalAdditionalCameraData();

        Debug.Log(cameraData.name);

        Camera uiCamera = UIManager.Instance.uiCamera;

        if (cameraData.renderType == CameraRenderType.Base && !cameraData.cameraStack.Contains(uiCamera))
        {
            cameraData.cameraStack.Add(uiCamera);
            Debug.Log("UICamera is added.");
        }
    }
}
