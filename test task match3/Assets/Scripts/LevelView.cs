using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private SpriteRenderer backGroundImage;
    public Sprite[] iconSprites;
    private Tile[,] _tiles;

    public delegate void GameStarted(int width, int height, int iconLength);
    public event GameStarted OnGameStart;

    private void Start()
    {
        OnGameStart?.Invoke(width, height, iconSprites.Length);
    }

    public void GenerateField(int[,] field)
    {
        _tiles = new Tile[width, height];
        
        float startX = transform.position.x;
        float startY = transform.position.y;
        Vector2 offset = tilePrefab.tile.bounds.size;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile newTile = Instantiate(tilePrefab.gameObject, 
                    new Vector3(startX + offset.x * x, startY + offset.y * y, 0), 
                    Quaternion.identity).GetComponent<Tile>();
                
                newTile.transform.parent = transform;
                _tiles[x, y] = newTile;
            }
        }
        FillTileArray(field);
    }
    
    private void FillTileArray(int[,] field)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tiles[x, y].icon.sprite = iconSprites[field[x,y]];
            }
        }
    }
    
    private void ChangePosition()
    {
        
    }

    private void DestroyTiles()
    {
        
    }

    private void GenerateTiles()
    {
        
    }

    private void UpdateScore()
    {
        
    }
}
