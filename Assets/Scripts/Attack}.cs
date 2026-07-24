using UnityEngine;

public class Attack : MonoBehaviour
{
   
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danogolpe;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Cambiamos a GetButtonDown para que solo se ejecute 1 vez por cada clic
        if (Input.GetButtonDown("Fire1"))
        {
            Golpe();
        }
    }

    private void Golpe()
    {
        if (animator != null)
        {
            animator.SetTrigger("Golpe");
        }

        // Detecta todos los colisionadores en el radio de golpe
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemy"))
            {
                // Obtenemos el script de vida del enemigo
                Enemy_Controller enemigo = colisionador.GetComponent<Enemy_Controller>();

                // Si el objeto realmente tiene el script, le quitamos vida
                if (enemigo != null)
                {
                    enemigo.tomardano(danogolpe);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
        }
    }


}
