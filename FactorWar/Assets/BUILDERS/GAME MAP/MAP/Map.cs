using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    //datos y caracteristicas del mapa, Tamaño, etc.
    public const float outerRadius = 0.5f; // esto es 0.5f, pero para visualizar mejor lo he aumentado
    public const float innerRadius = outerRadius * 0.866025404f;
    private int _size = 10;

    public Dictionary<Vector2, MapBox> MapGrid;

    private HexGridManager hexGridManager;

    private void Awake()
    {
        MapGrid = new Dictionary<Vector2, MapBox>();
    }




}
