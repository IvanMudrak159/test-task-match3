using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    private Tile[,] _tiles;
    [SerializeField] private int width, height;
    [SerializeField] private Sprite[] iconSprites;
    [SerializeField] private float shiftDelay;

    public delegate void GenerateFieldHandler(int height, int width, int iconLength);
    public event GenerateFieldHandler GenerateFieldEvent;

    public delegate bool FieldUpdatedHandler();
    public event FieldUpdatedHandler FieldUpdatedEvent;

    private void Start()
    {
        GenerateFieldEvent?.Invoke(height, width, iconSprites.Length);
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
    
    public void SwapTiles(Tile firstTile, Tile secondTile)
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
            if (IsNullTile(tile.y, tile.x)) continue;

            _tiles[tile.y, tile.x].icon.sprite = null;
        }
    }

    public void UpdateTiles(int[,] field)
    {
        StartCoroutine(FindNullTilesRoutine(field));
        
    }
    
    private IEnumerator FindNullTilesRoutine(int[,] field)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (IsNullTile(y, x))
                {
                    yield return StartCoroutine(ShiftDownTilesRoutine(y, x, field, shiftDelay));
                }
            }
        }
        FieldUpdatedEvent?.Invoke();
    }

    private int GetDepth(int y, int x)
    {
        int depth = 0;
        for (int i = y; i < height; i++)
        {
            if (IsNullTile(i, x))
            {
                depth += 1;
            }
        }
        return depth;
    }

    private IEnumerator ShiftDownTilesRoutine(int y, int x, int[,] field, float shiftDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(shiftDelay);

        int depth = GetDepth(y, x);
        for (int i = 0; i < depth; i++)
        {
            yield return delay;
            for (int j = y; j < height - 1; j++)
            {
                _tiles[j, x].icon.sprite = _tiles[j + 1, x].icon.sprite;
                _tiles[j + 1, x].icon.sprite = null;
            }
        }
        
        y = height - depth;
        for (int i = 0; i < depth; i++)
        {
            yield return delay;
            for (int j = 0; j <= i; j++)
            {
                int iconIndex = field[y + i - j, x];
                _tiles[height - j - 1, x].icon.sprite = iconSprites[iconIndex];
            }
        }
    }

    private bool IsNullTile(int y, int x)
    {
        if (_tiles[y, x].icon.sprite == null) return true;
        return false;
    }
}
