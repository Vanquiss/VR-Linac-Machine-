using UnityEngine;
using UnityEngine.InputSystem; // Namespace obligatorio
//using DG.Tweening;

public class LinacInputManager : MonoBehaviour
{
    // Referencia a la clase generada automáticamente
    private LinacInputs _inputs;
    
    [Header("Referencias a la Máquina")]
    [SerializeField] private Transform gantryModel;
    
    // Variables de estado para movimiento continuo
    private float _gantryInputDelta;
    private const float RotSpeed = 15f; // Grados por segundo

    private void Awake()
    {
        // Instanciamos la clase de inputs (muy ligera en RAM)
        _inputs = new LinacInputs();

        // 1. Configuración de eventos para botones (Discretos)
        // La sintaxis es: Mapa.Accion.fase += contexto => funcion();
        //_inputs.MachineOperator.EmergencyStop.performed += ctx => PerformEmergencyStop();

        // 2. Configuración para ejes (Continuos)
        // Para ejes, leemos el valor en Update o nos suscribimos al cambio
        _inputs.MachineOperator.RotateGantry.performed += ctx => _gantryInputDelta = ctx.ReadValue<float>();
        _inputs.MachineOperator.RotateGantry.canceled += ctx => _gantryInputDelta = 0f;
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    private void Update()
    {
        // Solo procesamos lógica si hay input real (ahorro de ciclos CPU)
        if (Mathf.Abs(_gantryInputDelta) > 0.01f)
        {
            ApplyRotation(_gantryInputDelta);
        }
    }

    private void ApplyRotation(float direction)
    {
        // Lógica directa sin interpolación excesiva para respuesta inmediata
        // O usa DOTween aquí si prefieres suavizado visual sobre input raw
        float step = direction * RotSpeed * Time.deltaTime;
        gantryModel.Rotate(0, 0, step);
    }

    // private void PerformEmergencyStop()
    // {
    //     Debug.Log("¡PARADA DE EMERGENCIA!");
    //     // Aquí matarías todos los Tweens activos de DOTween
    //     gantryModel.DOKill(); 
    //     _gantryInputDelta = 0;
    // }
}