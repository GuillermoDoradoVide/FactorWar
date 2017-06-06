using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class MapBox : MonoBehaviour {
    [Header("Terrain Type")]
    public TerrainType terrainType;
    public int terrainCell;
    public bool occupied;
    public Vector3 cell;

    private MapBoxCoordinates mapBoxCoordinates;
    public Material[] greenMaterial;
    public Material[] redMaterial;

    public MapBoxPrefabs terrainPrefabs;

    //provisional
    //public Map map;

    // public Entity entity;

    // private Mesh mesh;
    // [SerializeField]
    // private Material[] materials;

    private void OnEnable()
    {
        EventManager.Instance.AddListener<EventEntitySelected>(calculateArea);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<EventEntitySelected>(calculateArea);
    }

    private void Awake()
    {
    }

    private void loadTerrainDetails()
    {
        if (terrainType == TerrainType.SIMPLE)
        {
            GameObject terrainDetail = Instantiate(terrainPrefabs.Simple[terrainCell]);
            terrainDetail.transform.SetParent(transform);
            terrainDetail.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            GameObject terrainDetail = Instantiate(terrainPrefabs.Blocked[terrainCell]);
            terrainDetail.transform.SetParent(transform);
            terrainDetail.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void setPosition(int newX, int newZ)
    {
        mapBoxCoordinates.xPos = newX;
        mapBoxCoordinates.zPos = newZ;
        cell.x = newX;
        cell.y = mapBoxCoordinates.yPos;
        cell.z = newZ;
    }

    public MapBoxCoordinates getPosition()
    {
        return mapBoxCoordinates;
    }

    public void setEntity(Entity newEntity)
    {
        // If the object can be placed in blocked terrain, then yes (INTERFACE)
        //entity = newEntity;
        setOccupiedTrue();
    }

    public void setOccupiedTrue()
    {
        occupied = true;
    }

    public void setOccupiedFalse()
    {
        occupied = false;
    }
    /*
    public Mesh getMesh()
    {
        return mesh;
    }

    public Material[] getMaterials()
    {
        return materials;
    }*/

    public Material[] changeColorRed()
    {
        return redMaterial;
    }

    public Material[] changeColorGreen()
    {
        return greenMaterial;
    }

    public void Save (BinaryWriter writer)
    {
        writer.Write((byte)terrainType);
        writer.Write((byte)terrainCell);
        writer.Write(occupied);
        
    }

    public void Load(BinaryReader reader)
    {
        terrainType = (TerrainType)reader.ReadByte();
        terrainCell = reader.ReadByte();
        occupied = reader.ReadBoolean();
        loadTerrainDetails();
        Debugger.printLog(terrainType + "::" + terrainCell);
    }

    public void calculateArea(EventEntitySelected e)
    {
        List<Vector3> areaToCheck = HexagonCell.hexRange(this, e.AttackVisionRange);
    }


}
