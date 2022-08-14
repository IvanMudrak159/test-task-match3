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
        int[] previousBelow = new int[height];
        
        _field = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int[] possibleIcons = _icons.Where(val => val != previousLeft && val != previousBelow[y]).ToArray();
                int iconIndex = Random.Range(0, possibleIcons.Length);
                _field[x, y] = possibleIcons[iconIndex];

                previousLeft = _field[x, y];
                previousBelow[y] = _field[x, y];
            }
        }
        PrintField(_field);
        fieldGenerated?.Invoke(_field, _width, _height);
    }

    public void ChangePosition(Vector2Int firstPos, Vector2Int secondPos)
    {
        int tempValue = _field[firstPos.x, firstPos.y];
        _field[firstPos.x, firstPos.y] = _field[secondPos.x, secondPos.y];
        _field[secondPos.x, secondPos.y] = tempValue;
        PrintField(_field);
    }

    public bool FindAllMatches()
    {
        bool foundMatch = false;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_field[x, y] == -1)
                {
                    foundMatch = true;
                    continue;
                }
                FindMatch(x, y, ArrayDirection.Up, true);
                FindMatch(x, y, ArrayDirection.Right, true);
            }
        }

        return foundMatch;
    }

    private List<Vector2Int> FindMatch(int x, int y, Vector2Int direction, bool firstIteration)
    {
        List<Vector2Int> matchingTiles = new List<Vector2Int>();
        int currentTileType = _field[x, y];
        Vector2Int currentTile = new Vector2Int(x, y);
        
        while (IsInside(currentTile))
        {
            int nextTileType = _field[currentTile.x, currentTile.y];
            
            if (nextTileType != currentTileType) break;
            matchingTiles.Add(currentTile);
            currentTileType = nextTileType;
            currentTile += direction;

        }

        if (matchingTiles.Count > 2)
        {
            if (!firstIteration) return matchingTiles;
            
            if (direction == ArrayDirection.Up)
            {
                foreach (var tile in matchingTiles)
                {
                    List<Vector2Int> rightMatch = FindMatch(tile.x, tile.y, ArrayDirection.Right, false);
                    if (rightMatch.Count > 2)
                    {
                        rightMatch.Remove(tile);
                        matchingTiles.AddRange(rightMatch);
                        break;
                    }
                }
            }
            else if (direction == ArrayDirection.Right)
            {
                foreach (var tile in matchingTiles)
                {
                    List<Vector2Int> upMatch = FindMatch(tile.x, tile.y, ArrayDirection.Up, false);
                    List<Vector2Int> downMatch = FindMatch(tile.x, tile.y, ArrayDirection.Down, false);
                    if (upMatch.Count + downMatch.Count > 2)
                    {
                        upMatch.Remove(tile);
                        downMatch.Remove(tile);
                        
                        matchingTiles.AddRange(upMatch);
                        matchingTiles.AddRange(downMatch);
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
        bool xInside = tile.x >= 0 && tile.x < _width;
        bool yInside = tile.y >= 0 && tile.y < _height;
        return xInside && yInside;
    }
    
    private void DestroyTiles(List<Vector2Int> tilesToDestroy)
    {
        foreach (var tile in tilesToDestroy)
        {
            _field[tile.x, tile.y] = -1;
        }
    }

    private void GenerateTiles()
    {
        
    }

    //Debug
    private void PrintField(int[,] field)
    {
        string table = "";
        for (int x = _width - 1; x >= 0; x--)
        {
            string row = "";
            for (int y = 0; y < _height; y++)
            {
                row += _field[x, y] + " ";
            }

            row += "\n";
            table += row;
        }
        Debug.Log(table);
    }
}
