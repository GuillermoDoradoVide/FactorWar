using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class MapBox : MonoBehaviour {
    public enum TerrainType {SIMPLE, BLOCKED , OBSTACLE };
    [Header("Terrain Type")]
    public TerrainType terrainType;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private MapBoxCoordinates mapBoxCoordinates;
    public bool occupied;
    public Entity entity;
    public Vector3 cell;

    public int elevation;

    //provisional
    public Map map;

    public Material[] greenMaterial;
    public Material[] redMaterial;

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
        entity = newEntity;
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

    public Mesh getMesh()
    {
        return mesh;
    }

    public Material[] getMaterials()
    {
        return materials;
    }

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
        writer.Write(occupied);
        
    }

    public void Load(BinaryReader reader)
    {
        terrainType = (TerrainType)reader.ReadByte();
        occupied = reader.ReadBoolean();
    }
}
