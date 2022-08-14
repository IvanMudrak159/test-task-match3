using System;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    private Tile[,] _tiles;
    private int _width, _height;
    [SerializeField] private Sprite[] iconSprites;
    public int IconSpritesLength => iconSprites.Length;

    public void SetTilesPosition(Tile[,] tiles)
    {
        _tiles = tiles;
        _width = _tiles.GetLength(0);
        _height = _tiles.GetLength(1);

        float startX = transform.position.x;
        float startY = transform.position.y;
        Vector2 offset = tiles[0,0].tile.bounds.size;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _tiles[y, x].transform.position = new Vector3(startX + offset.x * x, startY + offset.y * y, 0);
                _tiles[y, x].transform.parent = transform;
            }
        }
    }
    
    public void SetTilesSprite(int[,] field)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _tiles[x, y].icon.sprite = iconSprites[field[x,y]];
            }
        }
    }

    public void ChangePosition(Transform firstTile, Transform secondTile)
    {
        Vector2 tempPos = firstTile.position;
        firstTile.position = secondTile.position;
        secondTile.position = tempPos;
    }

    private void UpdateScore()
    {
        
    }
}
