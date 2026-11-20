using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }
    private InputActions inputActions;

    private void Awake() {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();
    }

    private void OnDestroy() {
        inputActions.Disable();
    }

    public bool IsUpActionPressed() {
        return inputActions.Player.UpArrow.IsPressed();
    }

    public bool IsLeftActionPressed() {
        return inputActions.Player.LeftArrow.IsPressed();
    }

    public bool IsRightActionPressed() {
        return inputActions.Player.RightArrow.IsPressed();
    }
}
