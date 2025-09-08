using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region inspector fields
    [Header("General")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _swordArcAnimator;
    [SerializeField] private SpriteRenderer _sprite;
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 60f;
    [SerializeField] private float _jumpImpulseForce = 10f;
    [Header("Is grounded feature")]
    [SerializeField] private BoxCollider2D _groundCheckerCollider;
    [SerializeField] private LayerMask _groundedMask = Physics2D.AllLayers;
    #endregion

    #region vars
    private bool _jumpedByRigidBody, _jumping, _isGrounded, _attacking, _moving;
    private Rigidbody2D _rBody;
    private Vector2 _inputVector, _groundCheckerOffsetPostion, _groundCheckerSize, _boxCastPosition;
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
        RemoveCallbacks();
    }

    private void AssignCallbacks()
    {
        _inputActions.Player.Movement.performed += OnInputMovePerformed;
        _inputActions.Player.Movement.canceled += OnInputMoveCancelled;
        _inputActions.Player.Jump.started += OnInputJumpStarted;
        _inputActions.Player.Attack.started += OnAttackStarted;
    }

    private void RemoveCallbacks()
    {
        _inputActions.Player.Movement.performed += OnInputMovePerformed;
        _inputActions.Player.Movement.canceled += OnInputMoveCancelled;
        _inputActions.Player.Jump.started -= OnInputJumpStarted;
        _inputActions.Player.Attack.started -= OnAttackStarted;
    }
    #endregion

    #region  loop
    private void Update()
    {
        ManageSpriteFlipping();
        SetAnimators();
    }

    private void FixedUpdate()
    {
        // horizontal movement
        var currentVelocity = _rBody.velocity;
        _rBody.velocity = new Vector2(
            _moveSpeed * _inputVector.x * Time.fixedDeltaTime,
            currentVelocity.y
        );
        // jumping
        _isGrounded = IsGrounded();
        if (_jumping && _isGrounded)
        {
            _rBody.AddForce(Vector2.up * _jumpImpulseForce, ForceMode2D.Impulse);
            _jumping = false;
            _jumpedByRigidBody = true;
        }
    }

    private void ManageSpriteFlipping()
    {
        if (_inputVector.x < -0.01f)
        {
            _sprite.flipX = true;
        }
        else if (_inputVector.x > 0.01f)
        {
            _sprite.flipX = false;
        }
    }

    private bool IsGrounded()
    {
        _boxCastPosition = (Vector2)transform.position + _groundCheckerOffsetPostion;
        if (Physics2D.OverlapBox(_boxCastPosition, _groundCheckerSize, transform.eulerAngles.z, _groundedMask) != null)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region animators
    private void SetAnimators()
    {
        if (Math.Abs(_inputVector.x) > 0.005f)
        {
            _animator.SetBool("Running", true);
        }
        else
        {
            _animator.SetBool("Running", false);
        }
        if (_jumpedByRigidBody)
        {
            _attacking = false;
            _isGrounded = false;
        }
        _animator.SetBool("Attacking", _attacking);
        _animator.SetBool("Grounded", _isGrounded);
        _animator.SetBool("Jumping", _jumpedByRigidBody);
        _animator.SetFloat("VerticalSpeed", _rBody.velocity.y);
        _attacking = false;
        _jumpedByRigidBody = false;
    }
    #endregion

    #region Delegates && callbacks

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        _attacking = true;
    }

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
