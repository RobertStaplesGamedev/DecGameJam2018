using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TilemapGenerator))]
public class TilemapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        TilemapGenerator tilemapGenerator = (TilemapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (tilemapGenerator.autoUpdate)
            {
                tilemapGenerator.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            tilemapGenerator.DrawMapInEditor();
        }
    }
}