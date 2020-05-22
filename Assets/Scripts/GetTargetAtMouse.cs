using UnityEngine;
using UnityEngine.Events;

public class GetTargetAtMouse : MonoBehaviour
{
    public Camera PlayerCamera;
    public float Range = 10f;
    public float RoundingFactor = 1f;
    public bool Enabled = true;
    public UnityEvent<Vector3?> OnNewMouseTarget;
    public Vector3 TargetPosition { get; private set; }
    private string buildableAreaTag = "BuildableArea";

    void Start()
    {
        if (PlayerCamera == null)
        {
            PlayerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (!Enabled)
        {
            return;
        }

        // Get the direction that the camera is facing
        Vector3 facingDirection = PlayerCamera.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward));

        Ray ray = new Ray(PlayerCamera.transform.position, facingDirection);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, Range);

        foreach (RaycastHit raycastHit in raycastHits)
        {
            if (raycastHit.transform.CompareTag(buildableAreaTag))
            {
                if (RoundingFactor != 0)
                {
                    TargetPosition = GetRoundedVector3(raycastHit.point, RoundingFactor);
                }
                else
                {
                    TargetPosition = raycastHit.point;
                }

                OnNewMouseTarget.Invoke(TargetPosition);

                return;
            }
        }

        OnNewMouseTarget.Invoke(null);
    }

    public static Vector3 GetRoundedVector3(Vector3 value, float factor)
    {
        return new Vector3(UtilityMethods.RoundToNearestFactor(value.x, factor), value.y, UtilityMethods.RoundToNearestFactor(value.z, factor));
    }
}
