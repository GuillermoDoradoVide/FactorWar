using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour {

    public enum MapSize { SMALL, MEDIUM, BIG };
    public MapSize mapSize;
    public enum MapShape { NORMAL, SQUARE, HEXAGON };
    public MapShape mapShape;

    public GameObject[][] prefabs;
    [System.Serializable]
    public enum CellType {SIMPLE, OBSTACLE, TREES, ROCK, RIVER, HOLE }
    public CellType cellType;
    [SerializeField]
    public int[,] terrainType;

    public void init()
    {
        terrainType = new int[5, 5];
        for(int x = 0; x < 5; x ++)
        {
            for (int y = 0; y < 5; y++)
            {
                terrainType[x,y] = (int)Random.Range(0, 4);
            }
        }
    }

}
