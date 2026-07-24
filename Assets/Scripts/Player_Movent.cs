using UnityEngine;

public class Player_Movent : MonoBehaviour
{

    [SerializeField] private float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playeranimator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        playeranimator.SetFloat("Horizontal", moveX);
        playeranimator.SetFloat("Vertical", moveY);
        playeranimator.SetFloat("Speed", moveInput.sqrMagnitude);
    }

                 //Fisicas
    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }


}
