using UnityEngine;

public abstract class CollectibleSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public abstract void Collect(Player player);
}
