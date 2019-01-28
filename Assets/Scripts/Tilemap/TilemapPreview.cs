using System.Collections;
using UnityEngine;

public class tilePreview : MonoBehaviour {
    public bool autoUpdate;

    public TilemapSettings tilemapSettings;
    public Texture2D map;

    public void GenerateLevel() {
            TilemapGenerator Tilemap = new TilemapGenerator(map, tilemapSettings);
        }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            GenerateLevel();
        }
    }
}