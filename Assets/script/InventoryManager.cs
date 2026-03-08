using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int dirtCount = 0;
    public TextMeshProUGUI dirtText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddDirt(int amount)
    {
        dirtCount += amount;
        UpdateUI();
        Debug.Log("Erde im Inventar: " + dirtCount);
    }

    void UpdateUI()
    {
        if (dirtText != null)
        {
            dirtText.text = "Erde: " + dirtCount;
        }
    }
}