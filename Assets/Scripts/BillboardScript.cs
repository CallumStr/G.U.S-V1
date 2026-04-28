using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
    [Header("Billboard Settings")]
    [Tooltip("If true, sprite stays upright and only rotates around Y axis. If false, sprite fully faces camera (can tilt).")]
    public bool uprightBillboard = true;

    [Tooltip("Offset angle in degrees if sprite faces wrong direction.")]
    public float rotationOffset = 180f;

    [Tooltip("Smooth rotation speed (0 = instant).")]
    public float rotationSpeed = 0f;

    [Header("Optional")]
    [Tooltip("Assign a specific camera. Leave empty to use Camera.main.")]
    public Camera targetCamera;

    private Transform camTransform;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetCamera = targetCamera != null ? targetCamera : Camera.main;
        
        if (targetCamera != null)
            camTransform = targetCamera.transform;
    }

    void LateUpdate()
    {
        if (camTransform == null)
        {
            // Try to recover if camera was destroyed or not ready at Start
            targetCamera = targetCamera != null ? targetCamera : Camera.main;
            if (targetCamera != null)
                camTransform = targetCamera.transform;
            else
                return;
        }

        Quaternion targetRotation;

        if (uprightBillboard)
        {
            // --- UPRIGHT BILLBOARD (Y-axis only) ---
            Vector3 toCamera = camTransform.position - transform.position;
            toCamera.y = 0f;

            if (toCamera.sqrMagnitude < 0.0001f) return;

            float angle = Mathf.Atan2(toCamera.x, toCamera.z) * Mathf.Rad2Deg + rotationOffset;
            targetRotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            // --- FULL BILLBOARD (faces camera directly) ---
            targetRotation = camTransform.rotation;
            
            // Apply offset by rotating around the billboard's up axis
            if (rotationOffset != 0f)
                targetRotation *= Quaternion.Euler(0f, rotationOffset, 0f);
        }

        // Apply rotation
        if (rotationSpeed > 0f)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        else
            transform.rotation = targetRotation;
    }

    /// <summary>
    /// Call this if you need to manually refresh the camera reference at runtime.
    /// </summary>
    public void RefreshCamera()
    {
        targetCamera = targetCamera != null ? targetCamera : Camera.main;
        if (targetCamera != null)
            camTransform = targetCamera.transform;
    }
}