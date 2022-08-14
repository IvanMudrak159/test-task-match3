using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    private Tile[,] _tiles;
    [SerializeField] private int width, height;
    [SerializeField] private Sprite[] iconSprites;

    [SerializeField] private Text scoreText;
    private int _scoreCount;
    
    public delegate void GameStartedEvent(int width, int height, int iconLength);
    public event GameStartedEvent GameStarted;

    private void Start()
    {
        GameStarted?.Invoke(width, height, iconSprites.Length);
    }

    public void SetTilesPosition(Tile[,] tiles)
    {
        _tiles = tiles;
        height = _tiles.GetLength(0);
        width = _tiles.GetLength(1);

        float startX = transform.position.x;
        float startY = transform.position.y;
        Vector2 offset = _tiles[0,0].tile.bounds.size;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _tiles[y, x].transform.position = new Vector3(startX + offset.x * x, startY + offset.y * y, 0);
                _tiles[y, x].transform.parent = transform;
            }
        }
    }
    
    public void SetTilesSprite(int[,] field)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _tiles[y, x].icon.sprite = iconSprites[field[y, x]];
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
        _tiles[firstPos.y, firstPos.x] = _tiles[secondPos.y, secondPos.x];
        _tiles[secondPos.y, secondPos.x] = tempTile;
    }

    public void DestroyTiles(List<Vector2Int> tilesToDestroy)
    {
        foreach (var tile in tilesToDestroy)
        {
            if (_tiles[tile.y, tile.x] == null) continue;
            
            Destroy(_tiles[tile.y, tile.x].gameObject);
            _tiles[tile.y, tile.x] = null;
        }
    }

    public void UpdateScore(List<Vector2Int> matchingTiles)
    {
        _scoreCount += matchingTiles.Count * 100;
        scoreText.text = "Score: " + _scoreCount;
    }
}
