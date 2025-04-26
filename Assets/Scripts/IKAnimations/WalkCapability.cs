using System;
using UnityEngine;

public class WalkCapability : MonoBehaviour
{
    #region Public variables
    #endregion
    #region Private variables
    [SerializeField]
    private Transform leftFootTarget;
    [SerializeField]
    private Transform rightFootTarget;
    [SerializeField]
    private AnimationCurve horizontalCurve;
    [SerializeField]
    private AnimationCurve verticalCurve;
    private Vector3 leftTargetOffset;
    private Vector3 rightTargetOffset;
    private float leftLegLast;
    private float rightLegLast;
    #endregion
    #region Lifecycle
    private void Start()
    {
        leftTargetOffset = leftFootTarget.localPosition;
        rightTargetOffset = rightFootTarget.localPosition;
    }
    private void Update()
    {
        float leftLegForwardMovement = horizontalCurve.Evaluate(Time.time);
        float rightLegForwardMovement = horizontalCurve.Evaluate(Time.time - 1f);

        leftFootTarget.localPosition =
            leftTargetOffset +
            transform.InverseTransformVector(leftFootTarget.forward) * leftLegForwardMovement +
            transform.InverseTransformVector(leftFootTarget.up) * verticalCurve.Evaluate(Time.time + 0.5f);
        rightFootTarget.localPosition =
            rightTargetOffset +
            transform.InverseTransformVector(rightFootTarget.forward) * rightLegForwardMovement +
            transform.InverseTransformVector(rightFootTarget.up) * verticalCurve.Evaluate(Time.time - 0.5f);

        float leftLegDirection = leftLegForwardMovement - leftLegLast;
        float rightLegDirection = rightLegForwardMovement - rightLegLast;

        RaycastHit hit;
        // Check whether each foot is heading backwards, in that case clamp the target on the ground
        if (leftLegDirection < 0 && 
            Physics.Raycast(leftFootTarget.position + leftFootTarget.up, -leftFootTarget.up, out hit, Mathf.Infinity))
        {
            leftFootTarget.position = hit.point;
            transform.position += transform.forward * Mathf.Abs(leftLegDirection);
        }

        if (rightLegDirection < 0 &&
            Physics.Raycast(rightFootTarget.position + rightFootTarget.up, -rightFootTarget.up, out hit, Mathf.Infinity))
        {
            rightFootTarget.position = hit.point;
            transform.position += transform.forward * Mathf.Abs(rightLegDirection);
        }

        leftLegLast = leftLegForwardMovement;
        rightLegLast = rightLegForwardMovement;
    }
    #endregion
}