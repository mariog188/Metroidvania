using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpellUIManager : MonoBehaviour
{
    [SerializeField] private List<SpellSlot> slots = new List<SpellSlot>();

    public void ShowSpells(List<SpellSO> spells)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < spells.Count)
            {
                slots[i].SetSpell(spells[i]);
            }
            else
            {
                slots[i].SetSpell(null);
            }
        }
    }

    public void HighLightSpell(SpellSO activeSpell)
    {
        foreach (SpellSlot slot in slots)
        {
            slot.SetHighLight(slot.AssignedSpell == activeSpell);
        }
    }

    public void TriggerCooldown(SpellSO spell, float cooldownTime)
    {
        foreach (SpellSlot slot in slots)
        {
            if (slot.AssignedSpell == spell)
            {
                slot.TriggerCooldown(cooldownTime);
                break;
            }
        }
    }
}