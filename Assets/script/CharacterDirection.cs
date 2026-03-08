using UnityEngine;

public class CharacterDirection : MonoBehaviour
{
    public Transform visual;
    public Transform blockCheckPoint;

    public bool facingRight = true;

    private Vector3 blockCheckStartLocalPos;

    void Start()
    {
        if (blockCheckPoint != null)
        {
            blockCheckStartLocalPos = blockCheckPoint.localPosition;
        }
    }

    void Update()
    {
        if (visual == null) return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = visual.localScale;
        scale.x *= -1f;
        visual.localScale = scale;

        if (blockCheckPoint != null)
        {
            Vector3 pos = blockCheckStartLocalPos;
            pos.x = Mathf.Abs(blockCheckStartLocalPos.x) * (facingRight ? 1f : -1f);
            blockCheckPoint.localPosition = pos;
        }
    }
}