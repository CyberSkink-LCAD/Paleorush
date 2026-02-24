using UnityEngine;
using System;
public static class CollectibleEventSystem
{
    // Fired when a collectible is picked up
    public static event
    Action<Collectible.CollectibleType, int>
    OnCollectibleCollected;
    // Fired AFTER totals are updated
    public static event Action
    OnCollectiblesUpdated;
    public static void
RaiseCollectibleCollected(Collectible.CollectibleType type, int amount)
    {
        OnCollectibleCollected?.Invoke(type, amount);
    }
    public static void
    RaiseCollectiblesUpdated()
    {
        OnCollectiblesUpdated?.Invoke();
    }
}
