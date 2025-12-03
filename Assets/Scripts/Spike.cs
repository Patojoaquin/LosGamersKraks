using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damageAmount = 25;

    // Se activa cuando otro collider 2D entra en contacto con este.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto con el que chocamos tiene el tag "Player".
        if (collision.gameObject.CompareTag("Player"))
        {
            // Busca el componente PlayerController en el objeto del jugador.
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Llama a la función pública para hacerle daño.
                player.TakeDamage(damageAmount);
            }
        }
    }
}
