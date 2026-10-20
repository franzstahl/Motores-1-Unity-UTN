using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class ExteriorCleanup
{
    [MenuItem("Tools/Clean Selected Exterior")]
    private static void Clean()
    {
        foreach (GameObject root in Selection.gameObjects)
        {
            // Unpack prefab instances first, otherwise their components can't be removed
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                if (PrefabUtility.IsOutermostPrefabInstanceRoot(t.gameObject))
                    PrefabUtility.UnpackPrefabInstance(t.gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);
            }

            // Remove every collider under the selected parent
            foreach (Collider col in root.GetComponentsInChildren<Collider>(true))
                Undo.DestroyObjectImmediate(col);

            // Turn off shadow casting on every mesh renderer
            foreach (MeshRenderer mr in root.GetComponentsInChildren<MeshRenderer>(true))
            {
                Undo.RecordObject(mr, "Disable cast shadows");
                mr.shadowCastingMode = ShadowCastingMode.Off;
            }
        }
    }
}