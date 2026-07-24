using UnityEngine;

public class Player_Moviment : MonoBehaviour

{
    [Header("Estadísticas de Movimiento")]
    [SerializeField] private float speed = 3f;

    [Header("Estadísticas de Combate")]
    [SerializeField] private float vidaMaxima = 3f;
    private float vidaActual;

    [Header("Referencias")]
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    public AudioSource audioSource; // Por si le pones sonido de daño luego

    [HideInInspector] public bool recibirDano;
    private Vector2 moveInput;

    void Start()
    {
        vidaActual = vidaMaxima;

        // Inicializamos los componentes automáticamente para evitar NullReferenceException
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Capturamos los controles en el Update
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // 2. Control estricto de las animaciones del PRIMER SCRIPT
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Horizontal", moveX);
            playerAnimator.SetFloat("Vertical", moveY);
            playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
        }

        // 3. Voltear el personaje usando la escala según hacia dónde camina
        if (moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Mecánicas de Física
    private void FixedUpdate()
    {
        // Solo se mueve si no está bajo el efecto del golpe (empuje)
        if (!recibirDano)
        {
            // Usamos .linearVelocity compatible con Unity 6
            playerRb.linearVelocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
        }
    }

    public void RecibirDano(Vector2 direccion, int canDano)
    {
        if (!recibirDano)
        {
            recibirDano = true;

            // Restamos la vida
            vidaActual -= canDano;
            Debug.Log("Vida del jugador restante: " + vidaActual);

            // Calculamos el rebote/empuje físico limpio
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.5f).normalized;
            playerRb.linearVelocity = Vector2.zero; 
            playerRb.AddForce(rebote * 8f, ForceMode2D.Impulse); 

            // Verificamos si el jugador se quedó sin vida
            if (vidaActual <= 0)
            {
                Muerte();
            }
            else
            {
                // Si sobrevive, recupera el control después de 0.3 segundos
                Invoke("DesactivarDano", 0.3f);
            }
        }
    }

    public void DesactivarDano()
    {
        recibirDano = false;
    }

    void Muerte()
    {
        Debug.Log(gameObject.name + " ha muerto. Game Over.");

        // Activamos la animación de muerte en tu Animator
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("muerte");
        }

        // Congelamos por completo al jugador muerto y apagamos colisiones
        playerRb.linearVelocity = Vector2.zero;
        
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null) playerCollider.enabled = false;

        // Desactivamos este script para que no se pueda mover ni reciba más comandos
        this.enabled = false;
    }
}

