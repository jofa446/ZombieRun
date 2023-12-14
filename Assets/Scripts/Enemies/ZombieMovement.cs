using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Transform player; // Referencia al transform del jugador
    private PlayerController playerController; // Referencia al script del controlador del jugador

    private float activationRange = 150.0f; // Define el rango de activación

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Asigna el transform del jugador
        playerController = player.GetComponent<PlayerController>(); // Obtén la referencia al script del controlador del jugador
    }

    void Update()
    {
        // Verifica si el jugador está dentro del rango de activación en el eje Z
        if (Mathf.Abs(player.position.z - transform.position.z) < activationRange)
        {
            // Mueve al enemigo hacia adelante
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else if (transform.position.z < player.position.z + 100)
        {
            // Destruye el enemigo o realiza cualquier acción que desees
            Destroy(gameObject);
        }
    }
}
