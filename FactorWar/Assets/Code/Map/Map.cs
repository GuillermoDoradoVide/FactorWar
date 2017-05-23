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
    [SerializeField]
    public MapBox[,] cells;
    public Vector2 size;
    public const float outerRadius = 0.6f; // esto es 0.5f, pero para visualizar mejor lo he aumentado
    public const float innerRadius = outerRadius * 0.866025404f;

    public GameObject prefabTerrain;

    private void Start()
    {
        cells = new MapBox[(int)size.x, (int)size.y];
        initMap(MapSize.SMALL);
    }

    private void initMap(MapSize mapSize)
    {
        for (int z = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GameObject terrain = Instantiate(prefabTerrain);
                terrain.transform.position = new Vector3((x + z * 0.5f - z / 2) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                terrain.GetComponent<MapBoxScript>().mapbox.setPosition(HexagonCell.axialToCube(new Vector2(x,z)).x, HexagonCell.axialToCube(new Vector2(x, z)).z, HexagonCell.axialToCube(new Vector2(x, z)).y);
                cells[x, z] = terrain.GetComponent<MapBoxScript>().mapbox;
            }
        }
    }
    [ContextMenu(itemName:"Area")]
    public void calculateArea()
    {
        List<Vector3> areaToCheck = HexagonCell.hexRange(cells[10,10], 3);
        for (int i = 0; i < areaToCheck.Count; i ++)
        {
            cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].GetComponent<MapBoxScript>().changeColorToGreen();
        }
        
    }
    [ContextMenu(itemName: "Obstacles")]
    public void calculateTransitArea()
    {
        List<Vector3> areaToCheck = HexagonCell.hexReacheable(cells[10, 10], 5);
        for (int i = 0; i < areaToCheck.Count; i++)
        {
            if (cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].terrainType != MapBox.TerrainType.SIMPLE)
            {
                Debugger.printLog("obstacle: " + areaToCheck[i]);
                cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].GetComponent<MapBoxScript>().changeColorToRed();
            }
            else
            {
                cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].GetComponent<MapBoxScript>().changeColorToGreen();
            }
        }

    }

    private void Update()
    {

    }
}
