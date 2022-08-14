using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelModel
{
    private int[,] _field;
    private int[] _icons;
    
    public delegate void FieldGenerated(int[,] field);
    public event FieldGenerated OnFieldGenerated;
    
    public void GenerateField(int width, int height, int iconLength)
    {
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

        OnFieldGenerated?.Invoke(_field);
    }

    public void ChangePosition(Vector2 firstPosition, Vector2 secondPosition)
    {
        Vector2Int firstPos = new Vector2Int((int) firstPosition.x, (int) firstPosition.y);
        Vector2Int secondPos = new Vector2Int((int) secondPosition.x, (int) secondPosition.y);
        
        int tempValue = _field[firstPos.x, firstPos.y];
        _field[firstPos.x, firstPos.y] = _field[secondPos.x, secondPos.y];
        _field[secondPos.x, secondPos.y] = tempValue;
    }
    
    private void DestroyTiles()
    {
        
    }

    private void GenerateTiles()
    {
        
    }
}
