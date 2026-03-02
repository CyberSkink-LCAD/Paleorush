using UnityEngine;
using TMPro;
using static Collectible;

public class UICollectiblesDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text mammalText;
    [SerializeField] private TMP_Text dinosaurText;
    private void OnEnable()
    { 
        CollectibleEventSystem.OnCollectiblesUpdated += UpdateUI; 
    }
    private void OnDisable()
    { 
        CollectibleEventSystem.OnCollectiblesUpdated -= UpdateUI; 
    }
    private void Start()
    {
        UpdateUI(); 
    }
 private void UpdateUI()
        {
        if (CollectibleManager.Instance == null)
            return;
             int mammal = CollectibleManager.Instance.GetAmount(CollectibleType.mammal);
            
            int dinosaur = CollectibleManager.Instance.GetAmount(CollectibleType.dinosaur);
            mammalText.text = $"Mammal: {mammal}";
            dinosaurText.text = $"Dinosaur: {dinosaur}";
        }
    }

