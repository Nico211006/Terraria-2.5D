using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [Header("Main Parts")]
    public Transform visual;
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftLeg;
    public Transform rightLeg;

    [Header("Optional")]
    public Rigidbody rb;

    [Header("Walk Animation")]
    public float swingSpeed = 7f;
    public float armSwingAmount = 16f;
    public float legSwingAmount = 12f;
    public float maxSpeedForFullAnimation = 8f;

    [Header("Step Bob")]
    public float stepBobAmount = 0.02f;
    public float stepBobSpeedMultiplier = 1.8f;
    public float visualLerpSpeed = 8f;

    [Header("Smoothing")]
    public float rotationSmoothSpeed = 14f;
    public float idleResetSpeed = 10f;

    [Header("Mining Animation")]
    public float miningSpeed = 11f;
    public float miningAngle = 50f;
    public float leftArmMiningSupport = 10f;
    public float miningLegBend = 5f;

    [Header("Idle Life")]
    public float idleArmBreathAmount = 1.5f;
    public float idleBreathSpeed = 1.8f;

    private Quaternion leftArmStartRot;
    private Quaternion rightArmStartRot;
    private Quaternion leftLegStartRot;
    private Quaternion rightLegStartRot;

    private Vector3 visualStartLocalPos;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (leftArm != null) leftArmStartRot = leftArm.localRotation;
        if (rightArm != null) rightArmStartRot = rightArm.localRotation;
        if (leftLeg != null) leftLegStartRot = leftLeg.localRotation;
        if (rightLeg != null) rightLegStartRot = rightLeg.localRotation;

        if (visual != null)
            visualStartLocalPos = visual.localPosition;
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        bool isMovingInput = Mathf.Abs(moveInput) > 0.1f;

        bool isMining = false;
        if (!InventoryToggle.inventoryOpen)
        {
            isMining = Input.GetMouseButton(0);
        }

        float horizontalSpeed = 0f;
        if (rb != null)
        {
            horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        }

        bool isActuallyMoving = horizontalSpeed > 0.1f || isMovingInput;

        if (isMining)
        {
            AnimateMining();
        }
        else if (isActuallyMoving)
        {
            AnimateWalking(horizontalSpeed);
        }
        else
        {
            AnimateIdle();
        }
    }

    private void AnimateWalking(float horizontalSpeed)
    {
        float movePercent = Mathf.Clamp01(horizontalSpeed / maxSpeedForFullAnimation);
        float currentSwingSpeed = swingSpeed + (movePercent * 1.5f);

        float swing = Mathf.Sin(Time.time * currentSwingSpeed);
        float armSwing = swing * armSwingAmount * movePercent;
        float legSwing = swing * legSwingAmount * movePercent;

        Quaternion leftArmTarget = leftArmStartRot * Quaternion.Euler(armSwing, 0f, 0f);
        Quaternion rightArmTarget = rightArmStartRot * Quaternion.Euler(-armSwing, 0f, 0f);
        Quaternion leftLegTarget = leftLegStartRot * Quaternion.Euler(-legSwing, 0f, 0f);
        Quaternion rightLegTarget = rightLegStartRot * Quaternion.Euler(legSwing, 0f, 0f);

        SmoothBodyPart(leftArm, leftArmTarget, rotationSmoothSpeed);
        SmoothBodyPart(rightArm, rightArmTarget, rotationSmoothSpeed);
        SmoothBodyPart(leftLeg, leftLegTarget, rotationSmoothSpeed);
        SmoothBodyPart(rightLeg, rightLegTarget, rotationSmoothSpeed);

        AnimateStepBob(currentSwingSpeed, movePercent);
    }

    private void AnimateMining()
    {
        float miningSwing = Mathf.Sin(Time.time * miningSpeed) * miningAngle;
        float supportSwing = Mathf.Sin(Time.time * miningSpeed) * leftArmMiningSupport;

        Quaternion rightArmTarget = rightArmStartRot * Quaternion.Euler(-miningSwing, 0f, 0f);
        Quaternion leftArmTarget = leftArmStartRot * Quaternion.Euler(supportSwing, 0f, 0f);

        Quaternion leftLegTarget = leftLegStartRot * Quaternion.Euler(-miningLegBend, 0f, 0f);
        Quaternion rightLegTarget = rightLegStartRot * Quaternion.Euler(miningLegBend, 0f, 0f);

        SmoothBodyPart(rightArm, rightArmTarget, rotationSmoothSpeed);
        SmoothBodyPart(leftArm, leftArmTarget, rotationSmoothSpeed);
        SmoothBodyPart(leftLeg, leftLegTarget, rotationSmoothSpeed);
        SmoothBodyPart(rightLeg, rightLegTarget, rotationSmoothSpeed);

        ResetVisualPosition();
    }

    private void AnimateIdle()
    {
        float breath = Mathf.Sin(Time.time * idleBreathSpeed) * idleArmBreathAmount;

        Quaternion leftArmTarget = leftArmStartRot * Quaternion.Euler(breath, 0f, 0f);
        Quaternion rightArmTarget = rightArmStartRot * Quaternion.Euler(-breath, 0f, 0f);

        SmoothBodyPart(leftArm, leftArmTarget, idleResetSpeed);
        SmoothBodyPart(rightArm, rightArmTarget, idleResetSpeed);
        SmoothBodyPart(leftLeg, leftLegStartRot, idleResetSpeed);
        SmoothBodyPart(rightLeg, rightLegStartRot, idleResetSpeed);

        ResetVisualPosition();
    }

    private void AnimateStepBob(float currentSwingSpeed, float movePercent)
    {
        if (visual == null) return;

        float bob = Mathf.Abs(Mathf.Sin(Time.time * currentSwingSpeed * stepBobSpeedMultiplier))
                    * stepBobAmount * movePercent;

        Vector3 targetPos = visualStartLocalPos + new Vector3(0f, -bob, 0f);

        visual.localPosition = Vector3.Lerp(
            visual.localPosition,
            targetPos,
            Time.deltaTime * visualLerpSpeed
        );
    }

    private void ResetVisualPosition()
    {
        if (visual == null) return;

        visual.localPosition = Vector3.Lerp(
            visual.localPosition,
            visualStartLocalPos,
            Time.deltaTime * visualLerpSpeed
        );
    }

    private void SmoothBodyPart(Transform bodyPart, Quaternion targetRotation, float speed)
    {
        if (bodyPart == null) return;

        bodyPart.localRotation = Quaternion.Slerp(
            bodyPart.localRotation,
            targetRotation,
            Time.deltaTime * speed
        );
    }
}