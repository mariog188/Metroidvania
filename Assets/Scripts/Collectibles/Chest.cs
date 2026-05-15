using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private PlayerInput playerInput;
    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInput>(out PlayerInput input))
        {
            playerInput = input;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInput>(out PlayerInput input))
        {
            if (playerInput == input)
                playerInput = null;
        }
    }

    private void Update()
    {
        if (isOpened || playerInput == null)
            return;

        if (playerInput.actions["Interact"].WasPressedThisFrame())
        {
            isOpened = true;
            anim.Play("ChestOpen");
        }
    }
}
