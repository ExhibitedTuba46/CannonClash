using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Game_Grid))]
public class Button : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Game_Grid grid = (Game_Grid)target;
        if(GUILayout.Button("Locate Neighbours"))
        {
            grid.LocateTileNeighbours();
        }
    }
}
