using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TarMan))]
public class TarManEditor : Editor
{
	private void OnSceneGUI()
	{
		TarMan tarMan = (TarMan)target;

		DrawPositionHandle(ref tarMan.walkingState.patrolPosition1, "Pos1");
		DrawPositionHandle(ref tarMan.walkingState.patrolPosition2, "Pos2");

		void DrawPositionHandle(ref Vector2 field, string label)
		{
			EditorGUI.BeginChangeCheck();
			Handles.Label(field, label);
			Vector2 newPosition = Handles.PositionHandle(field, Quaternion.identity);
			newPosition.y = field.y;
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(tarMan, label + " moved");
				field = newPosition;
			}
		}
	}
}
