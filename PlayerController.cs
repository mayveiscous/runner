using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 300f;
    [SerializeField] float groundSlamForce = 1000f;

    PlayerState playerState;
    Rigidbody2D rb;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
        
        if (playerState == null)
        {
            Debug.LogError("PlayerState component not found on the same GameObject!");
        }
        
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on player object!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            playerState.SetState("isOnGround", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            playerState.SetState("isOnGround", false);
        }
    }
    
    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && playerState.CanJump())
        {
            float horizontalForce = moveInput != 0 ? moveSpeed * Mathf.Sign(moveInput) : 0f;
    
            rb.AddForce(new Vector2(horizontalForce, jumpForce));
        }
        else if (Input.GetKeyDown(KeyCode.S) && !playerState.GetValue("isOnGround"))
        {
            Debug.Log("Starting ground slam sequence");
            rb.AddForce(new Vector2(0, jumpForce / 1.5f));
            StartCoroutine(GroundSlamSequence());
        }

        IEnumerator GroundSlamSequence()
        {
            yield return new WaitForSeconds(0.35f);
            rb.AddForce(new Vector2(0, -groundSlamForce));
        }
    }
}