using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBox : MonoBehaviour {

    private BoxType _boxType;
    private TerrainBox _terrainBox;
    public BoxType BoxType
    {
        get
        {
            return _boxType;
        }

        set
        {
            _boxType = value;
        }
    }

    public TerrainBox TerrainBox
    {
        get
        {
            return _terrainBox;
        }

        set
        {
            _terrainBox = value;
        }
    }


}
