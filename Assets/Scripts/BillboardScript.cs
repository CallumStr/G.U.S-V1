using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void LateUpdate()
    {
        if (mainCamera == null) return;
        
        // Get direction to camera (flattened to Y axis)
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.y = 0;
        
        if (direction == Vector3.zero) return;
        
        // Calculate base rotation to face camera
        Quaternion baseRotation = Quaternion.LookRotation(-direction);
        
        // Apply rotation
        transform.rotation = baseRotation;
        
        // Handle flip by scaling X instead of using flipX
        // This way the billboard rotation stays intact
        if (spriteRenderer != null)
        {
            // Check if sprite is flipped by parent script
            // We don't touch flipX here - let the controller handle it
        }
    }
}