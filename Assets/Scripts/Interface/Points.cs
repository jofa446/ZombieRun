using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    private float pointsMeters; // Variable para almacenar los puntos en metros.
    private TextMeshProUGUI textMesh; // Referencia al componente TextMeshProUGUI para mostrar los puntos.
    private bool isCountingPoints = true; // Variable que controla si se deben seguir contando puntos.

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); // Obtener el componente TextMeshProUGUI en el mismo GameObject.
    }

    private void Update()
    {
        if (isCountingPoints)
        {
            // Incrementar los puntos en función del tiempo transcurrido.
            pointsMeters += Time.deltaTime * 10f;

            // Mostrar los puntos en el componente TextMeshProUGUI sin decimales.
            textMesh.text = pointsMeters.ToString("F0");
        }
    }

    public void AddPoints(float entryPoints)
    {
        if (isCountingPoints)
        {
            // Incrementar los puntos según el valor de entrada.
            pointsMeters += entryPoints;
        }
    }

    // Método para detener el conteo de puntos cuando el jugador toca un obstáculo.
    public void StopCountingPoints()
    {
        isCountingPoints = false;
    }
}
