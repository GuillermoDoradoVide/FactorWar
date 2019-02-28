using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAlgorithms : MonoBehaviour {

    private static Dictionary<string, Dictionary<Vector3, HexagonCell>> _hexContainers = new Dictionary<string, Dictionary<Vector3, HexagonCell>>();

    private static Vector3[] _cubeDirections = new Vector3[]
    {
        new Vector3(1, -1, 0), new Vector3(1, 0, -1), new Vector3(0, 1, -1),
        new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1)
    };

    private static Vector3[] _cubeDiagonalDirections = new Vector3[]
    {
        new Vector3(2, -1, -1), new Vector3(1, 1, -2), new Vector3(-1, 2, -1),
        new Vector3(-2, 1, 1), new Vector3(-1, -1, 2), new Vector3(1, -2, 1)
    };

    private static float _gameScale = 1.0f;
    private static float _hexRadius = 1.0f;

    private static float _hexWidth = 0.0f;
    private static float _hexHeight = 0.0f;

    private static float _hexSpacingVertical = 0.0f;
    private static float _hexSpacingHorizontal = 0.0f;

    private void Awake()
    {
        CalculateHexagonCellDimensions();
    }

    private static void CalculateHexagonCellDimensions()
    {
        _hexRadius = _hexRadius * _gameScale;

        _hexWidth = _hexRadius * 2;
        _hexSpacingHorizontal = _hexWidth * 0.75f;

        _hexHeight = (Mathf.Sqrt(3) / 2.0f) * _hexWidth;
        _hexSpacingVertical = _hexHeight / 2.0f;
    }

    /// <summary>
    ///  Converts a cube HexagonCell to an axial HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector2 CubeToAxial(Vector3 cube)
    {
        return (new Vector2(cube.x, cube.z));
    }
    /// <summary>
    ///  Converts an axial HexagonCell to a cube HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector3 AxialToCube(Vector2 axial)
    {
        return (new Vector3(axial.x, -axial.x - axial.y, axial.y));
    }

    /// <summary>
    ///  Converts an axial HexagonCell to a world transform position
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector3 AxialToWorldPosition(Vector2 axial)
    {
        float x = axial.x * _hexSpacingHorizontal;
        float z = -((axial.x * _hexSpacingVertical) + (axial.y * _hexSpacingVertical * 2.0f));

        return new Vector3(x, 0.0f, z);
    }
    /// <summary>
    ///  Converts a cube HexagonCell to a world transform position
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector3 CubeToWorldPosition(Vector3 cube)
    {
        float x = cube.x * _hexSpacingHorizontal;
        float y = 0.0f;
        float z = -((cube.x * _hexSpacingVertical) + (cube.z * _hexSpacingVertical * 2.0f));

        return new Vector3(x, y, z);
    }
    /// <summary>
    /// Converts a world transform position to the nearest axial HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector2 WorldPositionToAxial(Vector3 wPos)
    {
        float q = (wPos.x * (2.0f / 3.0f)) / _hexRadius;
        float r = ((-wPos.x / 3.0f) + ((Mathf.Sqrt(3) / 3.0f) * wPos.z)) / _hexRadius;
        return RoundAxial(new Vector2(q, r));
    }
    /// <summary>
    ///  Converts a world transform position to the nearest cube HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector3 WorldPositionToCube(Vector3 wPos)
    {
        return AxialToCube(WorldPositionToAxial(wPos));
    }
    /// <summary>
    ///  Rounds a given Vector2 to the nearest axial HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector2 RoundAxial(Vector2 axial)
    {
        return RoundCube(AxialToCube(axial));
    }
    /// <summary>
    ///  Rounds a given Vector3 to the nearest cube HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector3 RoundCube(Vector3 coord)
    {
        float rx = Mathf.Round(coord.x);
        float ry = Mathf.Round(coord.y);
        float rz = Mathf.Round(coord.z);

        float x_diff = Mathf.Abs(rx - coord.x);
        float y_diff = Mathf.Abs(ry - coord.y);
        float z_diff = Mathf.Abs(rz - coord.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector3(rx, ry, rz);
    }

    /// <summary>
    /// // Return a cube vector for a given direction index
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetCubeDirection(int direction)
    {
        return _cubeDirections[direction];
    }

    /// <summary>
    /// // Returns an cube diagonal vector for a given direction index
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetCubeDiagonalDirection(int direction)
    {
        return _cubeDiagonalDirections[direction];
    }

    /// <summary>
    /// // Returns the neighboring cube HexagonCell for a given direction index at a HexagonCell distance of 1
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetNeighborCube(Vector3 cube, int direction)
    {
        return GetNeighborCube(cube, direction, 1);
    }

    /// <summary>
    /// // Returns the neighboring cube HexagonCell for a given direction index at a given HexagonCell distance
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static Vector3 GetNeighborCube(Vector3 cube, int direction, int distance)
    {
        return cube + (GetCubeDirection(direction) * (float)distance);
    }
    /// <summary>
    /// // Returns all neighboring cube HexagonCells at a HexagonCell distance of 1
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static List<Vector3> GetNeighborCubes(Vector3 cube)
    {
        return GetNeighborCubes(cube, 1);
    }

    /// <summary>
    /// // Returns all neighboring cube HexagonCells inclusively up to a given HexagonCell distance
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="radius"></param>
    /// <param name="cleanResults"></param>
    /// <returns></returns>
    public static List<Vector3> GetNeighborCubes(Vector3 cube, int radius, bool cleanResults = true)
    {
        List<Vector3> cubes = new List<Vector3>();

        for (int x = (int)(cube.x - radius); x <= (int)(cube.x + radius); x++)
            for (int y = (int)(cube.y - radius); y <= (int)(cube.y + radius); y++)
                for (int z = (int)(cube.z - radius); z <= (int)(cube.z + radius); z++)
                    if ((x + y + z) == 0)
                        cubes.Add(new Vector3(x, y, z));

        cubes.Remove(cube);

        if (cleanResults)
            return CleanCubeResults(cubes);
        return cubes;
    }
    /// <summary>
    /// // Returns a neighboring diagonal cube HexagonCell for a given direction index at a HexagonCell distance of 1
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetDiagonalNeighborCube(Vector3 cube, int direction)
    {
        return cube + GetCubeDiagonalDirection(direction);
    }

    /// <summary>
    /// // Returns a neighboring diagonal cube HexagonCell for a given direction index at a given HexagonCell distance
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static Vector3 GetDiagonalNeighborCube(Vector3 cube, int direction, int distance)
    {
        return cube + (GetCubeDiagonalDirection(direction) * (float)distance);
    }

    /// <summary>
    /// // Returns all neighboring diagonal cube HexagonCells at a HexagonCell distance of 1
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static List<Vector3> GetDiagonalNeighborCubes(Vector3 cube)
    {
        return GetDiagonalNeighborCubes(cube, 1);
    }

    /// <summary>
    /// // Returns all neighboring diagonal cube HexagonCells at a given HexagonCell distance
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="distance"></param>
    /// <param name="cleanResults"></param>
    /// <returns></returns>
    public static List<Vector3> GetDiagonalNeighborCubes(Vector3 cube, int distance, bool cleanResults = true)
    {
        List<Vector3> cubes = new List<Vector3>();
        for (int i = 0; i < 6; i++)
            cubes.Add(GetDiagonalNeighborCube(cube, i, distance));
        if (cleanResults)
            return CleanCubeResults(cubes);
        return cubes;
    }

    /// <summary>
    /// // Boolean combines two lists of cube HexagonCells
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static List<Vector3> BooleanCombineCubes(List<Vector3> a, List<Vector3> b)
    {
        List<Vector3> vec = a;
        foreach (Vector3 vb in b)
            if (!a.Contains(vb))
                a.Add(vb);
        return vec;
    }

    /// <summary>
    /// // Boolean differences two lists of cube HexagonCells
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static List<Vector3> BooleanDifferenceCubes(List<Vector3> a, List<Vector3> b)
    {
        List<Vector3> vec = a;
        foreach (Vector3 vb in b)
            if (a.Contains(vb))
                a.Remove(vb);
        return vec;
    }

    /// <summary>
    /// // Boolean intersects two lists of cube HexagonCells
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static List<Vector3> BooleanIntersectionCubes(List<Vector3> a, List<Vector3> b)
    {
        List<Vector3> vec = new List<Vector3>();
        foreach (Vector3 va in a)
            foreach (Vector3 vb in b)
                if (va == vb)
                    vec.Add(va);
        return vec;
    }

    /// <summary>
    /// // Boolean excludes two lists of cube HexagonCells
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static List<Vector3> BooleanExclusionCubes(List<Vector3> a, List<Vector3> b)
    {
        List<Vector3> vec = new List<Vector3>();
        foreach (Vector3 va in a)
            foreach (Vector3 vb in b)
                if (va != vb)
                {
                    vec.Add(va);
                    vec.Add(vb);
                }
        return vec;
    }

    /// <summary>
    /// // Rotates a cube HexagonCell right by one HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector4 RotateCubeHexagonCellsRight(Vector3 cube)
    {
        return new Vector3(-cube.z, -cube.x, -cube.y);
    }

    /// <summary>
    /// // Rotates a cube HexagonCell left by one HexagonCell
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector4 RotateCubeHexagonCellsLeft(Vector3 cube)
    {
        return new Vector3(-cube.y, -cube.z, -cube.x);
    }

    /// <summary>
    /// // Calculates the distance between two cube HexagonCells
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float GetDistanceBetweenTwoCubes(Vector3 a, Vector3 b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    /// <summary>
    /// // Calculates the lerp interpolation between two cube HexagonCells given a value between 0.0f and 1.0f
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 GetLerpBetweenTwoCubes(Vector3 a, Vector3 b, float t)
    {
        Vector3 cube = new Vector3(
            a.x + (b.x - a.x) * t,
            a.y + (b.y - a.y) * t,
            a.z + (b.z - a.z) * t
        );

        return cube;
    }

    /// <summary>
    /// // Returns the cube HexagonCell a given distance interval between two cube HexagonCells
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static Vector3 GetPointOnLineBetweenTwoCubes(Vector3 a, Vector3 b, int distance)
    {
        float cubeDistance = GetDistanceBetweenTwoCubes(a, b);
        return RoundCube(GetLerpBetweenTwoCubes(a, b, ((1.0f / cubeDistance) * distance)));
    }

    /// <summary>
    ///  // Returns a list representing the line of cube HexagonCells between two given cube HexagonCells (inclusive)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="cleanResults"></param>
    /// <returns></returns>
    public static List<Vector3> GetLineBetweenTwoCubes(Vector3 a, Vector3 b, bool cleanResults = true)
    {
        List<Vector3> cubes = new List<Vector3>();
        float cubeDistance = GetDistanceBetweenTwoCubes(a, b);
        for (int i = 0; i <= cubeDistance; i++)
            cubes.Add(RoundCube(GetLerpBetweenTwoCubes(a, b, ((1.0f / cubeDistance) * i))));
        cubes.Add(a);
        if (cleanResults)
            return CleanCubeResults(cubes);
        return cubes;
    }

    /// <summary>
    /// // Returns a list of validated neighboring cube HexagonCells at a evaluation distance of 1
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="cleanResults"></param>
    /// <returns></returns>
    public static List<Vector3> GetReachableCubes(Vector3 cube, bool cleanResults = true)
    {
        List<Vector3> cubes = new List<Vector3>();

        HexagonCell originHexagonCell = GetHexagonCellFromContainer(cube, "all");
        HexagonCell currentHexagonCell = null;
        Vector3 currentCube = cube;

        for (int i = 0; i < 6; i++)
        {
            currentCube = GetNeighborCube(cube, i);
            currentHexagonCell = GetHexagonCellFromContainer(currentCube, "all");

            if (currentHexagonCell != null)
                cubes.Add(currentCube);
        }

        if (cleanResults)
            return CleanCubeResults(cubes);
        return cubes;
    }

    /// <summary>
    /// // Returns a list of validated neighboring cube HexagonCells at a given evaluation radius
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="radius"></param>
    /// <param name="cleanResults"></param>
    /// <returns></returns>
    public static List<Vector3> GetReachableCubes(Vector3 cube, int radius, bool cleanResults = true)
    {
        if (radius == 1)
            return GetReachableCubes(cube);

        List<Vector3> visited = new List<Vector3>();
        visited.Add(cube);

        List<List<Vector3>> fringes = new List<List<Vector3>>();
        fringes.Add(new List<Vector3>());
        fringes[0].Add(cube);

        for (int i = 1; i <= radius; i++)
        {
            fringes.Add(new List<Vector3>());
            foreach (Vector3 v in fringes[i - 1])
            {
                foreach (Vector3 n in GetNeighborCubes(v))
                {
                    if (!visited.Contains(n))
                    {
                        visited.Add(n);
                        fringes[i].Add(n);
                    }
                }
            }
        }

        if (cleanResults)
            return CleanCubeResults(visited);
        return visited;
    }

    /// <summary>
    /// // Returns an ordered list of cube HexagonCells following a spiral pattern around a given cube HexagonCell at a given HexagonCell distance
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="radius"></param>
    /// <param name="cleanResutls"></param>
    /// <returns></returns>
    public static List<Vector3> GetSpiralCubes(Vector3 cube, int radius, bool cleanResutls = true)
    {
        List<Vector3> vec = new List<Vector3>();
        Vector4 current = cube + GetCubeDirection(4) * (float)radius;

        for (int i = 0; i < 6; i++)
            for (int j = 0; j < radius; j++)
            {
                vec.Add(current);
                current = GetNeighborCube(current, i);
            }
        if (cleanResutls)
            return CleanCubeResults(vec);
        return vec;
    }

    /// <summary>
    /// // Returns an ordered list of cube HexagonCells following a spiral pattern around a given cube HexagonCell up to a given HexagonCell distance (inclusive)
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static List<Vector3> GetMultiSpiralCubes(Vector3 cube, int radius)
    {
        List<Vector3> cubes = new List<Vector3>();
        cubes.Add(cube);
        for (int i = 0; i <= radius; i++)
            foreach (Vector4 r in GetSpiralCubes(cube, i))
                cubes.Add(r);
        return cubes;
    }

    /// <summary>
    ///  // Returns an ordered list of cube HexagonCells following the A* path results between two given cube HexagonCells
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static List<Vector3> GetPathBetweenTwoCubes(Vector3 origin, Vector3 target, string container = "all")
    {
        if (origin == target)
            return new List<Vector3>();

        List<Vector3> openSet = new List<Vector3>();
        List<Vector3> closedSet = new List<Vector3>();
        openSet.Add(origin);

        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
        cameFrom.Add(origin, Vector3.zero);

        Vector3 current = Vector3.zero;
        HexagonCell HexagonCell = null;
        HexagonCell currentHexagonCell = null;
        HexagonCell neighborHexagonCell = null;
        float newCost = 0.0f;

        while (openSet.Count > 0)
        {
            current = openSet[0];
            currentHexagonCell = GetHexagonCellFromContainer(current, container);

            for (int i = 1; i < openSet.Count; i++)
            {
                HexagonCell = GetHexagonCellFromContainer(openSet[i], container);
                if (HexagonCell.fCost < currentHexagonCell.fCost || HexagonCell.fCost == currentHexagonCell.fCost && HexagonCell.hCost < currentHexagonCell.hCost)
                {
                    current = openSet[i];
                    currentHexagonCell = GetHexagonCellFromContainer(current, container);
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == target)
                break;

            List<Vector3> neighbors = new List<Vector3>();
            neighbors = GetReachableCubes(current);

            foreach (Vector3 neighbor in neighbors)
            {
                HexagonCell = GetHexagonCellFromContainer(neighbor, container);
                if (HexagonCell == null || closedSet.Contains(neighbor))
                    continue;

                newCost = currentHexagonCell.gCost + GetDistanceBetweenTwoCubes(current, neighbor);
                neighborHexagonCell = GetHexagonCellFromContainer(neighbor, container);

                if (newCost < neighborHexagonCell.gCost || !openSet.Contains(neighbor))
                {
                    neighborHexagonCell.gCost = newCost;
                    neighborHexagonCell.hCost = GetDistanceBetweenTwoCubes(current, neighbor);
                    cameFrom.Add(neighbor, current);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        List<Vector3> path = new List<Vector3>();

        current = target;
        path.Add(target);

        while (current != origin)
        {
            cameFrom.TryGetValue(current, out current);
            path.Add(current);
        }

        path.Reverse();

        return path;
    }

    /// <summary>
    /// // Validates all cube HexagonCell results against instantiated HexagonCell GameObjects
    /// </summary>
    /// <param name="cubes"></param>
    /// <returns></returns>
    public static List<Vector3> CleanCubeResults(List<Vector3> cubes)
    {
        List<Vector3> r = new List<Vector3>();
        foreach (Vector3 cube in cubes)
            if (GethexContainer("all").ContainsKey(cube))
                r.Add(cube);
        return r;
    }

    /// <summary>
    /// // Returns a HexagonCell container given a container key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private static Dictionary<Vector3, HexagonCell> GethexContainer(string key)
    {
        Dictionary<Vector3, HexagonCell> hexContainer;
        if (!_hexContainers.TryGetValue(key, out hexContainer))
        {
            _hexContainers.Add(key, new Dictionary<Vector3, HexagonCell>());
            _hexContainers.TryGetValue(key, out hexContainer);
        }
        return hexContainer;
    }

    /// <summary>
    /// // Removes empty HexagonCell containers
    /// </summary>
    private static void CleanEmptyhexContainers()
    {
        List<string> hexContainerKeysToRemove = new List<string>();
        Dictionary<Vector3, HexagonCell> hexContainer;
        foreach (string key in _hexContainers.Keys)
        {
            _hexContainers.TryGetValue(key, out hexContainer);
            if (hexContainer.Values.Count == 0)
                hexContainerKeysToRemove.Add(key);
        }

        foreach (string key in hexContainerKeysToRemove)
            _hexContainers.Remove(key);
    }

    /// <summary>
    /// // Returns a HexagonCell given a cube HexagonCell and a container key
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static HexagonCell GetHexagonCellFromContainer(Vector3 cube, string key)
    {
        HexagonCell HexagonCell = null;
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        if (cube == Vector3.zero)
            hexContainer.TryGetValue(Vector3.zero, out HexagonCell);
        else
            hexContainer.TryGetValue(cube, out HexagonCell);
        return HexagonCell;
    }

    /// <summary>
    ///  // Returns a list of HexagonCells given a container key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static List<HexagonCell> GetHexagonCellsFromContainer(string key)
    {
        List<HexagonCell> HexagonCells = new List<HexagonCell>();
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        foreach (KeyValuePair<Vector3, HexagonCell> entry in hexContainer)
            HexagonCells.Add(entry.Value);
        return HexagonCells;
    }

    /// <summary>
    /// // Returns a list of cube HexagonCells given a container key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static List<Vector3> GetCubesFromContainer(string key)
    {
        List<Vector3> cubes = new List<Vector3>();
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        foreach (Vector3 cube in hexContainer.Keys)
            cubes.Add(cube);
        return cubes;
    }

    /// <summary>
    ///  // Adds a given cube HexagonCell to the given container key
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="key"></param>
    public static void AddCubeToContainer(Vector3 cube, string key)
    {
        AddHexagonCellToContainer(GetHexagonCellFromContainer(cube, "all"), key);
    }

    /// <summary>
    /// // Adds a list of cube HexagonCells to the given container key
    /// </summary>
    /// <param name="cubes"></param>
    /// <param name="key"></param>
    public static void AddCubesToContainer(List<Vector3> cubes, string key)
    {
        foreach (Vector3 cube in cubes)
            AddHexagonCellToContainer(GetHexagonCellFromContainer(cube, "all"), key);
    }

    /// <summary>
    /// // Adds a given HexagonCell to the given container key
    /// </summary>
    /// <param name="HexagonCell"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool AddHexagonCellToContainer(HexagonCell HexagonCell, string key)
    {
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        if (!hexContainer.ContainsKey(HexagonCell.cube))
        {
            hexContainer.Add(HexagonCell.cube, HexagonCell);
            return true;
        }
        return false;
    }

    /// <summary>
    /// // Removes a given HexagonCell from the given container key
    /// </summary>
    /// <param name="HexagonCell"></param>
    /// <param name="key"></param>
    public static void RemoveHexagonCellFromContainer(HexagonCell HexagonCell, string key)
    {
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        if (hexContainer.ContainsKey(HexagonCell.cube))
            hexContainer.Remove(HexagonCell.cube);
    }

    /// <summary>
    /// // Removes all HexagonCells from given container key
    /// </summary>
    /// <param name="key"></param>
    public static void RemoveAllHexagonCellsInContainer(string key)
    {
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        hexContainer.Clear();
    }

    /// <summary>
    /// // Removes a given HexagonCell from all containers
    /// </summary>
    /// <param name="HexagonCell"></param>
    public static void RemoveHexagonCellFromAllContainers(HexagonCell HexagonCell)
    {
        foreach (string key in _hexContainers.Keys)
            RemoveHexagonCellFromContainer(HexagonCell, key);
    }

    /// <summary>
    ///  // Clears all HexagonCell containers
    /// </summary>
    public static void ClearAllhexContainers()
    {
        _hexContainers.Clear();
    }

    /// <summary>
    /// // Clears all HexagonCells from a given container key
    /// </summary>
    /// <param name="key"></param>
    public static void ClearHexagonCellsFromContainer(string key)
    {
        Dictionary<Vector3, HexagonCell> hexContainer = GethexContainer(key);
        hexContainer.Clear();
    }

    /// <summary>
    /// // Hides all HexagonCells for a given container key
    /// </summary>
    /// <param name="key"></param>
    public static void HideHexagonCellsInContainer(string key)
    {
        foreach (HexagonCell HexagonCell in GetHexagonCellsFromContainer(key))
        {
            HexagonCell.Hide();
            RemoveHexagonCellFromContainer(HexagonCell, "visible");
        }
    }

    /// <summary>
    /// // Shows all HexagonCells for a given container key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="bCollider"></param>
    public static void ShowHexagonCellsInContainer(string key, bool bCollider = true)
    {
        foreach (HexagonCell HexagonCell in GetHexagonCellsFromContainer(key))
        {
            HexagonCell.Show();
            AddHexagonCellToContainer(HexagonCell, "visible");
        }
    }

    /// <summary>
    /// // Hides and Clears all HexagonCells for a given container key
    /// </summary>
    /// <param name="key"></param>
    public static void HideAndClearhexContainer(string key)
    {
        foreach (HexagonCell HexagonCell in GetHexagonCellsFromContainer(key))
            HexagonCell.Hide();
        ClearHexagonCellsFromContainer(key);
    }



}
