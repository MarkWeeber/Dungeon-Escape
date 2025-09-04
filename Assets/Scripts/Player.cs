using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region inspector fields
    [SerializeField] private float _moveSpeed = 60f;
    [SerializeField] private float _jumpImpulseForce = 10f;
    [SerializeField] private BoxCollider2D _groundCheckerCollider;
    #endregion

    #region vars
    private bool _jumping, _isGrounded;
    private Rigidbody2D _rBody;
    private Vector2 _inputVector, _groundCheckerOffsetPostion, _groundCheckerSize;
    private InputActions _inputActions;

    #endregion

    #region init & deinit
    private void Start()
    {
        _rBody = GetComponent<Rigidbody2D>();
        _inputActions = GetComponent<InputManager>().InputActions;
        AssignCallbacks();
        GetGroundChecker();
    }

    private void GetGroundChecker()
    {
        _groundCheckerOffsetPostion = _groundCheckerCollider.offset;
        _groundCheckerSize = _groundCheckerCollider.size;
    }

    private void OnDestroy()
    {
        _inputActions.Player.Movement.performed -= OnInputMovePerformed;
        _inputActions.Player.Movement.canceled -= OnInputMoveCancelled;
    }

    private void AssignCallbacks()
    {
        _inputActions.Player.Movement.performed += OnInputMovePerformed;
        _inputActions.Player.Movement.canceled += OnInputMoveCancelled;
    }

    private void RemoveCallbacks()
    {
        _inputActions.Player.Movement.performed += OnInputMovePerformed;
        _inputActions.Player.Movement.canceled += OnInputMoveCancelled;
    }
    #endregion

    #region  loop
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        var currentVelocity = _rBody.velocity;
        _rBody.velocity = new Vector2(
            _moveSpeed * _inputVector.x * Time.fixedDeltaTime,
            currentVelocity.y
        );
        if (_jumping && _isGrounded)
        {
            _rBody.AddForce(Vector2.up * _jumpImpulseForce, ForceMode2D.Impulse);
            _jumping = false;
        }
    }

    private bool IsGrounded()
    {
        Vector2 boxCastPosition = (Vector2)transform.position + _groundCheckerOffsetPostion;
        if (Physics2D.BoxCast(boxCastPosition, _groundCheckerSize, 0f, Vector2.zero))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Delegates && callbacks

    private void OnInputJumpStarted(InputAction.CallbackContext context)
    {
        _jumping = true;
    }

    private void OnInputMovePerformed(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    private void OnInputMoveCancelled(InputAction.CallbackContext context)
    {
        _inputVector = Vector2.zero;
    }


    #endregion
}
