using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMath : MonoBehaviour {

    public Vector2 center;
    public float size;
    private float width;
    private float horizDistance;
    Vector3[] Directions = new Vector3[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0),
        new Vector3(-1, 0, 1), new Vector3(0, -1, 1)};

    Vector3[] Diagonals = new Vector3[] { new Vector3(2, -1, -1), new Vector3(1, 1, -2), new Vector3(-1, 2, -1), new Vector3(-2, 1, 1),
        new Vector3(-1, -1, 2), new Vector3(1, -2, 1)};

    private void Start()
    {
        calculateHexagon();
    }

    private void calculateHexagon()
    {
        width = size * 2;
        horizDistance = width * 3 / 4;
    }


    public Vector2 hex_corner(int i)
    {
        float angleDEG = 60 * i + 30;
        float angleRAD = (Mathf.PI / 180) * angleDEG;

        return (    new Vector2(center.x + size * Mathf.Cos(angleRAD), center.y + size * Mathf.Sin(angleRAD))   );
    }

    public float getHexVerticalSize()
    {
        return width;
    }

    public float getHexWidthSize()
    {
        return (width * Mathf.Sqrt(3)/2);
    }

    public float getVerticalDistance()
    {
        return horizDistance;
    }

    public float getHorizontalDistance()
    {
        return (width * Mathf.Sqrt(3) / 2);
    }

    public Vector2 cubeToAxial(Vector3 cube)
    {
        float q = cube.x;
        float r = cube.z;
        return (new Vector2(q,r));
    }

    public Vector3 axialToCube(Vector2 hex)
    {
        float x = hex.x;
        float z = hex.y;
        float y = -x-z;
        return (new Vector3(x, y, z));
    }

    private Vector3 hexAdd(Vector3 hex, Vector3 direction)
    {
        return (hex + direction);
    }

    public Vector3 cubeDirection(int direction)
    {
        return Directions[direction];
    }

    public Vector3 cubeNeightborsDirections(Vector3 hex, int neightbor)
    {

        return hexAdd(hex, cubeDirection(neightbor));
    }

    public Vector3 hexDiagonalNeighbor(Vector3 hex, int neighbor)
    {
        return hexAdd(hex, Diagonals[neighbor]);
    }

    public float hexDistance(Vector3 a, Vector3 b)
    {
        return (Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z)));
    }

    public List<Vector3> hexRange(Vector3 hex, int N)
    {
        List<Vector3> results = new List<Vector3>();
        for (int i = -N; i <= N; i ++)
        {
            for (int j = Mathf.Max(-N, -i-N); j <= Mathf.Min(N, -i+N); j++)
            {
                int k = -i - j;
                results.Add(hexAdd(hex, new Vector3(i, j, k) ));
            }
        }
        return results;
    }

    public void hexReacheable(Vector3 hex, int N)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(hex);
        List<Vector3> posibilities = new List<Vector3>();
        posibilities.Add(hex);
        for (int i = 1; i < N; i++)
        {
            for(int j = 0; j < (N-1); j++)
            {
                for(int dir = 0; dir <= 6; dir ++)
                {
                    Vector3 neightbor = cubeNeightborsDirections(hex, dir);
                    //if neighbor not visited/blocked
                    posibilities.Add(neightbor);
                }
            }
        }
    }


}
