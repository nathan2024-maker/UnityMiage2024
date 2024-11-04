using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D rb;
    public bool isJumping;
    public bool isGrounded;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    private Vector2 velocity = Vector2.zero;
    private float horizontalMovement;
    public Animator animator; // Ajout d'Animator
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Récupérer Rigidbody2D
        animator = GetComponent<Animator>(); // Récupérer Animator
    }

    void Update()
    {
        // Vérification si le joueur est au sol
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);

        // Calcul du mouvement horizontal
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        // Détection du saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        MovePlayer(horizontalMovement);

        Flip(rb.velocity.x);

        // Calcul de la vitesse du personnage
        float characterVelocity = Mathf.Abs(rb.velocity.x);
        
        // Mettre à jour le paramètre de vitesse dans l'Animator
        animator.SetFloat("speed", characterVelocity);

        // Saut du joueur
        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector2 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}
