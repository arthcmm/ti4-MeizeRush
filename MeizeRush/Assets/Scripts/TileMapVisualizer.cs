using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{

    [SerializeField]
    public Tilemap floorTileMap, wallTileMap;

    [SerializeField]
    private TileBase[] floorTile = new TileBase[8];
    public TileBase[] corridorTile = new TileBase[2];
    public TileBase[] wallLeftTile = new TileBase[1];

    public object Tilemap { get; internal set; }

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions, int size)
    {
        PaintTiles(floorPositions, floorTileMap, floorTile, size);
    }
    public void PaintCorridorTiles(IEnumerable<Vector2Int> floorPositions, int size)
    {
        PaintCorridorTiles(floorPositions, floorTileMap, corridorTile, size);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase[] tile, int size)
    {
        foreach (var position in positions)
        {
            PaintSingeTile(tilemap, tile, position, size);
        }
    }
    private void PaintCorridorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase[] tile, int pos)
    {
        Vector2Int posAnt = new Vector2Int(0, 0);
        foreach (var position in positions)
        {
            if (posAnt.x > position.x || posAnt.x < position.x)
            {
                PaintSingeCorridorTile(tilemap, tile, position, 0);
            }
            else
            {
                PaintSingeCorridorTile(tilemap, tile, position, 1);

            }
            posAnt = position;
        }
    }


    private void PaintSingeTile(Tilemap tilemap, TileBase[] tile, Vector2Int position, int size)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile[UnityEngine.Random.Range(0, 70 % (size + 1))]);
    }
    private void PaintSingeCorridorTile(Tilemap tilemap, TileBase[] tile, Vector2Int position, int pos)
    {

        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile[pos]);
    }

    internal void PaintSingleCornerWall(Vector2Int position, string neighboursBinaryType)
    {
        // PaintTiles()
    }

    internal void PaintSingleBasicWall(Vector2Int position, string neighboursBinaryType, int size)
    {
        PaintSingeTile(wallTileMap, wallLeftTile, position, size);
    }

    // Start is called before the first frame update
}
