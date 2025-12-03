using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Necesario para recargar la escena

// --- REQUISITOS DE COMPONENTES 2D ---
[RequireComponent(typeof(Rigidbody2D))] // Cambiado a Rigidbody2D
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Jump Settings")]
    public int maxJumps = 2;
    private int jumpsLeft;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float moveInput;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth; // Inicia la vida al máximo
        jumpsLeft = maxJumps;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (isDead) return; // Si está muerto, no hacer nada

        UpdateAnimations();
        HandleFlipping();
    }

    void FixedUpdate()
    {
        if (isDead) return; // Si está muerto, no hacer nada

        HandleMovement();
        CheckIfGrounded();
    }

    // --- NUEVOS MÉTODOS PARA LA VIDA ---

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("isHurt"); // Activa la animación de "herido"

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true); // Activa la animación de "muerto"

        // Desactiva el movimiento y las colisiones físicas
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // El cuerpo ya no es afectado por físicas
        GetComponent<Collider2D>().enabled = false; // Desactiva el collider

        // Opcional: Recargar la escena después de 2 segundos
        // Invoke(nameof(ReloadScene), 2f);
    }
    
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // --- MANEJO DE INPUT ---
    public void OnMove(InputValue value)
    {
        if (isDead) return;
        moveInput = value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (isDead) return;
        if (value.isPressed && (isGrounded || jumpsLeft > 0))
        {
            Jump();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (isDead) return;
        if (value.isPressed)
        {
            Attack();
        }
    }

    // --- LÓGICA DE FÍSICAS 2D (con chequeo de muerte)---
    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void CheckIfGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded)
        {
            jumpsLeft = maxJumps;
            animator.SetBool("isJumping", false);
        }
    }
    
    // (El resto de métodos como Jump, Attack, Flip, etc. no cambian)
    #region Unchanged Methods
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsLeft--;
        
        animator.SetBool("isJumping", true);
        isGrounded = false;
    }

    private void Attack()
    {
        animator.SetTrigger("isAttacking");
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.01f);
    }

    private void HandleFlipping()
    {
        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    #endregion
}
