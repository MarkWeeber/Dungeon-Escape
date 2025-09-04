using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;
    public InputActions InputActions { get => _inputActions; }

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
    }

    private void Oestroy()
    {
        _inputActions.Player.Disable();
    }

}
