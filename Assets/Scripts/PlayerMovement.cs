using UnityEngine;
using UnityEngine.InputSystem; // ¡Importante! Añadimos el nuevo Input System

public class PlayerMovement : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody rb;

    [Header("Movimiento")]
    [Tooltip("Velocidad de movimiento horizontal del jugador.")]
    public float moveSpeed = 5f;
    private Vector2 moveInput; // Usaremos un Vector2 para leer el input

    [Header("Salto")]
    [Tooltip("Fuerza con la que el jugador salta.")]
    public float jumpForce = 8f;

    [Header("Detección de Suelo")]
    [Tooltip("El objeto que se usa como referencia para saber si se toca el suelo.")]
    public Transform groundCheck;
    [Tooltip("El radio del círculo para detectar el suelo. Ajústalo al tamaño de tu cubo.")]
    public float groundCheckRadius = 0.5f;
    [Tooltip("La capa(s) que se consideran como 'suelo'.")]
    public LayerMask groundLayer;
    private bool isGrounded;

    // Start se llama antes del primer frame
    void Start()
    {
        // Obtenemos el componente Rigidbody automáticamente al iniciar
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Este objeto no tiene un componente Rigidbody. Por favor, añádelo.");
        }
    }

    // Update ya no se usa para leer Inputs, solo para lógicas que no son de físicas
    void Update()
    {
        // Comprobar si estamos tocando el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // FixedUpdate se llama en un intervalo de tiempo fijo, ideal para físicas
    void FixedUpdate()
    {
        if (rb == null) return;

        // --- APLICAR FÍSICAS ---

        // Mover el Rigidbody horizontalmente usando el valor guardado de OnMove
        rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, 0);
    }

    // --- MÉTODOS INVOCADOS POR EL PLAYERINPUT COMPONENT ---

    // Este método es llamado por el PlayerInput cuando la acción "Move" se activa
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Este método es llamado por el PlayerInput cuando la acción "Jump" se activa
    public void OnJump(InputValue value)
    {
        // Solo saltamos si el botón fue presionado Y estamos en el suelo
        if (value.isPressed && isGrounded)
        {
            Jump();
        }
    }

    // Método para manejar el salto
    void Jump()
    {
        // Aplicar una fuerza vertical para el salto
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0); // Opcional: Resetea la velocidad vertical para un salto más consistente
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // (Opcional) Dibuja una esfera en el editor para visualizar el groundCheck
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
