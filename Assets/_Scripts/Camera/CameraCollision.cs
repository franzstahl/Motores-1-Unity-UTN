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
    [SerializeField] private float recoverSpeed;

    private float currentDistance;
    private float rotX;
    private float rotY;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;

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
            // hit.distance is where the sphere's center stops, so no extra offset is needed
            targetDistance = hit.distance;
        }

        if (targetDistance < currentDistance)
        {
            currentDistance = targetDistance;
        }
        else
        {
            currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, recoverSpeed * Time.deltaTime);
        }

        Vector3 finalPosition = pivot + direction * currentDistance;

        // Apply the final position and rotation to the camera
        transform.position = finalPosition;
        transform.rotation = rotation;
    }
}