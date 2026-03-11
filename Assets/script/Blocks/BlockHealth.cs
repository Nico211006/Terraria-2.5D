using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    [Header("Leben")]
    public int maxHealth = 2;
    private int currentHealth;

    [Header("Drop")]
    public GameObject blockDropPrefab;
    public Vector3 dropOffset = new Vector3(0f, 0.8f, 0f);

    [Header("Optik")]
    public Renderer blockRenderer;
    private Color originalColor;

    private void Start()
    {
        currentHealth = maxHealth;

        if (blockRenderer == null)
        {
            blockRenderer = GetComponent<Renderer>();
        }

        if (blockRenderer != null)
        {
            originalColor = blockRenderer.material.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateBlockColor();

        if (currentHealth <= 0)
        {
            BreakBlock();
        }
    }

    private void BreakBlock()
    {
        if (blockDropPrefab != null)
        {
            Vector3 spawnPosition = transform.position + dropOffset;
            Instantiate(blockDropPrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void UpdateBlockColor()
    {
        if (blockRenderer != null)
        {
            float healthPercent = Mathf.Clamp01((float)currentHealth / maxHealth);
            blockRenderer.material.color = Color.Lerp(Color.black, originalColor, healthPercent);
        }
    }
}