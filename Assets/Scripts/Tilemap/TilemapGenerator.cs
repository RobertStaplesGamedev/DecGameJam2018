using UnityEngine;

public class TilemapGenerator : MonoBehaviour {

    public Texture2D map;
    public Color colourOfEmptySpace;

    public ColourToPrefab[] colourMappings;

    void Start() {
        GenerateLevel();
    }

    void GenerateLevel() {
        for (int x = 0; x < map.width; x++) {
            for (int y = 0; y < map.width; y++) {
                GenerateTile(x,y);
            }
        }
    }

    void GenerateTile(int x, int y) {
        Color pixelColour = map.GetPixel(x,y);

        if (pixelColour.a == 0 || (colourOfEmptySpace != null && colourOfEmptySpace == pixelColour)) {
            //there is nothing here, return
            return;
        }
        
        foreach (ColourToPrefab colourMapping in colourMappings) {
            if (colourMapping.colour.Equals(pixelColour)) {
                Vector2 position = new Vector2(x,y);
                Instantiate(colourMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }
}