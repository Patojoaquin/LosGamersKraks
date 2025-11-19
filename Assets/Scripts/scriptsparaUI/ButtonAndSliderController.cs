using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI

public class ButtonAndSliderController : MonoBehaviour
{
    // Asigna el GameObject del Slider aquí en el Inspector de Unity
    public GameObject sliderGameObject;
    // Asigna el componente AudioSource que quieres controlar
    public AudioSource audioSource;
    
    private Slider slider;

    void Start()
    {
        // Asegúrate de que el slider esté oculto al inicio del juego.
        if (sliderGameObject != null)
        {
            sliderGameObject.SetActive(false);
            // Obtenemos el componente Slider del GameObject
            slider = sliderGameObject.GetComponent<Slider>();
        }

        // Si el slider y el audio source están asignados,
        // ajusta el valor inicial del slider para que coincida con el volumen actual del audio.
        if (slider != null && audioSource != null)
        {
            slider.value = audioSource.volume;
        }
    }

    // Este método público se llamará cuando se haga clic en el botón.
    public void OnButtonClick()
    {
        if (sliderGameObject != null)
        {
            sliderGameObject.SetActive(!sliderGameObject.activeSelf);
        }
    }

    // Este método público se llamará cuando el valor del slider cambie.
    public void SetAudioVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
