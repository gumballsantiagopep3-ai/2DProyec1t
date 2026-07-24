using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Controller : MonoBehaviour
{
   
    [Header("Referencias")]
    public GameObject player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Estadísticas")]
    public float speed;
    [SerializeField] private float vida = 3f; // Recuerda darle un valor base
    private float distance;

    private bool estaMuerto = false; // Para evitar que te siga atacando si ya murió

    private void Start()
    {
        // Conseguimos los componentes necesarios del esqueleto
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Si te da flojera arrastrar al player en el inspector, esto lo busca solo:
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        // Si ya murió, no hace nada de movimiento
        if (estaMuerto) return;

        if (player == null) return;

        distance = UnityEngine.Vector2.Distance(transform.position, player.transform.position);

        // Movimiento hacia el jugador
        transform.position = UnityEngine.Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // --- GIRO DIGITAL (Evita que el esqueleto se ponga de cabeza) ---
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // Mira a la derecha (cambia a true si tu sprite original mira a la izquierda)
        }
        else
        {
            spriteRenderer.flipX = true;  // Mira a la izquierda
        }
    }
    

    public void tomardano(float dano)
    {
        if (estaMuerto) return;

        vida -= dano;
        Debug.Log("Vida del esqueleto: " + vida);

        if (vida <= 0)
        {
            muerte();
        }
    }

    private void muerte()
    {
        estaMuerto = true;

        // Activamos la flecha azul que acomodamos ayer en el Animator
        if (anim != null)
        {
            anim.SetTrigger("muerte");
        }

        Debug.Log("El enemigo ha muerto");

        // Desactivamos el colisionador para que el jugador no choque con el cadáver
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        // Desaparece después de 1.5 segundos para que se vea la animación caer
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si lo que golpeamos es el enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Obtenemos el script del enemigo y le aplicamos daño
            Enemy_Controller enemigo = collision.gameObject.GetComponent<Enemy_Controller>();

            if (enemigo != null)
            {
                enemigo.tomardano(1f); // Le quitamos 1 de vida (o la cantidad que quieras)
            }
        }
    }



}
