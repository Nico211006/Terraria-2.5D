using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [Header("Body Parts")]
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;

    [Header("Walk Animation")]
    public float swingSpeed = 6f;
    public float swingAmount = 25f;

    [Header("Mining Animation")]
    public float miningSpeed = 10f;
    public float miningAngle = 50f;

    private Quaternion leftArmStartRot;
    private Quaternion rightArmStartRot;
    private Quaternion leftLegStartRot;
    private Quaternion rightLegStartRot;

    void Start()
    {
        if (leftArm != null) leftArmStartRot = leftArm.localRotation;
        if (rightArm != null) rightArmStartRot = rightArm.localRotation;
        if (leftLeg != null) leftLegStartRot = leftLeg.localRotation;
        if (rightLeg != null) rightLegStartRot = rightLeg.localRotation;
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;
        bool isMining = Input.GetKey(KeyCode.E);

        if (isMining)
        {
            AnimateMining();
        }
        else if (isMoving)
        {
            AnimateWalking();
        }
        else
        {
            ResetToIdle();
        }
    }

    void AnimateWalking()
    {
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAmount;

        if (leftArm != null)
            leftArm.localRotation = leftArmStartRot * Quaternion.Euler(swing, 0f, 0f);

        if (rightArm != null)
            rightArm.localRotation = rightArmStartRot * Quaternion.Euler(-swing, 0f, 0f);

        if (leftLeg != null)
            leftLeg.localRotation = leftLegStartRot * Quaternion.Euler(-swing, 0f, 0f);

        if (rightLeg != null)
            rightLeg.localRotation = rightLegStartRot * Quaternion.Euler(swing, 0f, 0f);
    }

    void AnimateMining()
    {
        float miningSwing = Mathf.Sin(Time.time * miningSpeed) * miningAngle;

        // Rechter Arm schlägt nach vorne / hinten statt schräg zur Seite
        if (rightArm != null)
            rightArm.localRotation = rightArmStartRot * Quaternion.Euler(-miningSwing, 0f, 0f);

        // Andere Körperteile ruhig halten
        if (leftArm != null)
            leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, leftArmStartRot, Time.deltaTime * 10f);

        if (leftLeg != null)
            leftLeg.localRotation = Quaternion.Lerp(leftLeg.localRotation, leftLegStartRot, Time.deltaTime * 10f);

        if (rightLeg != null)
            rightLeg.localRotation = Quaternion.Lerp(rightLeg.localRotation, rightLegStartRot, Time.deltaTime * 10f);
    }

    void ResetToIdle()
    {
        if (leftArm != null)
            leftArm.localRotation = Quaternion.Lerp(leftArm.localRotation, leftArmStartRot, Time.deltaTime * 10f);

        if (rightArm != null)
            rightArm.localRotation = Quaternion.Lerp(rightArm.localRotation, rightArmStartRot, Time.deltaTime * 10f);

        if (leftLeg != null)
            leftLeg.localRotation = Quaternion.Lerp(leftLeg.localRotation, leftLegStartRot, Time.deltaTime * 10f);

        if (rightLeg != null)
            rightLeg.localRotation = Quaternion.Lerp(rightLeg.localRotation, rightLegStartRot, Time.deltaTime * 10f);
    }
}