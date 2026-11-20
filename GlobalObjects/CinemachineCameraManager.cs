using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraManager : MonoBehaviour {
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private const float CINEMACHINE_CAMERA_ZOOM_SPEED = 2.5f;
    private const float CINEMACHINE_CAMERA_ORTOGRAPHIC_SIZE_START  = 20f;
    private const float CINEMACHINE_CAMERA_ORTOGRAPHIC_SIZE_NORMAL = 10f;

    private float cinemachineCameraTargetOrtographicSize;

    private void Update() {
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, cinemachineCameraTargetOrtographicSize, CINEMACHINE_CAMERA_ZOOM_SPEED * Time.deltaTime);
    }

    public void ChangeCinemachineCameraTarget(Transform TargetTransform) {
        cinemachineCamera.Target.TrackingTarget = TargetTransform;
    }

    public void ChangeCinemachineCameraTargetOrtographicSize(Lander.States playerState) {
        switch (playerState) {
            case Lander.States.WaitingToStart:
                cinemachineCameraTargetOrtographicSize = CINEMACHINE_CAMERA_ORTOGRAPHIC_SIZE_START;
                break;
                
            case Lander.States.Normal:
                cinemachineCameraTargetOrtographicSize = CINEMACHINE_CAMERA_ORTOGRAPHIC_SIZE_NORMAL;
                break;
        }
    }
}
