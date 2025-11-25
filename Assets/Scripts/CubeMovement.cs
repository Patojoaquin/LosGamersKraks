
using UnityEngine;
using UnityEngine.InputSystem; // Importa el nuevo Input System

public class CubeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private bool isGrounded;

    // Acciones internas para el nuevo Input System
    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody no encontrado en el cubo. Por favor, añade un componente Rigidbody al cubo.");
        }

        // --- Configuración del nuevo Input System ---
        // Crea la acción de mover y la vincula a las teclas W/S
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("1DAxis")
            .With("Positive", "<Keyboard>/w")
            .With("Negative", "<Keyboard>/s");

        // Crea la acción de saltar y la vincula a la barra espaciadora
        jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");

        // Se suscribe al evento "performed" de la acción de saltar
        jumpAction.performed += _ => Jump();
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
        // Lee el valor de movimiento de la acción
        float moveInput = moveAction.ReadValue<float>();

        // Aplica el movimiento
        Vector3 movement = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el cubo está en el suelo
        if (collision.gameObject.CompareTag("Ground")) // Asegúrate de que tu suelo tenga el tag "Ground"
        {
            isGrounded = true;
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
