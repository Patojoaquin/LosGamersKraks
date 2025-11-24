using UnityEngine;

public class CloseGame : MonoBehaviour
{
    // Este método cerrará la aplicación cuando sea llamado.
    // Funciona tanto en builds ejecutables como en el editor (aunque en el editor solo detendrá la ejecución).
    public void QuitGame()
    {
        // Si estamos ejecutando la aplicación en un build (PC, Mac, Linux Standalone, etc.)
        // Application.Quit() cerrará la aplicación.
        Application.Quit();

        // Si estamos en el editor de Unity, Application.Quit() no hará nada.
        // En su lugar, podemos usar UnityEditor.EditorApplication.isPlaying = false;
        // para detener el modo de juego. Esto solo funciona en el editor.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
