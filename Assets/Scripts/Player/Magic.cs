using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Magic : MonoBehaviour
{
    [Header("References")]
    public Player player;
    public SpellUIManager spellUIManager;

    [Header("Spell State")]
    [SerializeField] private List<SpellSO> availableSpells = new List<SpellSO>();
    [SerializeField] private int currentIndex = 0;
    public SpellSO CurrentSpell => availableSpells.Count > 0 ? availableSpells[currentIndex] : null;

    public bool CanCast => Time.time >= nextCastTime;
    public float nextCastTime;

    private void Start()
    {
        spellUIManager.ShowSpells(availableSpells);
        HighLightCurrentSpell();
    }

    public void LearnSpell(SpellSO spell)
    {
        Debug.Log($"Learned spell: {spell.name}");
        if (!availableSpells.Contains(spell))
        {
            availableSpells.Add(spell);
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, availableSpells.Count - 1);
        spellUIManager.ShowSpells(availableSpells);

        if (availableSpells.Count > 0)
        {
            HighLightCurrentSpell();
        }
    }

    public void NextSpell()
    {
        if (availableSpells.Count == 0)
            return;
        currentIndex = (currentIndex + 1) % availableSpells.Count;
        HighLightCurrentSpell();
    }

    public void PreviousSpell()
    {
        if (availableSpells.Count == 0)
            return;
        currentIndex = (currentIndex - 1 + availableSpells.Count) % availableSpells.Count;
        HighLightCurrentSpell();
    }

    private void HighLightCurrentSpell()
    {
        if (CurrentSpell != null)
        {
            spellUIManager.HighLightSpell(CurrentSpell);
        }
    }

    public void AnimationFinished()
    {
        player.Animationfinished();
        CastSpell();
    }

    private void CastSpell()
    {
        if (!CanCast || CurrentSpell == null)
            return;

        CurrentSpell.Cast(player);

        nextCastTime = Time.time + CurrentSpell.cooldown;
    }
}
