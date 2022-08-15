using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldModel
{
    private int[,] _field;
    private int _width, _height;
    private int[] _icons;
    
    public delegate void FieldGenerateHandler(int[,] field, int height, int width);
    public event FieldGenerateHandler FieldGeneratedEvent;

    public delegate void MatchFoundHandler(List<Vector2Int> matchedTiles);
    public event MatchFoundHandler MatchFoundEvent;

    public delegate void FieldUpdatedHandler(int[,] field);
    public event FieldUpdatedHandler FieldUpdatedEvent;
    
    public void GenerateFieldEvent(int width, int height, int iconLength)
    {
        _width = width;
        _height = height;
        
        _icons = Enumerable.Range(0, iconLength).ToArray();
        int previousLeft = -1;
        int[] previousBelow = new int[width];
        
        _field = new int[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int[] possibleIcons = _icons.Where(val => val != previousLeft && val != previousBelow[x]).ToArray();
                int iconIndex = Random.Range(0, possibleIcons.Length);
                _field[y, x] = possibleIcons[iconIndex];

                previousLeft = _field[y, x];
                previousBelow[x] = _field[y, x];
            }
        }
        FieldGeneratedEvent?.Invoke(_field, _height, _width);
    }

    public void ChangePosition(Vector2Int firstPos, Vector2Int secondPos)
    {
        int tempValue = _field[firstPos.y, firstPos.x];
        _field[firstPos.y, firstPos.x] = _field[secondPos.y, secondPos.x];
        _field[secondPos.y, secondPos.x] = tempValue;
    }

    public bool FindAllMatches()
    {
        bool foundMatch = false;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_field[y, x] == -1)
                {
                    foundMatch = true;
                    continue;
                }
                FindMatch(y, x, Vector2Int.up, true);
                FindMatch(y, x, Vector2Int.right, true);
            }
        }

        if (foundMatch)
        {
            ShiftDownTiles();
            GenerateTiles();
        }
        
        return foundMatch;
    }

    private List<Vector2Int> FindMatch(int y, int x, Vector2Int direction, bool firstIteration)
    {
        List<Vector2Int> matchingTiles = new List<Vector2Int>();
        int currentTileType = _field[y, x];
        Vector2Int currentTile = new Vector2Int(x, y);
        
        while (IsInside(currentTile))
        {
            int nextTileType = _field[currentTile.y, currentTile.x];
            
            if (nextTileType != currentTileType) break;
            matchingTiles.Add(currentTile);
            currentTileType = nextTileType;
            currentTile += direction;
        }
        
        if (!firstIteration) return matchingTiles;

        if (matchingTiles.Count > 2)
        {
            if (direction == Vector2Int.up)
            {
                foreach (var tile in matchingTiles)
                {
                    List<Vector2Int> rightMatch = FindMatch(tile.y, tile.x, Vector2Int.right, false);
                    List<Vector2Int> leftMatch = FindMatch(tile.y, tile.x, Vector2Int.left, false);
                    if (rightMatch.Count + leftMatch.Count > 3)
                    {
                        rightMatch.Remove(tile);
                        leftMatch.Remove(tile);
                        matchingTiles.AddRange(rightMatch);
                        matchingTiles.AddRange(leftMatch);
                        break;
                    }
                }
            }
            else if (direction == Vector2Int.right)
            {
                foreach (var tile in matchingTiles)
                {
                    List<Vector2Int> upMatch = FindMatch(tile.y, tile.x, Vector2Int.up, false);
                    if (upMatch.Count > 2)
                    {
                        upMatch.Remove(tile);
                        matchingTiles.AddRange(upMatch);
                        break;
                    }
                }
            }
            DestroyTiles(matchingTiles);
            MatchFoundEvent?.Invoke(matchingTiles);
            
        }
        return new List<Vector2Int>();
    }
    
    private bool IsInside(Vector2Int tile)
    {
        bool yInside = tile.y >= 0 && tile.y < _height;
        bool xInside = tile.x >= 0 && tile.x < _width;
        return xInside && yInside;
    }

    private void DestroyTiles(List<Vector2Int> tilesToDestroy)
    {
        foreach (var tile in tilesToDestroy)
        {
            _field[tile.y, tile.x] = -1;
        }
    }

    private void ShiftDownTiles()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if(_field[y,x ] == -1) continue;
                int depth = GetDepth(y, x);
                if (depth != 0)
                {
                    _field[y - depth, x] = _field[y, x];
                    _field[y, x] = -1;
                }
            }
        }
    }

    private int GetDepth(int y, int x)
    {
        int depth = 0;
        for (int i = y - 1; i >= 0; i--)
        {
            if(_field[i, x] != -1) break;
            depth += 1;
        }
        return depth;
    }

    private void GenerateTiles()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_field[y, x] == -1)
                {
                    int iconIndex = Random.Range(0, _icons.Length);
                    _field[y, x] = _icons[iconIndex];
                }
            }
        }
        FieldUpdatedEvent?.Invoke(_field);
    }

    //Debug
    private void PrintField()
    {
        string table = "";
        for (int y = _height - 1; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < _width; x++)
            {
                row += _field[y, x] + " ";
            }
            row += "\n";
            table += row;
        }
        Debug.Log(table);
    }
}
