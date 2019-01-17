using System.Collections;
using UnityEngine;

public class tilePreview : MonoBehaviour {
    public bool autoUpdate;

    public TilemapSettings tilemapSettings;
    public Texture2D map;

    public void DrawTilemapInEditor() {
            TilemapGenerator Tilemap = new TilemapGenerator(map, tilemapSettings);
        }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawTilemapInEditor();
        }
    }
}