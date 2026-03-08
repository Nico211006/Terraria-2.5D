using UnityEngine;

public class BlockHitEffect : MonoBehaviour
{
    public static BlockHitEffect Instance;

    public ParticleSystem hitParticles;
    public ParticleSystem destroyParticles;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayHitEffect(Vector3 position, Color color)
    {
        if (hitParticles == null) return;

        hitParticles.transform.position = position;

        var main = hitParticles.main;
        main.startColor = color;

        hitParticles.Play();
    }

    public void PlayDestroyEffect(Vector3 position, Color color)
    {
        if (destroyParticles == null) return;

        destroyParticles.transform.position = position;

        var main = destroyParticles.main;
        main.startColor = color;

        destroyParticles.Play();
    }
}