using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexagonCell
{

    static Vector3[] Directions = new Vector3[] { new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1), new Vector3(-1, 1, 0),
        new Vector3(-1, 0, 1), new Vector3(0, -1, 1)};

    static Vector3[] Diagonals = new Vector3[] { new Vector3(2, -1, -1), new Vector3(1, 1, -2), new Vector3(-1, 2, -1), new Vector3(-2, 1, 1),
        new Vector3(-1, -1, 2), new Vector3(1, -2, 1)};

    public static float hexFloatLerp(float a, float b, float t)
    {
        return (a + (b - a) * t);
    }

    public static Vector3 hexLerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(
            hexFloatLerp(a.x, b.x, t),
            hexFloatLerp(a.y, b.y, t),
            hexFloatLerp(a.z, b.z, t)
            );
    }

    public static Vector3 cubeRound(Vector3 hex)
    {
        float x = Mathf.Round(hex.x);
        float y = Mathf.Round(hex.y);
        float z = Mathf.Round(hex.z);

        float x_diff = Mathf.Abs(x - hex.x);
        float y_diff = Mathf.Abs(y - hex.y);
        float z_diff = Mathf.Abs(z - hex.z);

        if (x_diff > y_diff && x_diff > z_diff)
        {
            x = -y - z;
        }
        else if (y_diff > z_diff)
        {
            y = -x - z;
        }
        else
        {
            z = -x - y;
        }
        return new Vector3(x, y, z);
    }

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
        float y = -x - hex.y;
        return (new Vector3(x, y, z));
    }

    public static Vector2 cubeToOddr(Vector3 cube)
    {
        float col = cube.x + (cube.z - (Mathf.Abs(cube.z) % 2)) / 2;
        float row = cube.z;
        return new Vector2(col, row);
    }

    public static Vector3 oddrToCube(Vector2 hex)
    {
        float x = hex.x - (hex.y - (Mathf.Abs(hex.y) % 2)) / 2;
        float z = hex.y;
        float y = -x - z;
        return new Vector3(x, y, z);
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

    public static List<Vector3> hexLineOfSight(Vector3 a, Vector3 b)
    {
        Vector3 epsionHex = new Vector3(Mathf.Epsilon, Mathf.Epsilon, Mathf.Epsilon);
        Vector3 newA = hexAdd(a, epsionHex);
        Vector3 newB = hexAdd(b, epsionHex);
        float N = hexDistance(newA, newB);
        List<Vector3> vision = new List<Vector3>();
        for (int i = 0; i <= N; i++)
        {
            vision.Add(cubeRound(hexLerp(newA, newB, (1 / N) * i)));
        }
        return vision;
    }
}
