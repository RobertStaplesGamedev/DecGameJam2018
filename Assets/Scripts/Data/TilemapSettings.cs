using System.Collections;
using UnityEngine;

[CreateAssetMenu()]
public class TilemapSettings : UpdatableData {
    
     public ColourToPrefab[] colourMappings;
     public Color colourOfEmptySpace;

}
[System.Serializable]
public class ColourToPrefab {
    public Color colour;
    public GameObject prefab;

}