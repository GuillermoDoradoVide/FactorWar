using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    MapShape mapShape;
    MapSize mapSize;
    public const float outerRadius = 0.5f; // esto es 0.5f, pero para visualizar mejor lo he aumentado
    public const float innerRadius = outerRadius * 0.866025404f;
    private int size;

    public Dictionary<Vector2, MapBox> MapCell;

    public GameObject prefabTerrain;
    public GameObject prefabTree;
    public GameObject prefabRock;

    public Unit unit;

    private void Awake()
    {
        MapCell = new Dictionary<Vector2, MapBox>();
        CreateMap(mapShape, mapSize);
        //PROVISIONAL
        MapBox aux;
        MapCell.TryGetValue(new Vector2(2, 2), out aux);
        unit.setMapCell(aux);
    }

    public void setsize(int newSize)
    {
        switch(newSize) {
            case 0:
                {
                    mapSize = MapSize.SMALL;
                    break;
                }
            case 1:
                {
                    mapSize = MapSize.MEDIUM;
                    break;
                }
            case 2:
                {
                    mapSize = MapSize.BIG;
                    break;
                }
        }
    }

    public void setShape(int newShape)
    {
        switch (newShape)
        {
            case 0:
                {
                    mapShape = MapShape.HEXAGON;
                    break;
                }
            case 1:
                {
                    mapShape = MapShape.SQUARE;
                    break;
                }
            case 2:
                {
                    mapShape = MapShape.NORMAL;
                    break;
                }
        }
    }

    public void createNewMap()
    {
        CreateMap(mapShape, mapSize);
    }

    private void CreateMap(MapShape mapShape, MapSize mapSize)
    {
        clearMap();
        if (mapSize == MapSize.BIG)
        {
            size = 25;
        }
        else if (mapSize == MapSize.MEDIUM)
        {
            size = 15;
        }
        else
        {
            size = 7;
        }

        if (mapShape == MapShape.HEXAGON)
        {
            initHexagonMap();
        }
        else if (mapShape == MapShape.SQUARE)
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
            MapCell.Clear();
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
        for (int z = -size; z < size; z++)
        {
            for (int x = -size; x < size; x++)
            {
                int y = -x - z;
                if (Mathf.Abs(y) <= size)
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

                    if (terrain.GetComponentInChildren<Text>() != null)
                    {
                        terrain.GetComponentInChildren<Text>().text = "(" + z + "," + x + ")";
                    }
                }

            }
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)mapSize);
        writer.Write((byte)mapShape);
        var enumerator = MapCell.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.Save(writer);
        }
    }

    public void Load(BinaryReader reader)
    {
        CreateMap((MapShape)reader.ReadByte(), (MapSize)reader.ReadByte());
        Debugger.printLog((MapShape)reader.ReadByte() + "::" + (MapSize)reader.ReadByte());
        var enumerator = MapCell.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.Load(reader);
        }
    }

    /*
    public void calculateArea(EventEntitySelected e)
    {
        MapBox originCell;
        MapCell.TryGetValue(e.UnitMapbox.cell, out originCell);
        if (originCell != null)
        {
            List<Vector3> areaToCheck = HexagonCell.hexRange(this, vision);
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
                if (originCell.terrainType == TerrainType.SIMPLE)
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
                if (originCell.terrainType == TerrainType.SIMPLE)
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
    */
}
