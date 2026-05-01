using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Heal Spell")]
public class HealSO : SpellSO
{
    [Header("Heal Settings")]
    public int healAmount;
    public GameObject healFXPrefab;

    public override void Cast(Player player)
    {
        GameObject newHealFX = Instantiate(healFXPrefab, player.transform.position + Vector3.down * .2f, Quaternion.identity);
        Destroy(newHealFX, 2);

        player.health.ChangeHealth(healAmount);
    }
}
