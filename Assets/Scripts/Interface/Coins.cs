using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    private int coinsTotales = 0; // Variable para almacenar el número de monedas.
    private TextMeshProUGUI textMesh; // Referencia al componente TextMeshProUGUI para mostrar el número de monedas.

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>(); // Obtener el componente TextMeshProUGUI en el mismo GameObject.
        UpdateCoinText(); // Actualizar el texto al inicio.
    }

    public void AddCoin()
    {
        coinsTotales = coinsTotales + 1;
        UpdateCoinText(); // Llamar a la función para actualizar el texto después de sumar una moneda.
    }

    private void UpdateCoinText()
    {
        if (textMesh != null)
        {
            textMesh.text = coinsTotales.ToString(); // Actualizar el texto con el nuevo número de monedas.
        }
    }
}
