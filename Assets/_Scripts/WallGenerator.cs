using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardnialDirectionList);
        tilemapVisualizer.PaintWalls(basicWallPositions);
    }

    public static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var floorPosition in floorPositions)
        {
            foreach (var direction in directions)
            {
                var adjacentPosition = floorPosition + direction;
                if (!floorPositions.Contains(adjacentPosition))
                {
                    wallPositions.Add(adjacentPosition);
                }
            }
        }
        return wallPositions;
    }
}
