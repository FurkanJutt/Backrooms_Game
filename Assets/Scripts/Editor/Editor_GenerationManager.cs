using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerationManager))]
public class Editor_GenerationManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GenerationManager generationManager = (GenerationManager)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Update Map Size"))
        {
            generationManager.UpdateMapSize();
        }
        if (GUILayout.Button("Reload World"))
        {
            generationManager.ReloadWorld();
        }
        if (GUILayout.Button("Generate World"))
        {
            generationManager.GenerateWorld();
        }
        if (GUILayout.Button("Spawn Player"))
        {
            generationManager.SpawnPlayer();
        }
    }
}
