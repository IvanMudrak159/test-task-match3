using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelModel
{
    private int[,] _field;
    private int _width, _height;
    private int[] _icons;
    
    public delegate void FieldGenerated(int[,] field, int width, int height);
    public event FieldGenerated fieldGenerated;

    public delegate void MatchFound(List<Vector2Int> matchedTiles);

    public event MatchFound matchFound;
    
    public void GenerateField(int width, int height, int iconLength)
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
        fieldGenerated?.Invoke(_field, _width, _height);
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
                    //TODO: change -1 positions with other values, can swipe without match
                    foundMatch = true;
                    continue;
                }
                FindMatch(y, x, ArrayDirection.Up, true);
                FindMatch(y, x, ArrayDirection.Right, true);
            }
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
            if (direction == ArrayDirection.Up)
            {
                foreach (var tile in matchingTiles)
                {
                    List<Vector2Int> rightMatch = FindMatch(tile.y, tile.x, ArrayDirection.Right, false);
                    List<Vector2Int> leftMatch = FindMatch(tile.y, tile.x, ArrayDirection.Left, false);
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
            else if (direction == ArrayDirection.Right)
            {
                foreach (var tile in matchingTiles)
                {
                    List<Vector2Int> upMatch = FindMatch(tile.y, tile.x, ArrayDirection.Up, false);
                    if (upMatch.Count > 2)
                    {
                        upMatch.Remove(tile);
                        matchingTiles.AddRange(upMatch);
                        break;
                    }
                }
            }
            DestroyTiles(matchingTiles);
            matchFound?.Invoke(matchingTiles);
            
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

    private void GenerateTiles()
    {
        
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
