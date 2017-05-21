using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewMapBox", menuName = "MAPS/MapBox", order = 2)]
public class MapBox : ScriptableObject {
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

    public void setPosition(float newX, float newY)
    {
        boxPosition.xPos = newX;
        boxPosition.yPos = newY;
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
}
