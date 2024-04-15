using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;

    [SerializeField]
    private TileBase floorTile;

    [SerializeField]
    private RuleTile wallRule;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, wallRule);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    public void PaintWalls(IEnumerable<Vector2Int> wallPositions)
    {
        PaintTiles(wallPositions, floorTilemap, wallRule);
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        GameObject floor = GameObject.Find("Floor");
        // kill all children
        foreach (Transform child in floor.transform)
        {
            Destroy(child.gameObject);
        }
        floorTilemap.ClearAllTiles();
    }

    public TileBase GetTile(Vector2Int position)
    {
        return floorTilemap.GetTile((Vector3Int)position);
    }
}
