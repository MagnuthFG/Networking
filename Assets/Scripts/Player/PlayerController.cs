using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using SF = UnityEngine.SerializeField;
using UnityEngine.InputSystem;
using static PlayerInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour, IPlayerActions
{    
    [Header("Settings")]
    [SF] private float movementSpeed = 5f;
    [SF] private float shipRotationSpeed = 100f;
    [SF] private float turretRotationSpeed = 4f;

    [Header("Visuals")]
    [SF] private SpriteRenderer _foam = null;

    private PlayerInput _playerInput;
    private Vector2 _moveInput = new();
    private Vector2 _cursorLocation;

    private Transform _shipTransform;
    private Rigidbody2D _rb;

    private Transform turretPivotTransform;
    public UnityAction<bool> onFireEvent;

    private NetworkVariable<bool> _isMoving = new(default, 
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );


    public override void OnNetworkSpawn(){
        _isMoving.OnValueChanged += OnMovmentChanged;

        if (!IsOwner) return;

        if (_playerInput == null){
            _playerInput = new();
            _playerInput.Player.SetCallbacks(this);
        }
        _playerInput.Player.Enable();

        _shipTransform = transform;
        _rb = GetComponent<Rigidbody2D>();

        turretPivotTransform = transform.Find("PivotTurret");
        if (turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);
    }

    public void OnFire(InputAction.CallbackContext context){
        if (!enabled) return;

        if (context.performed){
            onFireEvent?.Invoke(true);

        } else if (context.canceled){
            onFireEvent?.Invoke(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context){
        if (!enabled) return;
        _moveInput = context.ReadValue<Vector2>();

        bool moving = 
            _moveInput.x != 0 || 
            _moveInput.y != 0;

        _isMoving.Value = moving;
    }

    public void OnAim(InputAction.CallbackContext context){
        _cursorLocation = context.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        if (!IsOwner) return;
        _rb.velocity = transform.up * _moveInput.y * movementSpeed;
        _rb.MoveRotation(_rb.rotation + _moveInput.x * -shipRotationSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;
        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
        Vector2 targetDirection = new Vector2(screenToWorldPosition.x - turretPivotTransform.position.x, screenToWorldPosition.y - turretPivotTransform.position.y).normalized;
        Vector2 currentDirection = Vector2.Lerp(turretPivotTransform.up, targetDirection, Time.deltaTime * turretRotationSpeed);
        turretPivotTransform.up = currentDirection;
    }


    private void OnMovmentChanged(bool previous, bool current){
        _foam.enabled = current;
    }

}
