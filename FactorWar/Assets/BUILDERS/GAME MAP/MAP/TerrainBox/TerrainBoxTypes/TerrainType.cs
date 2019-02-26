using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainType", menuName = "MAP/TerrainBox/TerrainBoxType", order = 0)]
public class TerrainType : ScriptableObject
{
    [Header("- Features")]
    public List<TerrainFeature> terrainFeature;
}
