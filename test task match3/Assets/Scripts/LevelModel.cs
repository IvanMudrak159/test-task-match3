
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelModel: MonoBehaviour
{
    public int width, height = 9;
    public int iconLength = 7;
    private int[,] _field;

    public delegate void OnFieldGenerated(int[,] field);

    public event OnFieldGenerated onFieldGenerated;
    private void Start()
    {
        GenerateField();
    }

    private void GenerateField()
    {
        _field = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _field[x, y] = Random.Range(0, iconLength);
            }
        }
        
        onFieldGenerated?.Invoke(_field);
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
