using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PREFABS", menuName = "MAPBOXPREFABS", order = 0)]
public class MapBoxPrefabs : ScriptableObject {
    [Header(" TERRRAIN - SIMPLES")]
    public GameObject[] Simple;
    [Header("TERRAIN - BLOCKED")]
    public GameObject[] Blocked;
    [Header("TERRAIN - OBSTACLE")]
    public GameObject[] Obstacle;
}
