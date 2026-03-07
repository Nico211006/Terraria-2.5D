using UnityEngine;

public class PlayerBlockDetector : MonoBehaviour
{
    public GameObject testBlock;

    public float breakTime = 2f; // Zeit zum Abbauen
    private float breakProgress = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (testBlock != null)
            {
                breakProgress += Time.deltaTime;

                Debug.Log("Abbau Fortschritt: " + breakProgress);

                if (breakProgress >= breakTime)
                {
                    Debug.Log("Block zerstört: " + testBlock.name);
                    Destroy(testBlock);
                }
            }
        }
        else
        {
            breakProgress = 0f;
        }
    }
}