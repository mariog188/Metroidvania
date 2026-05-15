using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private List<CollectibleSO> loots = new List<CollectibleSO>();
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private float launchForce = 4f;

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
            StartCoroutine(OpenChestRoutine());
        }
    }

    private IEnumerator OpenChestRoutine()
    {
        isOpened = true;
        anim.Play("ChestOpen");

        yield return new WaitForSeconds(spawnDelay);

        foreach (CollectibleSO collectibleSO in loots)
        {
            Loot newLoot = Instantiate(lootPrefab, transform.position, Quaternion.identity).GetComponent<Loot>();
            newLoot.Initialize(collectibleSO);

            Rigidbody2D rb = newLoot.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(Random.Range(-0.2f, 0.2f), 1).normalized;
            rb.AddForce(direction * launchForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
