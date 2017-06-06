using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    public MapData mapData;
    public const float outerRadius = 0.5f; // esto es 0.5f, pero para visualizar mejor lo he aumentado
    public const float innerRadius = outerRadius * 0.866025404f;

    [Header("Area")]
    public int vision;
    public int range;
    [Range(5,25)]
    public int size;
    [Range(5, 25)]
    public int hexagonN;

    public Dictionary<Vector2, MapBox> MapCell;

    public GameObject prefabTerrain;
    public GameObject prefabTree;
    public GameObject prefabRock;

    public Unit unit;

    private void Awake()
    {
        mapData.init();
        MapCell = new Dictionary<Vector2, MapBox>();
        CreateMap(mapData.mapShape);
        //PROVISIONAL
        MapBox aux;
        MapCell.TryGetValue(new Vector2(2, 2), out aux);
        unit.setMapCell(aux);
        initData();
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<EventEntitySelected>(calculateArea);
    }

    private void OnDisable()
    {
        EventManager.Instance.AddListener<EventEntitySelected>(calculateArea);
    }

    private void CreateMap(MapData.MapShape mapShape)
    {
        clearMap();
        if (mapShape == MapData.MapShape.HEXAGON)
        {
            initHexagonMap();
        }
        else if (mapShape == MapData.MapShape.SQUARE)
        {
            initSquareMap();
        }
        else
        {
            initRombMap();
        }
    }

    private void clearMap()
    {
        if (MapCell != null && MapCell.Count != 0)
        {
            var enumerator = MapCell.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Destroy(enumerator.Current.Value.gameObject);
            }
        }
    }

    public void initRombMap()
    {
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject terrain = Instantiate(prefabTerrain);
                terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, z);
                MapCell.Add(new Vector3(x, z), terrain.GetComponent<MapBoxScript>().mapbox);
                if (terrain.GetComponentInChildren<Text>() != null)
                {
                    terrain.GetComponentInChildren<Text>().text = "(" + z + "," + x + ")";
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
                int y = -x - z;
                if (Mathf.Abs(y) <= hexagonN)
                {
                    GameObject terrain = Instantiate(prefabTerrain);
                    terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                    terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, z);
                    MapCell.Add(new Vector3(x, z), terrain.GetComponent<MapBoxScript>().mapbox);
                    if (terrain.GetComponentInChildren<Text>() != null)
                    {
                        terrain.GetComponentInChildren<Text>().text = "(" + z + "," + x + ")";
                    }
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
                int valueRange = (x + (z / 2));
                if (valueRange > 0 && valueRange < size)
                {
                    GameObject terrain = Instantiate(prefabTerrain);
                    terrain.transform.position = new Vector3((x + z * 0.5f) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f));
                    terrain.GetComponent<MapBoxScript>().mapbox.setPosition(x, z);
                    MapCell.Add(new Vector3(x, z), terrain.GetComponent<MapBoxScript>().mapbox);
                    /*
                    if (mapData.terrainType[x, z] == 3)
                    {
                        GameObject tree = Instantiate(prefabTree);
                        tree.transform.SetParent(terrain.transform);
                        tree.transform.position = new Vector3(0, 0, 0);
                    }

                    if (mapData.terrainType[x, z] == 4)
                    {
                        GameObject rock = Instantiate(prefabRock);
                        rock.transform.SetParent(terrain.transform);
                        rock.transform.position = new Vector3(0, 0, 0);
                    }*/
                    if (terrain.GetComponentInChildren<Text>() != null)
                    {
                        terrain.GetComponentInChildren<Text>().text = "(" + z + "," + x + ")";
                    }
                }

            }
        }
    }

    private void initData()
    {
    }


    public void calculateArea(EventEntitySelected e)
    {
        MapBox originCell;
        MapCell.TryGetValue(e.UnitMapbox.cell, out originCell);
        if(originCell!= null)
        {
            List<Vector3> areaToCheck = HexagonCell.hexRange(originCell, vision);
            for (int i = 0; i < areaToCheck.Count; i++)
            {
                MapCell.TryGetValue(areaToCheck[i], out originCell);
                originCell.GetComponent<MapBoxScript>().changeColorToGreen();
            }
        }
        else
        {
            Debugger.printErrorLog("There is not such origin cellHexMap.");
        }
    }

   
    public void calculateTransitArea(Vector2 originPos)
    {
        MapBox originCell;
        MapCell.TryGetValue(originPos, out originCell);
        if (originCell != null)
        {
            List<Vector3> areaToCheck = HexagonCell.hexReacheable(originCell, vision);
            for (int i = 0; i < areaToCheck.Count; i++)
            {
                MapCell.TryGetValue(new Vector2((int)HexagonCell.cubeToAxial(areaToCheck[i]).x, (int)HexagonCell.cubeToAxial(areaToCheck[i]).y), out originCell);
                if (originCell.terrainType == MapBox.TerrainType.SIMPLE)
                {
                    originCell.GetComponent<MapBoxScript>().changeColorToGreen();
                }
                else
                {
                    originCell.GetComponent<MapBoxScript>().changeColorToRed();
                }
            }
        }
        else
        {
            Debugger.printErrorLog("There is not such origin cellHexMap.");
        }
    }

    public void calculateLineOfSight(Vector2 originPos, Vector2 destinyPos)
    {
        MapBox originCell;
        MapCell.TryGetValue(originPos, out originCell);
        MapBox destinyCell;
        MapCell.TryGetValue(destinyPos, out destinyCell);
        if (originCell != null && destinyCell != null)
        {
            List<Vector3> line = new List<Vector3>();
            line = HexagonCell.hexLineOfSight(originCell, destinyCell);
                for (int i = 0; i < line.Count; i++)
                {
                    MapCell.TryGetValue(new Vector2((int)HexagonCell.cubeToAxial(line[i]).x, (int)HexagonCell.cubeToAxial(line[i]).y), out originCell);
                    if (originCell.terrainType == MapBox.TerrainType.SIMPLE)
                    {
                        originCell.GetComponent<MapBoxScript>().changeColorToGreen();
                    }
                    else
                    {
                        originCell.GetComponent<MapBoxScript>().changeColorToRed();
                    }
            }
        }
        else
        {
            Debugger.printErrorLog("There is not such origin/destiny cellHexMaps.");
        }
    }

    private void Update()
    {

    }

    public void Save(BinaryWriter writer)
    {
        var enumerator = MapCell.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.Save(writer);
        }
    }

    public void Load(BinaryReader reader)
    {
        var enumerator = MapCell.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.Load(reader);
        }
    }
}
