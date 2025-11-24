using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class SceneLoader : MonoBehaviour
{
    // Este método carga una escena por su nombre.
    // Lo puedes llamar desde un botón en el Inspector de Unity.
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("El nombre de la escena no puede estar vacío.");
            return;
        }

        // Asegúrate de que la escena esté añadida en Build Settings (File -> Build Settings).
        SceneManager.LoadScene(sceneName);
    }
}
