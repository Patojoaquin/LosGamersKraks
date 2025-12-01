using UnityEngine;
using UnityEngine.InputSystem; // Importante: para el nuevo Input System

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public int saltosMaximos = 2;

    [Header("Detección de Suelo")]
    public Transform checkSuelo;
    public float radioCheckSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private int saltosRestantes;
    private bool estaEnSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        saltosRestantes = saltosMaximos;

        // --- DEPURACIÓN: Verificación de checkSuelo ---
        if (checkSuelo == null)
        {
            Debug.LogError("--- ERROR DE CONFIGURACIÓN --- La variable 'checkSuelo' del script 'MovimientoJugador' NO ESTÁ ASIGNADA en el Inspector. Esto está ocurriendo en el objeto llamado: '" + gameObject.name + "'. Por favor, arrastra el objeto 'CheckSuelo' a la casilla correspondiente en el Inspector y vuelve a probar.");
            this.enabled = false; // Desactiva el script para evitar más errores.
            return;
        }
        else
        {
            Debug.Log("--- CONFIGURACIÓN CORRECTA --- La variable 'checkSuelo' en el objeto '" + gameObject.name + "' ha sido asignada correctamente. El objeto asignado es: '" + checkSuelo.name + "'.");
        }
    }

    void Update()
    {
        // Dibuja un círculo invisible en los pies. Si toca la "capaSuelo", estaEnSuelo es true.
        estaEnSuelo = Physics2D.OverlapCircle(checkSuelo.position, radioCheckSuelo, capaSuelo);

        if (estaEnSuelo && rb.linearVelocity.y <= 0)
        {
            saltosRestantes = saltosMaximos;
        }

        // --- NUEVA ENTRADA DE MOVIMIENTO (Input System) ---
        var keyboard = Keyboard.current;
        if (keyboard == null) return; // No hacer nada si no hay teclado

        float movimientoHorizontal = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            movimientoHorizontal = -1f;
        }
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            movimientoHorizontal = 1f;
        }
        movimiento.x = movimientoHorizontal;


        // --- NUEVA ENTRADA DE SALTO (Input System) ---
        if (keyboard.spaceKey.wasPressedThisFrame && saltosRestantes > 0)
        {
            saltosRestantes--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Resetea la vel. vertical para un salto consistente
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Aplica el movimiento horizontal
        rb.linearVelocity = new Vector2(movimiento.x * velocidad, rb.linearVelocity.y);
    }
}
