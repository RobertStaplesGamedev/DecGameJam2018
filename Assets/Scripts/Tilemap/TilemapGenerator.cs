using UnityEngine;

public class TilemapGenerator : MonoBehaviour {

    public bool autoUpdate;

    Texture2D map;
    TilemapSettings tilemapSettings;

    void Start() {
        GenerateLevel();
    }
    public TilemapGenerator(Texture2D _map, TilemapSettings _tilemapSettings) {
        map = _map;
        tilemapSettings = _tilemapSettings;
    }
    public void GenerateLevel() {
        for (int x = 0; x < map.width; x++) {
            for (int y = 0; y < map.width; y++) {
                GenerateTile(tilemapSettings, x,y);
            }
        }
    }

    void GenerateTile(TilemapSettings tilemapSettings, int x, int y) {
        Color pixelColour = map.GetPixel(x,y);

        if (pixelColour.a == 0 || (tilemapSettings.colourOfEmptySpace != null && tilemapSettings.colourOfEmptySpace == pixelColour)) {
            //there is nothing here, return
            return;
        }
        
        foreach (ColourToPrefab colourMapping in tilemapSettings.colourMappings) {
            if (colourMapping.colour.Equals(pixelColour)) {
                Vector2 position = new Vector2(x,y);
                Instantiate(colourMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }
}