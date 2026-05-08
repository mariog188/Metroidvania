using NUnit.Framework.Constraints;
using UnityEngine;
using TMPro;

public class Loot : MonoBehaviour
{
    private Player player;
    [SerializeField] private CollectibleSO collectibleSO;

    public Animator anim;
    public TMP_Text itemMessage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision entered loot area: " + collision);
        player = collision.GetComponent<Player>();
        Debug.Log("Player entered loot area: " + player);
        if (player == null)
        {
            return;
        }

        CollectItem();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void CollectItem()
    { 
        itemMessage.text = $"Collected: {collectibleSO.itemName}";
        anim.Play("CollectLoot");
        collectibleSO.Collect(player);
        Destroy(gameObject,1);
    }
}
