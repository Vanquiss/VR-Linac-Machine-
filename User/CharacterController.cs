using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DesktopController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float mouseSensitivity = 0.1f; // Sensibilidad baja para precisión técnica
    [SerializeField] private Transform playerCamera;

    private LinacInputs _inputs;
    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _xRotation = 0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new LinacInputs();

        // Suscripción eficiente a eventos (sin polling constante de inputs)
        _inputs.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += ctx => _moveInput = Vector2.zero;

        _inputs.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _inputs.Player.Look.canceled += ctx => _lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        // Bloquear cursor para experiencia FPS
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Transformar input local a dirección de mundo
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        
        // CharacterController maneja colisiones automáticamente (paredes, suelo básico)
        // SimpleMove aplica gravedad automáticamente, ahorrando cálculos manuales
        _controller.SimpleMove(move * moveSpeed);
    }

    private void HandleLook()
    {
        // Rotación Vertical (Cámara)
        float mouseY = _lookInput.y * mouseSensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f); // Evitar romperse el cuello
        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Rotación Horizontal (Cuerpo entero)
        float mouseX = _lookInput.x * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }
}