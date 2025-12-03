using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Enemy Settings")]
    public float speed = 2f;
    public float reachDistance = 0.5f;

    [Header("Chasing Settings")]
    public bool isChasing = false; // Ahora es público para que lo veas en el Inspector

    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentTarget;
    private Transform playerTransform;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTarget = pointB; // Empezar moviéndose hacia el punto B
        if (pointA != null) pointA.SetParent(null);
        if (pointB != null) pointB.SetParent(null);
    }

    void Update()
    {
        if (isDead || (pointA == null && pointB == null)) return; // Don't do anything if dead or no patrol points are set

        // Si está persiguiendo, el objetivo es el jugador
        if (isChasing && playerTransform != null)
        {
            currentTarget = playerTransform;
        }

        if (currentTarget == null) return; // Safety check if target becomes null (e.g. player is destroyed)

        // Moverse hacia el objetivo actual (sea un punto de patrulla o el jugador)
        Vector2 direction = (currentTarget.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        // Girar (Flip) en dirección al objetivo
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        // Si NO está persiguiendo, comprobar si debe cambiar de punto de patrulla
        if (!isChasing)
        {
            if (Vector2.Distance(transform.position, currentTarget.position) < reachDistance)
            {
                currentTarget = (currentTarget == pointB) ? pointA : pointB;
            }
        }
    }

    // --- NUEVO: Trigger para detectar al jugador ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cuando el jugador entra en el rango de visión
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            playerTransform = other.transform; // Guarda la posición del jugador
        }
    }

    // --- Opcional: Para que deje de perseguir si el jugador se aleja ---
    private void OnTriggerExit2D(Collider2D other)
    {
        // Cuando el jugador sale del rango de visión
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            playerTransform = null;
            // Vuelve a su patrullaje normal, eligiendo el punto más cercano
            if(pointA != null && pointB != null)
            {
                currentTarget = (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position)) ? pointA : pointB;
            }
        }
    }

    // (El resto del script: OnCollisionEnter2D, EnemyDies, etc. no cambia)
    #region Unchanged Methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            ContactPoint2D contact = collision.contacts[0];

            if (contact.normal.y < -0.5) 
            {
                EnemyDies();
                
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if(playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
                    playerRb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
                }
            }
            else
            {
                if (player != null)
                {
                    player.Die();
                }
            }
        }
    }

    private void EnemyDies()
    {
        isDead = true;
        speed = 0;
        
        if(animator != null) animator.SetTrigger("isDead");
        
        foreach(Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }

        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 1.5f);
    }
    
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    #endregion
}
