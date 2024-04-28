using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{

    [SerializeField]
    private Tilemap floorTileMap, wallTileMap;

    [SerializeField]
    private TileBase floorTile, corridorTile, wallLeftTile;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTileMap, floorTile);
    }
    public void PaintCorridorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTileMap, corridorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingeTile(tilemap, tile, position);
        }
    }

    private void PaintSingeTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {

        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    internal void PaintSingleCornerWall(Vector2Int position, string neighboursBinaryType)
    {
        // PaintTiles()
    }

    internal void PaintSingleBasicWall(Vector2Int position, string neighboursBinaryType)
    {
        PaintSingeTile(wallTileMap, wallLeftTile, position);
    }

    // Start is called before the first frame update
}
