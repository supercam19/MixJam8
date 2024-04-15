using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    private static HashSet<Vector2Int> pathOrigins = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        AddArea(startPos, 2, path);
        var prevPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomCardinalDirection() * 3;
            pathOrigins.Add(newPos);
            AddArea(newPos, 2, path);
            prevPos = newPos;
        }
        return path;
    }

    private static void AddArea(Vector2Int position, int size, HashSet<Vector2Int> area)
    {
        for (int x = position.x - size; x < position.x + size; x++)
        {
            for (int y = position.y - size; y < position.y + size; y++)
            {
                area.Add(new Vector2Int(x, y));
            }
        }
    }

    public static HashSet<Vector2Int> GetPathOrigins()
    {
        return pathOrigins;
    }

    public static void ClearOrigins()
    {
        pathOrigins.Clear();
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardnialDirectionList = new List<Vector2Int>()
    {
        new Vector2Int(0,1), // Up
        new Vector2Int(1, 0), // Right
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0) // Left
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardnialDirectionList[Random.Range(0, cardnialDirectionList.Count)];
    }
}
