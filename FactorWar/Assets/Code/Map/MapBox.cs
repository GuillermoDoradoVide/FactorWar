using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private BoxPosition boxPosition;
    public bool occupied;
    public Entity entity;
    public Vector3 cell;

    public Material[] greenMaterial;
    public Material[] redMaterial;

    public void setPosition(float newX, float newY, float newZ)
    {
        boxPosition.xPos = newX;
        boxPosition.yPos = newY;
        boxPosition.zPos = newZ;
        cell.x = newX;
        cell.y = newY;
        cell.z = newZ;
    }

    public BoxPosition getPosition()
    {
        return boxPosition;
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
}
