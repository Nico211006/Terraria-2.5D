using UnityEngine;

public class BlockDamageVisual : MonoBehaviour
{
    public Renderer blockRenderer;
    public Texture[] crackTextures;

    public void SetDamage(float progress)
    {
        int index = Mathf.FloorToInt(progress * crackTextures.Length);

        index = Mathf.Clamp(index, 0, crackTextures.Length - 1);

        blockRenderer.material.mainTexture = crackTextures[index];
    }

    public void ResetDamage()
    {
        blockRenderer.material.mainTexture = null;
    }
}