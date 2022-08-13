using System;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private Row[] rows;
    public Sprite[] iconSprites;
    private Tile[,] _tiles;

    private void Start()
    {
    }

    private void FillTileArray(int width, int height)
    {
        _tiles = new Tile[rows.Length, rows[0].tiles.Length];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tiles[x, y] = rows[x].tiles[y];
            }
        }
    }
    public void GenerateField(int[,] field)
    {
        int width = field.GetLength(0);
        int height = field.GetLength(1);
        FillTileArray(width, height);
        
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
