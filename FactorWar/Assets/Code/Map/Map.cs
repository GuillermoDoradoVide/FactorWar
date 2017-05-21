using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BoxPosition
{
    public float xPos;
    public float yPos;
};

public class Map : MonoBehaviour
{
    public enum MapSize { SMALL, MEDIUM };
    public List<GameObject> cells;
    public float width;
    public float height;
    public const float outerRadius = 0.5f;
    public const float innerRadius = outerRadius * 0.866025404f;

    public GameObject prefabTerrain;

    private void Start()
    {
        cells = new List<GameObject>();
        initMap(MapSize.SMALL);
    }

    private void initMap(MapSize mapSize)
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject terrain = Instantiate(prefabTerrain);
                terrain.transform.position = new Vector3((x + z * 0.5f - z / 2) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, z);
                cells.Add(terrain);
            }
        }
    }

    private void Update()
    {

    }
}
