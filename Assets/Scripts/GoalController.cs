using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El jugador ha llegado a la meta! Terminando el juego.");
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}
