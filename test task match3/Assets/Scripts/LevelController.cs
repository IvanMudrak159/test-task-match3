using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
   
   [SerializeField] private Tile tilePrefab;
   
   private Tile _selectedTile;
   
   public LevelModel model;
   public LevelView view;
   
   private Vector2[] _adjacentDirections;
   private void Awake()
   {
      model = new LevelModel();
      _adjacentDirections = new[] {Vector2.down, Vector2.left, Vector2.right, Vector2.up};
   }

   private void OnEnable()
   {
      view.GameStarted += model.GenerateField;
      model.fieldGenerated += OnFieldGenerated;
      model.matchFound += view.DestroyTiles;
      model.matchFound += view.UpdateScore;
   }

   private void OnDisable()
   {
      view.GameStarted -= model.GenerateField;
      model.fieldGenerated -= OnFieldGenerated;
      model.matchFound -= view.DestroyTiles;
      model.matchFound -= view.UpdateScore;
   }
   
   private void OnFieldGenerated(int[,] field, int width, int height)
   {
      Tile[,] tiles = new Tile[height, width];
      
      for (int y = 0; y < height; y++)
      {
         for (int x = 0; x < width; x++)
         {
            Tile newTile = Instantiate(tilePrefab);
            
            newTile.tileSelected += SwapTiles;
            tiles[y, x] = newTile;
         }
      }
      view.SetTilesPosition(tiles);
      view.SetTilesSprite(field);
   }

   private void SwapTiles(Tile tile)
   {
      if (_selectedTile == tile)
      {
         _selectedTile = null;
      }
      else if (_selectedTile == null)
      {
         _selectedTile = tile;
      }
      else
      {
         if (IsAdjacent(tile))
         {
            view.ChangePosition(_selectedTile ,tile);
            model.ChangePosition(_selectedTile.Position, tile.Position);
            
            bool foundMatch = model.FindAllMatches();
            
            if (!foundMatch)
            {
               view.ChangePosition(_selectedTile ,tile);
               model.ChangePosition(_selectedTile.Position, tile.Position);
            }
         }
         _selectedTile.UpdateInfo();
         tile.UpdateInfo();

         _selectedTile = null;
      }
   }

   private bool IsAdjacent(Tile tile)
   {
      bool isAdjacent = false;
      foreach (var direction in _adjacentDirections)
      {
         if (direction == _selectedTile.Position - tile.Position)
         {
            isAdjacent = true;
            break;
         }
      }
      return isAdjacent;
   }
   
   private void DestroyTiles()
   {
        
   }

   private void GenerateTiles()
   {
        
   }
}
