using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[System.Serializable]
public struct BoxPosition
{
    public float xPos;
    public float yPos;
    public float zPos;
};

public class Map : MonoBehaviour
{
    [SerializeField]
    public MapBox[,] cells;
    
    public const float outerRadius = 0.6f; // esto es 0.5f, pero para visualizar mejor lo he aumentado
    public const float innerRadius = outerRadius * 0.866025404f;

    [Header("Area")]
    public int vision;
    public int range;
    public enum MapSize { SMALL, MEDIUM };
    public MapSize mapSize;
    public enum MapShape { NORMAL, SQUARE, HEXAGON };
    public MapShape mapShape;
    [Range(5,25)]
    public int size;
    [Range(5, 25)]
    public int hexagonN;

    public Dictionary<Vector2, MapBox> MapHex;

    public GameObject prefabTerrain;

    private void Start()
    {
        MapHex = new Dictionary<Vector2, MapBox>();
        initMap(mapSize, mapShape);
    }

    private void initMap(MapSize mapSize, MapShape mapShape)
    {
        if (mapShape == MapShape.HEXAGON)
        {
            cells = new MapBox[(hexagonN + size), (hexagonN + size)];
            Debugger.printLog("MapHexSize: " + cells.Length);
            initHexagonMap();
        }
        else if (mapShape == MapShape.SQUARE)
        {
            cells = new MapBox[size, (int)(size * 1.5)];
            initSquareMap();
        }
        else
        {
            cells = new MapBox[size, size];
            for (int z = 0; z < size; z++)
            {
                for (int x = 0; x < size; x++)
                {
                    int y = -z - x;
                    //GameObject terrain = Instantiate(prefabTerrain);
                    // terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                    //terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, y, z);

                    // terrain.GetComponent<MapBoxScript>().mapbox.setPosition(HexagonCell.axialToCube(new Vector2(z,x)).x, HexagonCell.axialToCube(new Vector2(z, x)).y, HexagonCell.axialToCube(new Vector2(z,x)).z);
                    //cells[z, x + z / 2] = terrain.GetComponent<MapBoxScript>().mapbox; /* square map */
                    //cells[(int)(z + size.y),(int)( x + size.x + (Mathf.Min(0, z)))] = terrain.GetComponent<MapBoxScript>().mapbox; /* hexagon map */

                    GameObject terrain = Instantiate(prefabTerrain);
                    terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                    terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, y, z);
                    MapHex.Add(new Vector2((int)(z + size), (int)(x + size + (Mathf.Min(0, z)))), terrain.GetComponent<MapBoxScript>().mapbox);
                    //cells[z, x] = terrain.GetComponent<MapBoxScript>().mapbox;
                    if (terrain.GetComponentInChildren<Text>() != null)
                    {
                        terrain.GetComponentInChildren<Text>().text = "(" + z + "," + x + ")";
                    }

                }
            }
        }

    }

    public void initHexagonMap()
    {
        for (int z = -hexagonN; z < hexagonN; z++)
        {
            for (int x = -hexagonN; x < hexagonN; x++)
            {
                int y = -z - x;
                if (Mathf.Abs(y) <= hexagonN)
                {
                    GameObject terrain = Instantiate(prefabTerrain);
                    terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                    terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, y, z);
                    MapHex.Add(new Vector2((z + size), (x + size + (Mathf.Min(0, z)))), terrain.GetComponent<MapBoxScript>().mapbox);
                }
            }
        }
    }

    public void initSquareMap()
    {
        for (int z = 0; z < size; z++)
        {
            for (int x = (int)-size/2; x < (size + Mathf.FloorToInt(z / 2)); x++)
            {
                int y = -z - x;
                int valueRange = (x + (z / 2));
                if (valueRange > 0 && valueRange < size)
                {
                    GameObject terrain = Instantiate(prefabTerrain);
                    terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                    terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, y, z);
                    MapHex.Add(new Vector2((z + size), (x + size + (Mathf.Min(0, z)))), terrain.GetComponent<MapBoxScript>().mapbox);
                }

            }
        }
    }

    [ContextMenu(itemName:"Area")]
    public void calculateArea()
    {
        MapBox aux;
        MapHex.TryGetValue(new Vector2(0,0), out aux);
        
        List<Vector3> areaToCheck = HexagonCell.hexRange(aux, vision);
        for (int i = 0; i < areaToCheck.Count; i ++)
        {
            MapHex.TryGetValue(new Vector2((int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y), out aux);
            aux.GetComponent<MapBoxScript>().changeColorToGreen();
        } 
    }

    [ContextMenu(itemName: "Obstacles")]
    public void calculateTransitArea()
    {
        List<Vector3> areaToCheck = HexagonCell.hexReacheable(cells[10, 10], vision);
        for (int i = 0; i < areaToCheck.Count; i++)
        {
            if (cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].terrainType != MapBox.TerrainType.SIMPLE)
            {
                Debugger.printLog("obstacle: " + areaToCheck[i]);
                cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].GetComponent<MapBoxScript>().changeColorToRed();
            }
            else
            {
                Debugger.printLog(i);
                cells[(int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y].GetComponent<MapBoxScript>().changeColorToGreen();
            }
        }

    }
    [ContextMenu(itemName: "Line of sight")]
    public void calculateLineOfSight(/*List<Vector3> rangeOfSight*/)
    {
        List<Vector3> line = new List<Vector3>();
        line = HexagonCell.hexLineOfSight(cells[10, 10].cell, cells[19, 19].cell);
        for (int i = 0; i < line.Count; i ++)
        {
            if (cells[(int)HexagonCell.cubeToAxial(line[i]).x, (int)HexagonCell.cubeToAxial(line[i]).y].terrainType != MapBox.TerrainType.SIMPLE)
            {
                Debugger.printLog("obstacle: " + line[i]);
                cells[(int)HexagonCell.cubeToAxial(line[i]).x, (int)HexagonCell.cubeToAxial(line[i]).y].GetComponent<MapBoxScript>().changeColorToRed();
            }
            else
            {
                cells[(int)HexagonCell.cubeToAxial(line[i]).x, (int)HexagonCell.cubeToAxial(line[i]).y].GetComponent<MapBoxScript>().changeColorToGreen();
            }
        }
    }

    private void Update()
    {

    }
}
