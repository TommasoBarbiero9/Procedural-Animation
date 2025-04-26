using UnityEngine;

public class FootIKTarget : MonoBehaviour
{
    #region Private variables
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform body;
    [SerializeField]
    private FootIKTarget otherFoot; // The limb we don't want to move simultaneously
    [Header("Step settings")]
    [SerializeField]
    private float speed; // The speed of the limb between each step
    [SerializeField]
    private float stepDistance; // The distance needed to make this limb move
    [SerializeField]
    private float stepLength; // The length of the actual movement
    [SerializeField]
    private float stepHeight; // How high the limb will go while making a step
    [SerializeField]
    private Vector3 footOffset; // We're using this to make each limb perfectly touch the ground
    private float footSpacing; // The distance on the x-axis between the target and the body
    private float footHeight; // The distance on the z-axis between the target and the body
    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float ratio;
    #endregion
    #region Lifecycle
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        footHeight = transform.localPosition.z;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        ratio = 1;
    }
    private void LateUpdate()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray(body.position + (body.right * footSpacing) + (body.forward * footHeight), Vector3.down);

        // Projecting our target on the ground
        if (Physics.Raycast(ray, out RaycastHit hit, 10, groundLayer.value))
        {
            // Check whether we need to make a step based on opposite limb's current state and distance from hit point
            if (ratio >= 1 && !otherFoot.IsMoving() && Vector3.Distance(newPosition, hit.point) > stepDistance)
            {
                ratio = 0;
                // Get the direction we should move in
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = hit.point + (body.forward * (stepLength * direction)) + footOffset;
                newNormal = hit.normal;
            }
        }

        // If the ratio is lower than 1 it means that we're currently making a step
        if (ratio < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, ratio);
            tempPosition.y += Mathf.Sin(ratio * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, ratio);
            ratio += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }
    #endregion
    #region Public methods
    public bool IsMoving() => ratio < 1;
    #endregion
}