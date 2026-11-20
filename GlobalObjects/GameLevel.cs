using UnityEngine;

public class GameLevel : MonoBehaviour {
    [SerializeField] private int LevelNumber;
    [SerializeField] private Transform LanderStartPositionTransform;
    [SerializeField] private Transform CameraStartTrackingPosition;

    public int GetLevelNumber() {
        return LevelNumber;
    }

    public Vector3 GetLanderStartPositionTransform() {
        return LanderStartPositionTransform.position;
    }

    public Transform GetCameraStartTrackingPosition() {
        return CameraStartTrackingPosition;
    }
}
