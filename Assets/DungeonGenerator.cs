using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tile floorTile;
    public Tile wallTile;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public int minRoomSize = 3;
    public int maxRoomSize = 8;
    public int numRooms = 10;
    public int maxAttempts = 1000;

    List<Rect> rooms = new List<Rect>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        int attempts = 0;
        while (rooms.Count < numRooms && attempts < maxAttempts)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);
            int roomX = Random.Range(0, floorTilemap.size.x - roomWidth);
            int roomY = Random.Range(0, floorTilemap.size.y - roomHeight);

            Rect room = new Rect(roomX, roomY, roomWidth, roomHeight);

            if (IsRoomValid(room))
            {
                rooms.Add(room);
            }

            attempts++;
        }

        foreach (Rect room in rooms)
        {
            CreateRoom(room);
        }

        GenerateCorridors();

        PlacePlayer();
        PlaceEnemies();
    }

    bool IsRoomValid(Rect room)
    {
        foreach (Rect existingRoom in rooms)
        {
            if (existingRoom.Overlaps(room))
            {
                return false;
            }
        }

        return true;
    }

    void CreateRoom(Rect room)
    {
        for (int x = (int)room.x; x < room.xMax; x++)
        {
            for (int y = (int)room.y; y < room.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                floorTilemap.SetTile(position, floorTile);
                // Wall creation
                if (x == (int)room.x || y == (int)room.y || x == (int)room.xMax - 1 || y == (int)room.yMax - 1)
                {
                    bool hasAdjacentCorridor = HasAdjacentCorridor(position);
                    if (!hasAdjacentCorridor)
                    {
                        wallTilemap.SetTile(position, wallTile);
                    }
                }
            }
        }
    }

    bool HasAdjacentCorridor(Vector3Int position)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int adjacentPosition = new Vector3Int(position.x + x, position.y + y, position.z);
                if (floorTilemap.GetTile(adjacentPosition) == floorTile)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void GenerateCorridors()
    {
        List<Rect> connectedRooms = new List<Rect>();
        List<(Rect, Rect)> edges = new List<(Rect, Rect)>();

        connectedRooms.Add(rooms[0]);

        while (connectedRooms.Count < rooms.Count)
        {
            float minDistance = float.PositiveInfinity;
            Rect closestRoomA = new Rect();
            Rect closestRoomB = new Rect();

            foreach (var roomA in connectedRooms)
            {
                foreach (var roomB in rooms)
                {
                    if (roomA == roomB || connectedRooms.Contains(roomB)) continue;

                    float distance = DistanceBetweenRooms(roomA, roomB);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestRoomA = roomA;
                        closestRoomB = roomB;
                    }
                }
            }

            connectedRooms.Add(closestRoomB);
            edges.Add((closestRoomA, closestRoomB));
        }

        foreach (var edge in edges)
        {
            DrawCorridorBetweenRooms(edge.Item1, edge.Item2);
        }
    }

    float DistanceBetweenRooms(Rect roomA, Rect roomB)
    {
        return Vector2.Distance(roomA.center, roomB.center);
    }

    void DrawCorridorBetweenRooms(Rect roomA, Rect roomB)
    {
        Vector2 roomACenter = roomA.center;
        Vector2 roomBCenter = roomB.center;

        int xStart = Mathf.Min((int)roomACenter.x, (int)roomBCenter.x);
        int xEnd = Mathf.Max((int)roomACenter.x, (int)roomBCenter.x);
        for (int x = xStart; x <= xEnd; x++)
        {
            floorTilemap.SetTile(new Vector3Int(x, (int)roomACenter.y, 0), floorTile);
        }

        int yStart = Mathf.Min((int)roomACenter.y, (int)roomBCenter.y);
        int yEnd = Mathf.Max((int)roomACenter.y, (int)roomBCenter.y);
        for (int y = yStart; y <= yEnd; y++)
        {
            floorTilemap.SetTile(new Vector3Int((int)roomBCenter.x, y, 0), floorTile);
        }
    }

    void PlacePlayer()
    {
        Rect room = rooms[Random.Range(0, rooms.Count)];
        Vector3Int position = new Vector3Int((int)room.center.x, (int)room.center.y, 0);
        Instantiate(playerPrefab, floorTilemap.GetCellCenterWorld(position), Quaternion.identity);
    }

    void PlaceEnemies()
    {
        for (int i = 0; i < numRooms / 2; i++)
        {
            Rect room = rooms[Random.Range(0, rooms.Count)];
            Vector3Int position = new Vector3Int((int)room.center.x, (int)room.center.y, 0);
            Instantiate(enemyPrefab, floorTilemap.GetCellCenterWorld(position), Quaternion.identity);
        }
    }
}