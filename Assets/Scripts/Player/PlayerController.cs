using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables de movimiento y físicas del jugador
    public float[] lanePositions = { -5.0f, 0.0f, 5.0f };
    public float laneChangeSpeed = 6.0f;
    public float forwardSpeed = 10.0f;
    public float jumpForce = 9.0f;
    public float slideDuration = 1.0f;

    // Variables de estado del jugador
    private int currentLane = 1;
    private bool isChangingLane;
    private bool isGrounded;
    private bool hasJumped;
    private bool isSliding;
    private float slideTimer;

    // Estado del jugador
    private bool playerAlive = true;

    // Referencia al script Points
    private Points pointsScript;



    // Referencia al Animator
    private Animator animator;

    // Botton try again
    GameObject tryAgainButton;

    // Objetivo de posición y componente Rigidbody
    private Vector3 targetPosition;
    private Rigidbody rb;

    private void Start()
    {
        // Inicialización de posición y referencia al componente Rigidbody
        InitializePosition();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Buscar el componente Animator en el mismo GameObject
        animator = GetComponent<Animator>();

        // Busca el TryAgainButton en otro GameObject
        tryAgainButton = GameObject.Find("TryAgainButton");
        tryAgainButton.SetActive(false);

        // Buscar el componente Points en otro GameObject
        GameObject pointsGameObject = GameObject.Find("Meters");
        pointsScript = pointsGameObject?.GetComponent<Points>();

    }

    private void Update()
    {
        // Control del jugador si está vivo
        if (playerAlive)
        {
            isGrounded = IsPlayerGrounded();
            MoveForward();

            if (!isChangingLane)
            {
                HandleLaneChange();
            }

            if (isGrounded)
            {
                HandleJumpAndSlideInput();
            }

            if (isSliding)
            {
                HandleSlide();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                GetDown();
            }
        }
    }

    private void MoveForward()
    {
        // Movimiento hacia adelante
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private void HandleLaneChange()
    {
        // Cambio de carril
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            LaneChange(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            LaneChange(1);
        }
    }

    private void HandleJumpAndSlideInput()
    {
        // Control de saltos y deslizamientos
        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.S) && !hasJumped)
        {
            Bend();
        }
    }

    private void HandleSlide()
    {
        // Control del deslizamiento
        slideTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetUpAndJump();
        }
        else if (slideTimer >= slideDuration)
        {
            GetUp();
        }
    }

    private bool IsPlayerGrounded()
    {
        // Verificar si el jugador está en el suelo
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, 0.2f);
    }

    private void LaneChange(int direction)
    {
        // Cambio de carril suave
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
        SetTargetPosition();
        StartCoroutine(SmoothLaneChange());
        GetUp();
    }

    private void SetTargetPosition()
    {
        // Establecer la posición objetivo del carril
        targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
    }

    private IEnumerator SmoothLaneChange()
    {
        // Rutina para un cambio suave de carril
        isChangingLane = true;
        float initialX = transform.position.x;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * laneChangeSpeed;
            float newX = Mathf.Lerp(initialX, targetPosition.x, t);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            yield return null;
        }

        isChangingLane = false;
    }

    private void Jump()
    {
        // Control del salto
        if (!hasJumped)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
        }
    }

    private void Bend()
    {
        // Control del deslizamiento
        transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
        isSliding = true;
        slideTimer = 0f;
    }

    private void GetUpAndJump()
    {
        // Levantarse y saltar después
        GetUp();
        Jump();
    }

    private void GetUp()
    {
        // Levantarse después de deslizarse
        isSliding = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void GetDown()
    {
        // Agacharse
        rb.velocity = new Vector3(rb.velocity.x, -20.0f, rb.velocity.z);
        Bend();
    }

    private void Die()
    {
        animator.SetTrigger("Dead");
        // Método para manejar la muerte del jugador
        playerAlive = false;

        // Detener el conteo de puntos
        if (pointsScript != null)
        {
            pointsScript.StopCountingPoints();
        }

        // Invocar el método SetActive después de 3 segundos
        Invoke("ActivateTryAgainButton", 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Manejo de colisiones
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasJumped = false;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            
        }
    }

    private void InitializePosition()
    {
        // Inicialización de la posición del jugador
        targetPosition = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
        transform.position = targetPosition;
    }

    public float CurrentXPosition()
    {
        // Obtener la posición actual en el eje X
        return transform.position.x;
    }

    private void ActivateTryAgainButton()
    {
        // Acceder al componente GameObject y activar el botón
        if (tryAgainButton != null)
        {
            tryAgainButton.SetActive(true);
        }
    }
}
