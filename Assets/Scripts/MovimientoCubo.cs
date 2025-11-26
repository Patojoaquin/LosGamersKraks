using UnityEngine;
using UnityEngine.InputSystem; // ¡Importante! Añadir el namespace del nuevo Input System.

/// <summary>
/// Controla el movimiento y el salto de un objeto usando físicas (Rigidbody)
/// y el nuevo Input System de Unity.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class MovimientoCubo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de desplazamiento del cubo.")]
    public float velocidad = 7f;

    [Header("Configuración de Salto")]
    [Tooltip("La fuerza que se aplica al saltar.")]
    public float fuerzaSalto = 8f;

    // --- Variables del Input System ---
    private InputAction moverAction;
    private InputAction saltarAction;

    // --- Variables privadas ---
    private Rigidbody rb;
    private bool estaEnSuelo;
    private Vector3 direccionMovimiento;

    void Awake()
    {
        // Obtenemos la referencia al componente Rigidbody.
        rb = GetComponent<Rigidbody>();
        
        // --- Definición de las Acciones de Input ---
        
        // Acción para moverse (WASD, flechas y stick de gamepad).
        moverAction = new InputAction("Mover", binding: "<Gamepad>/leftStick");
        moverAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        // Acción para saltar (Espacio y botón sur del gamepad).
        saltarAction = new InputAction("Saltar", binding: "<Keyboard>/space");
        saltarAction.AddBinding("<Gamepad>/buttonSouth");
    }

    void OnEnable()
    {
        // Activamos las acciones cuando el objeto se activa.
        moverAction.Enable();
        saltarAction.Enable();
    }

    void OnDisable()
    {
        // Desactivamos las acciones para evitar que se ejecuten cuando el objeto está inactivo.
        moverAction.Disable();
        saltarAction.Disable();
    }
    
    void Start()
    {
        // Congelamos la rotación en los ejes para que el cubo no se vuelque al moverse.
        rb.freezeRotation = true;
    }

    void Update()
    {
        // --- CAPTURA DE INPUTS ---
        ProcesarInputs();
    }

    void FixedUpdate()
    {
        // --- APLICACIÓN DE FÍSICAS ---
        Mover();
    }

    /// <summary>
    /// Lee los inputs desde las Actions para el movimiento y el salto.
    /// </summary>
    void ProcesarInputs()
    {
        // Leemos el valor del input de movimiento (es un Vector2).
        Vector2 input2D = moverAction.ReadValue<Vector2>();
        
        // Creamos un vector de dirección 3D basado en la orientación del objeto.
        direccionMovimiento = (transform.forward * input2D.y) + (transform.right * input2D.x);

        // Comprobamos si la acción de saltar fue 'triggered' (presionada) en este frame.
        if (saltarAction.triggered && estaEnSuelo)
        {
            Saltar();
        }
    }

    /// <summary>
    /// Aplica el movimiento al Rigidbody del cubo.
    /// </summary>
    void Mover()
    {
        // Calculamos la velocidad deseada.
        Vector3 velocidadMovimiento = direccionMovimiento.normalized * velocidad;
        
        // Mantenemos la velocidad vertical actual (gravedad/salto).
        velocidadMovimiento.y = rb.linearVelocity.y;

        // Asignamos la nueva velocidad al Rigidbody.
        rb.linearVelocity = velocidadMovimiento;
    }

    /// <summary>
    /// Aplica una fuerza vertical al Rigidbody para simular un salto.
    /// </summary>
    void Saltar()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        estaEnSuelo = false;
    }

    // --- MANEJO DE COLISIONES (sin cambios) ---
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaEnSuelo = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !estaEnSuelo)
        {
            estaEnSuelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaEnSuelo = false;
        }
    }
}
