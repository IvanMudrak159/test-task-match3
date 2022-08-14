using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    private Tile[,] _tiles;
    [SerializeField] private int width, height;
    [SerializeField] private Sprite[] iconSprites;
    
    public delegate void GameStartedEvent(int width, int height, int iconLength);
    public event GameStartedEvent GameStarted;

    private void Start()
    {
        GameStarted?.Invoke(width, height, iconSprites.Length);
    }

    public void SetTilesPosition(Tile[,] tiles)
    {
        _tiles = tiles;
        width = _tiles.GetLength(0);
        height = _tiles.GetLength(1);

        float startX = transform.position.x;
        float startY = transform.position.y;
        Vector2 offset = _tiles[0,0].tile.bounds.size;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tiles[x, y].transform.position = new Vector3(startX + offset.x * x, startY + offset.y * y, 0);
                _tiles[x, y].transform.parent = transform;
            }
        }
    }
    
    public void SetTilesSprite(int[,] field)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tiles[x, y].icon.sprite = iconSprites[field[x,y]];
            }
        }
    }

    public void ChangePosition(Tile firstTile, Tile secondTile)
    {
        Vector2 tempPos = firstTile.transform.position;
        firstTile.transform.position = secondTile.transform.position;
        secondTile.transform.position = tempPos;

        
        Tile tempTile = firstTile;
        Vector2Int firstPos = firstTile.Position;
        Vector2Int secondPos = secondTile.Position;
        _tiles[firstPos.x, firstPos.y] = _tiles[secondPos.x, secondPos.y];
        _tiles[secondPos.x, secondPos.y] = tempTile;
    }

    public void DestroyTiles(List<Vector2Int> tilesToDestroy)
    {
        foreach (var tile in tilesToDestroy)
        {
            Destroy(_tiles[tile.x, tile.y].gameObject);
            _tiles[tile.x, tile.y] = null;
        }
    }

    private void UpdateScore()
    {
        
    }
}
