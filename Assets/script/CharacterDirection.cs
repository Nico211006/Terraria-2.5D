using UnityEngine;

public class CharacterDirection : MonoBehaviour
{
    public Transform visual;
    public Transform blockCheckPoint;

    public bool facingRight = true;

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            facingRight = true;
            visual.localScale = new Vector3(1, 1, 1);
            blockCheckPoint.localPosition = new Vector3(Mathf.Abs(blockCheckPoint.localPosition.x), blockCheckPoint.localPosition.y, blockCheckPoint.localPosition.z);
        }
        else if (moveInput < 0)
        {
            facingRight = false;
            visual.localScale = new Vector3(-1, 1, 1);
            blockCheckPoint.localPosition = new Vector3(-Mathf.Abs(blockCheckPoint.localPosition.x), blockCheckPoint.localPosition.y, blockCheckPoint.localPosition.z);
        }
    }
}