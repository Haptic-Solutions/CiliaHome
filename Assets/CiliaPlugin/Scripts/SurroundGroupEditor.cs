using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Cilia))]
public class SurroundGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Cilia ciliaInstance = (Cilia)target;

        if (GUILayout.Button("Update Surround Group List"))
        {
            ciliaInstance.AddSurroundGroups();
        }
        AssetDatabase.Refresh();
    }
}
