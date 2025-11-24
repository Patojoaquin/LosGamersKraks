using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con UI como Slider y Image

public class MusicController : MonoBehaviour
{
    [Header("Componentes de UI")]
    public GameObject sliderObject; // El objeto del Slider que se mostrará/ocultará
    public Slider volumeSlider;     // La referencia al componente Slider
    public Image buttonIcon;        // La imagen del botón (o un ícono) que cambiará

    [Header("Sprites del Icono")]
    public Sprite soundOnSprite;    // Sprite para cuando hay sonido
    public Sprite soundOffSprite;   // Sprite para cuando el sonido está en cero

    [Header("Audio")]
    public AudioSource musicSource; // La fuente del audio que quieres controlar

    void Start()
    {
        // Asegurarnos de que el slider esté oculto al iniciar
        if (sliderObject != null)
        {
            sliderObject.SetActive(false);
        }

        // Configurar el valor inicial del slider y el ícono
        if (musicSource != null && volumeSlider != null)
        {
            volumeSlider.value = musicSource.volume;
            UpdateIcon();
        }

        // Añadir un listener al slider para que llame a OnVolumeChange cuando su valor cambie
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChange);
        }
    }

    // Método para mostrar u ocultar el slider
    public void ToggleSlider()
    {
        if (sliderObject != null)
        {
            // Invierte el estado activo del slider (si está activo lo desactiva, y viceversa)
            sliderObject.SetActive(!sliderObject.activeSelf);
        }
    }

    // Método que se llama cada vez que el valor del slider cambia
    public void OnVolumeChange(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
        }
        
        UpdateIcon();
    }

    // Método para actualizar el ícono basado en el volumen
    private void UpdateIcon()
    {
        if (buttonIcon == null || soundOnSprite == null || soundOffSprite == null) return;

        if (volumeSlider.value == 0)
        {
            buttonIcon.sprite = soundOffSprite;
        }
        else
        {
            buttonIcon.sprite = soundOnSprite;
        }
    }
}
