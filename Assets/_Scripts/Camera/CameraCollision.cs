using UnityEngine;
using Unity.Cinemachine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float sensitivity;

    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Camera Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private float recoverSpeed; // Speed at which the camera returns to its original distance after being pushed in by a collision

    private float currentDistance; // The current distance of the camera from the target, which may be less than the desired distance due to collisions
    private float rotX;
    private float rotY;

    private void Start()
    {

        Vector3 angles = transform.eulerAngles;
        rotY = Mathf.DeltaAngle(0f, angles.x); // Converts 350 to -10 so the Min Y / Max Y clamp works

        rotX = angles.y;
        rotY = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        gameObject.GetComponent<CinemachineBrain>().enabled = false;
        currentDistance = distance;
    }

    private void LateUpdate()
    {
        if (!target) return;

        // Rotation with mouse input
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotY = Mathf.Clamp(rotY, minY, maxY);

        // Rotation of the camera
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);


        Vector3 pivot = target.position + Vector3.up * height;

        // Ideal position of the camera based on rotation and distance
        Vector3 desiredPosition = pivot - rotation * Vector3.forward * distance;

        // Direction from the character to the camera
        Vector3 direction = desiredPosition - pivot;
        float desiredDistance = direction.magnitude;

        direction.Normalize();

        float targetDistance = desiredDistance;

        // SphereCast instead of Raycast: the camera has volume, a thin ray lets it poke into walls
        if (Physics.SphereCast(
            pivot,
            collisionRadius,
            direction,
            out RaycastHit hit,
            desiredDistance,
            collisionMask,
            QueryTriggerInteraction.Ignore)) // Triggers (room zones, etc.) must not push the camera
        {
            // If the SphereCast hits something, we set the target distance to the hit distance
            targetDistance = hit.distance;
        }

        // Smoothly adjust the current distance towards the target distance
        if (targetDistance < currentDistance)
        {
            currentDistance = targetDistance;
        }
        else
        {
            currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, recoverSpeed * Time.deltaTime); // Smoothly recover to the original distance when not colliding
        }

        Vector3 finalPosition = pivot + direction * currentDistance; // Calculate the final position of the camera based on the current distance

        // Apply the final position and rotation to the camera
        transform.position = finalPosition;
        transform.rotation = rotation;
    }
}