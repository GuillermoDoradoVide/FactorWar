using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexagonCell {

    static Vector3[] Directions = new Vector3[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0),
        new Vector3(-1, 0, 1), new Vector3(0, -1, 1)};

    static Vector3[] Diagonals = new Vector3[] { new Vector3(2, -1, -1), new Vector3(1, 1, -2), new Vector3(-1, 2, -1), new Vector3(-2, 1, 1),
        new Vector3(-1, -1, 2), new Vector3(1, -2, 1)};

    public static Vector2 cubeToAxial(Vector3 cube)
    {
        float q = cube.x;
        float r = cube.z;
        return (new Vector2(q, r));
    }

    public static Vector3 axialToCube(Vector2 hex)
    {
        float x = hex.x;
        float z = hex.y;
        float y = - x - z;
        return (new Vector3(x, y, z));
    }

    private static Vector3 hexAdd(Vector3 hex, Vector3 direction)
    {
        return (hex + direction);
    }

    public static Vector3 cubeDirection(int direction)
    {
        return Directions[direction];
    }

    public static Vector3 hexNeightborsDirections(Vector3 hex, int neightbor)
    {
        return hexAdd(hex, cubeDirection(neightbor));
    }

    public static Vector3 hexDiagonalNeighbor(Vector3 hex, int neighbor)
    {
        return hexAdd(hex, Diagonals[neighbor]);
    }

    public static float hexDistance(Vector3 a, Vector3 b)
    {
        return (Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z)));
    }

    public static List<Vector3> hexRange(MapBox hex, int N)
    {
        List<Vector3> results = new List<Vector3>();
        for (int i = -N; i <= N; i++)
        {
            for (int j = Mathf.Max(-N, -i - N); j <= Mathf.Min(N, -i + N); j++)
            {
                int k = -i - j;
                results.Add(hexAdd(hex.cell, new Vector3(i, j, k)));
            }
        }
        return results;
    }

    public static List<Vector3> hexReacheable(MapBox hex, int N)
    {
        List<Vector3> posibilities = new List<Vector3>();
        posibilities.Add(hex.cell);
        for (int i = 1; i < N; i++)
        {
            for (int j = 0; j < (N - 1); j++)
            {
                for (int dir = 0; dir <= 5; dir++)
                {
                    Vector3 neightbor = hexNeightborsDirections(hex.cell, dir);
                    
                    posibilities.Add(neightbor);
                }
            }
        }
        return posibilities;
    }

}
