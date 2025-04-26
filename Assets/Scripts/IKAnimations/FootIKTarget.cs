using UnityEngine;

public class FootIKTarget : MonoBehaviour
{
    #region Private variables
    [SerializeField]
    private LayerMask groudLayer;
    [SerializeField]
    private Transform body;
    [SerializeField]
    private FootIKTarget otherFoot;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float stepDistance;
    [SerializeField]
    private float stepLength;
    [SerializeField]
    private float stepHeight;
    [SerializeField]
    private Vector3 footOffset;
    private float footSpacing;
    private float footHeight;
    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float lerp;
    #endregion
    #region Lifecycle
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        footHeight = transform.localPosition.z;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }
    private void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;
        
        Ray ray = new Ray(body.position + (body.right * footSpacing) + (body.forward * footHeight), Vector3.down);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 10, groudLayer.value))
        {
            Debug.Log($"{gameObject.name} hitting ground");
            if (lerp >= 1 && !otherFoot.IsMoving() && Vector3.Distance(newPosition, hit.point) > stepDistance)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = hit.point + (body.forward * (stepLength * direction)) + footOffset;
                newNormal = hit.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            
            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }
    #endregion
    #region Public methods
    public bool IsMoving() => lerp < 1;
    #endregion
}
