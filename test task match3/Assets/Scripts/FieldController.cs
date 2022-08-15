using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
   [SerializeField] private Tile tilePrefab;
   private Tile _selectedTile;
   
   private FieldModel _model;
   [SerializeField] private FieldView view;
   
   private Vector2[] _adjacentDirections;

   public delegate void SendScoreHandler(int score);
   public static event SendScoreHandler SendScoreEvent;

   private void Awake()
   {
      _model = new FieldModel();
      _adjacentDirections = new[] {Vector2.down, Vector2.left, Vector2.right, Vector2.up};
   }

   private void OnEnable()
   {
      view.GenerateFieldEvent += _model.GenerateFieldEvent;
      _model.FieldGeneratedEvent += OnFieldGeneratedEvent;
      _model.MatchFoundEvent += view.DestroyTiles;
      _model.FieldUpdatedEvent += view.UpdateTiles;
      view.FieldUpdatedEvent += _model.FindAllMatches;
      _model.MatchFoundEvent += SendScore;
   }

   private void OnDisable()
   {
      view.GenerateFieldEvent -= _model.GenerateFieldEvent;
      _model.FieldGeneratedEvent -= OnFieldGeneratedEvent;
      _model.MatchFoundEvent -= view.DestroyTiles;
      _model.FieldUpdatedEvent -= view.UpdateTiles;
      view.FieldUpdatedEvent -= _model.FindAllMatches;
      _model.MatchFoundEvent -= SendScore;
   }
   
   private void OnFieldGeneratedEvent(int[,] field, int height, int width)
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
            view.SwapTiles(_selectedTile ,tile);
            _model.ChangePosition(_selectedTile.Position, tile.Position);
            
            bool foundMatch = _model.FindAllMatches();
            
            if (!foundMatch)
            {
               view.SwapTiles(_selectedTile ,tile);
               _model.ChangePosition(_selectedTile.Position, tile.Position);
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

   private void SendScore(List<Vector2Int> matchedTiles)
   {
      SendScoreEvent?.Invoke(matchedTiles.Count * 100);
   }
}
