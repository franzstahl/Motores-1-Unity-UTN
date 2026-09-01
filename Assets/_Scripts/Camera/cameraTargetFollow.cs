using UnityEngine;

/// <summary>
/// Keeps this transform's POSITION locked to a target (e.g. the player) without
/// inheriting the target's rotation. Use this on a dedicated "CameraParent" object
/// that is NOT parented under the player, and point Cinemachine's Tracking Target
/// at this object instead of the player directly.
///
/// This way, CinemachinePanTilt (rotation) and CinemachineThirdPersonFollow (position)
/// both read from the same object, but that object's rotation is driven purely by
/// mouse input (via CinemachineInputAxisController), never by the player's own facing.
/// </summary>
public class cameraTargetFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.6f, 0f); // roughly chest/head height

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        // Deliberately not touching rotation here — Pan Tilt owns it.
    }
}