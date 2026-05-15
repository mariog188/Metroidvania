using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpellSlot : MonoBehaviour
{
    [Header("References")]
    public Image iconImage;
    public GameObject highLight;
    [SerializeField] private TMP_Text spellText;
    [SerializeField] private Image cooldownOverlay;
    public SpellSO AssignedSpell { get; private set; }
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor = Color.white;
    private Vector3 normalScale = Vector3.one;
    private Vector3 highlightScale = Vector3.one * 1.2f;

    [Header("Pop Settings")]
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuration = 0.4f;

    public void SetSpell(SpellSO spellSO)
    {
        AssignedSpell = spellSO;
        if (spellSO != null)
        {
            cooldownOverlay.sprite = spellSO.icon;
            iconImage.sprite = spellSO.icon;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            AssignedSpell = null;
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
        }
        cooldownOverlay.fillAmount = 0f;
        SetHighLight(false);
    }

    public void SetHighLight(bool active)
    {
        highLight.SetActive(active);

        iconImage.color = active ? highlightColor : normalColor;
        iconImage.rectTransform.localScale = active ? highlightScale : normalScale;

        if (active && AssignedSpell != null)
        {
            spellText.text = AssignedSpell.itemName;
        }
        spellText.enabled = active;

    }

    public void TriggerCooldown(float cooldownTime)
    {
        StartCoroutine(CooldownRoutine(cooldownTime));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        cooldownOverlay.fillAmount = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - (elapsed / duration);
            yield return null;
        }

        cooldownOverlay.fillAmount = 0;
        yield return StartCoroutine(PopEffect());
    }

    private IEnumerator PopEffect()
    {
        float elapsed = 0f;
        float halfDuration = popDuration / 2f;
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            iconImage.rectTransform.localScale = Vector3.Lerp(normalScale, Vector3.one * popScale, t);

            yield return null;
        }
        elapsed = 0f;

        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            iconImage.rectTransform.localScale = Vector3.Lerp(Vector3.one * popScale, normalScale, t);

            yield return null;
        }

        iconImage.rectTransform.localScale = normalScale;
    }
}
