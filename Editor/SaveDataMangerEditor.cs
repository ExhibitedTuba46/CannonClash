using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveDataManager))]
public class SaveDataMangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveDataManager dataManager = (SaveDataManager)target;

        if (GUILayout.Button("Save Data"))
        {
            dataManager.WriteData();
        }

        if (GUILayout.Button("Load Data"))
        {
            dataManager.ReadData();
        }
    }
}
