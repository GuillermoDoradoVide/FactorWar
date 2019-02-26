using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapBoxPrefabs : ScriptableObject {
    [Header(" TERRRAIN - SIMPLES")]
    public GameObject[] Simple;
    [Header("TERRAIN - BLOCKED")]
    public GameObject[] Blocked;
    [Header("TERRAIN - OBSTACLE")]
    public GameObject[] Obstacle;
}
