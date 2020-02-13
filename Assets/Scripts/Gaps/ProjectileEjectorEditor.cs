using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProjectileEjector)), CanEditMultipleObjects]
public class ProjectileEjectorEditor : Editor
{
    private void OnSceneGUI()
    {
        ProjectileEjector ejector = (ProjectileEjector)target;

        EditorGUI.BeginChangeCheck();
        Vector3 oldWorldSpacePosition = ejector.transform.TransformPoint(ejector.spawnPosition);
        Handles.Label(oldWorldSpacePosition, "Spawn position");
        Vector3 newWorldSpacePosition = Handles.PositionHandle(oldWorldSpacePosition, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(ejector, "Change Projectile Ejector Spawn Position");
            ejector.spawnPosition = ejector.transform.InverseTransformPoint(newWorldSpacePosition);
        }
    }
}
