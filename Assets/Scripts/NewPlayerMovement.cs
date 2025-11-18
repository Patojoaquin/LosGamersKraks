using UnityEngine;
using UnityEngine.InputSystem;

// Este es un nuevo script de movimiento con movimiento en el eje Z, salto y sprint.
// Está protegido contra el error 'UnassignedReferenceException' para la variable 'groundCheck'.

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class NewPlayerMovement : MonoBehaviour
{
    // --- Variables de Movimiento ---
    [Header("Configuración de Movimiento")]
    [Tooltip("La velocidad de movimiento normal del personaje.")]
    public float moveSpeed = 6f;

    [Tooltip("La velocidad del personaje al correr.")]
    public float sprintSpeed = 10f;

    [Tooltip("La fuerza con la que el personaje salta.")]
    public float jumpForce = 12f;

    // --- Variables de Chequeo de Suelo ---
    [Header("Chequeo de Suelo")]
    [Tooltip("IMPORTANTE: Arrastra aquí un objeto hijo que esté a los pies del personaje.")]
    public Transform groundCheck;

    [Tooltip("El radio de la esfera para detectar si estamos tocando el suelo.")]
    public float groundCheckRadius = 0.2f;

    [Tooltip("La capa (Layer) que se considera como suelo.")]
    public LayerMask groundLayer;

    // --- Componentes y Variables Privadas ---
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isSprinting = false;

    // Awake se llama cuando el script es cargado.
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update se llama una vez por frame.
    private void Update()
    {
        CheckIfGrounded();
    }

    // FixedUpdate se llama a un ritmo fijo, ideal para físicas.
    private void FixedUpdate()
    {
        // Determina la velocidad actual dependiendo de si se está corriendo o no.
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Aplicamos la velocidad en el eje Z.
        // moveInput.x contiene el valor de A (-1) o D (+1).
        // Mantenemos la velocidad en Y y dejamos la de X en 0.
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, moveInput.x * currentSpeed);
    }

    #region Input System Methods
    // Estos métodos son llamados por el componente PlayerInput.

    /// <summary>
    /// Lee el valor del input de movimiento (A/D).
    /// </summary>
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Se activa al presionar el botón de salto.
    /// </summary>
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    /// <summary>
    /// Se activa al presionar el botón de sprint.
    /// </summary>
    public void OnSprint(InputValue value)
    {
        // isPressed es true si el botón está presionado, false si se suelta.
        isSprinting = value.isPressed;
    }

    #endregion

    /// <summary>
    /// Comprueba si el personaje está tocando el suelo.
    /// Incluye una salvaguarda para evitar el error 'UnassignedReferenceException'.
    /// </summary>
    private void CheckIfGrounded()
    {
        // --- SALVAGUARDA ---
        // Si 'groundCheck' no ha sido asignado en el Inspector, detenemos la función aquí.
        if (groundCheck == null)
        {
            // Mostramos un error detallado en la consola de Unity para facilitar la depuración.
            Debug.LogError("La variable 'groundCheck' no ha sido asignada en el Inspector del script NewPlayerMovement. Por favor, asigna un objeto Transform para que el chequeo de suelo funcione.");
            // Salimos de la función para que no intente acceder a 'groundCheck.position' y cause el error.
            isGrounded = false; // Asumimos que no está en el suelo si no podemos comprobarlo.
            return;
        }

        // Si la variable está asignada, procedemos con normalidad.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// Dibuja la esfera de 'groundCheck' en el editor para que sea fácil de visualizar.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
