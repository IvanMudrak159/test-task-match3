using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelModel: MonoBehaviour
{
    public int width, height = 9;
    public int iconLength = 7;
    private int[] _icons;
    
    private int[,] _field;

    public delegate void FieldGenerated(int[,] field);
    public event FieldGenerated OnFieldGenerated;
    
    private void Start()
    {
        GenerateField();
    }

    private void GenerateField()
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

    private void ChangePosition()
    {
        
    }

    private void FindMatch()
    {
        
    }

    private void FindAllMatches()
    {
        //Dunno if it will be used
    }

    private void DestroyTiles()
    {
        
    }

    private void GenerateTiles()
    {
        
    }
}
