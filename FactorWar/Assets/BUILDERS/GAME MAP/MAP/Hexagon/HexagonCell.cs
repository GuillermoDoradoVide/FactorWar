﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCell : MonoBehaviour {

    private Hex _hexCell;

    private Vector3 _position = Vector3.zero;
    public Vector3 position { get { return _position; } }

    private Vector3 _cube = Vector3.zero;
    public Vector3 cube { get { return _cube; } }

    private GameObject _cell = null;

    public float gCost = 0.0f;
    public float hCost = 0.0f;
    public float fCost
    {
        get { return gCost + hCost; }
    }

    public void InitHexagonCell(Vector3 cube, Vector3 position)
    {
        _cube = cube;
        _position = position;

        _hexCell = new Hex((int)_cube.x, (int)_cube.y, (int)_cube.z);

        // generate and create the 3D cell model.
    }

    public void Show()
    {

    }

    public void Hide()
    {

    }



}
