using UnityEngine;

[DefaultExecutionOrder(100)] // Runs after PlayerMovement, so after the animation and the armature rotation
[RequireComponent(typeof(CharacterController))]
public class HeadAndTailCOllision : MonoBehaviour
{
    [Header("Limbs")]
    [SerializeField] private Collider[] limbColliders; // Head and tail colliders (they are trigger for easier checks)

    [Header("Settings")]
    [SerializeField] private LayerMask environmentMask = ~0; // Exclude the Player layer here
    [SerializeField] private float maxPushPerFrame = 0.3f; // Safety cap so a bad overlap never teleports the player
    [SerializeField] private int iterations = 2; // More passes resolve corners better
    [SerializeField] private bool ignoreWhileClimbing = true; // The head touches the wall by design while climbing

    private readonly Collider[] overlaps = new Collider[16];
    private CharacterController controller;
    private PlayerMovement movement;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        if (ignoreWhileClimbing && movement.CurrentState == MovementState.Climbing) return;

        Vector3 totalPush = Vector3.zero;

        for (int i = 0; i < iterations; i++)
        {
            Vector3 pushThisPass = Vector3.zero;

            foreach (Collider limb in limbColliders)
            {
                if (limb == null || !limb.enabled) continue;
                pushThisPass += ComputeLimbPush(limb);
            }

            if (pushThisPass.sqrMagnitude < 0.000001f) break; // Nothing left to resolve

            // Respect the total cap across all iterations
            Vector3 remaining = Vector3.ClampMagnitude(totalPush + pushThisPass, maxPushPerFrame) - totalPush;
            controller.Move(remaining);
            remaining.y = 0f; // Head and tail only push horizontally, never fight gravity
            totalPush += remaining;
            Physics.SyncTransforms(); // Colliders moved with the transform, make the physics world aware
        }
    }

    private Vector3 ComputeLimbPush(Collider limb)
    {
        Bounds b = limb.bounds;
        int count = Physics.OverlapBoxNonAlloc(b.center, b.extents, overlaps, Quaternion.identity,
                                               environmentMask, QueryTriggerInteraction.Ignore);

        Vector3 push = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            Collider other = overlaps[i];
            if (other == controller || other.transform.IsChildOf(transform)) continue; // Skip ourselves

            if (Physics.ComputePenetration(
                    limb, limb.transform.position, limb.transform.rotation,
                    other, other.transform.position, other.transform.rotation,
                    out Vector3 direction, out float distance))
            {
                // Keep the deepest push per axis instead of summing, so two overlaps don't double the correction
                Vector3 p = direction * distance;
                if (Mathf.Abs(p.x) > Mathf.Abs(push.x)) push.x = p.x;
                if (Mathf.Abs(p.y) > Mathf.Abs(push.y)) push.y = p.y;
                if (Mathf.Abs(p.z) > Mathf.Abs(push.z)) push.z = p.z;
            }
        }

        return push;
    }
}