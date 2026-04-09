using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public PlayerInput playerInput;

    public float speed;
    public int facingDirection = 1;

    public Vector2 moveInput;

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        float targetSpeed = moveInput.x * speed;
        rigidbody2D.linearVelocity = new Vector2(targetSpeed, rigidbody2D.linearVelocity.y);
    }

    void Flip()
    {
        if (moveInput.x > .1f)
        {
            facingDirection = 1;
        }
        else if (moveInput.x < .1f)
        {
            facingDirection = -1;
        }
            transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }
}
