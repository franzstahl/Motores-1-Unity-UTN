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

    private float rotX;
    private float rotY;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;

        rotX = angles.y;
        rotY = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        gameObject.GetComponent<CinemachineBrain>().enabled = false;
    }

    private void LateUpdate()
    {
        if (!target) return;

        // Rotación con mouse
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotY = Mathf.Clamp(rotY, minY, maxY);

        // Rotación de la cámara
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);


        Vector3 pivot = target.position + Vector3.up * height;

        // Posición ideal de la cámara
        Vector3 desiredPosition = pivot - rotation * Vector3.forward * distance;

        // Dirección desde el personaje hacia la cámara
        Vector3 direction = desiredPosition - target.position;
        float desiredDistance = direction.magnitude;

        direction.Normalize();

        Vector3 finalPosition = desiredPosition;

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
            finalPosition = pivot + direction * hit.distance;
        }

        // Aplicar posición y rotación
        transform.position = finalPosition;
        transform.rotation = rotation;
    }
}