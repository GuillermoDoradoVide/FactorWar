using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MapBoxCoordinates
{
    public int xPos { get; set; }
    public int zPos { get; set; }
    public int yPos
    {
        get
        { return -xPos - zPos;
        }
    }

    public MapBoxCoordinates (int x, int z)
    {
        xPos = x;
        zPos = z;
    }

    public override string ToString()
    {
        return ("(" + xPos.ToString() + ", " + yPos.ToString() + ", " + zPos.ToString() + ")");
    }

    public string ToStringOnSeparateLines()
    {
        return (xPos.ToString() + "\n " + yPos.ToString() + "\n" + zPos.ToString());
    }


};
