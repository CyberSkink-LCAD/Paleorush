using UnityEngine;
using System.Collections.Generic;
using static Collectible;
public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance
    {
        get;
        private set;
    }
    private Dictionary<CollectibleType, int> collectibles = new Dictionary<CollectibleType, int>();
    private void Awake()
    {
        // Singleton protection
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // Initialize all collectible types to 0
        foreach (CollectibleType type in
        System.Enum.GetValues(typeof(CollectibleType)))
        {
            collectibles[type] = 0;
        }
    }
    private void HandleCollectibleCollected(CollectibleType type, int amount)
    {
        if (!collectibles.ContainsKey(type))
            collectibles[type] = 0;
        collectibles[type] += amount;
        Debug.Log($"{type}: {collectibles[type]}");
        // Notify UI AFTER updating totals
        CollectibleEventSystem.RaiseCollectiblesUpdated();
    }

    private void OnEnable()
    {
        // Subscribe to the collectible event
        CollectibleEventSystem.OnCollectibleCollected += HandleCollectibleCollected;
    }
    private void OnDisable()
    {
        // Unsubscribe from the event
        CollectibleEventSystem.OnCollectibleCollected -= HandleCollectibleCollected;
    }
    public int GetAmount(CollectibleType type)
    {
        if (collectibles.ContainsKey(type))
            return collectibles[type];
        return 0;
    }
}
