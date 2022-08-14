using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
   
   [SerializeField] private int width, height;
   [SerializeField] private Tile tilePrefab;
   private Tile[,] _tiles;
   
   private Tile _selectedTile = null;
   private Vector2 _selectedTilePosition;
   
   public LevelModel model;
   public LevelView view;

   public delegate void GameStarted(int width, int height, int iconLength);
   public event GameStarted OnGameStart;

   private Vector2[] _directions;
   private void Awake()
   {
      model = new LevelModel();
      _directions = new[] {Vector2.down, Vector2.left, Vector2.right, Vector2.up};
   }

   private void OnEnable()
   {
      OnGameStart += model.GenerateField;
      model.OnFieldGenerated += GenerateField;
   }

   private void OnDisable()
   {
      OnGameStart -= model.GenerateField;
      model.OnFieldGenerated -= GenerateField;
   }

   private void Start()
   {
      OnGameStart?.Invoke(width, height, view.IconSpritesLength);
   }

   private void GenerateField(int[,] field)
   {
      _tiles = new Tile[width, height];
      
      for (int y = 0; y < height; y++)
      {
         for (int x = 0; x < width; x++)
         {
            Tile newTile = Instantiate(tilePrefab);
            
            newTile.tileSelected += SwapTiles;
            _tiles[y, x] = newTile;
         }
      }
      view.SetTilesPosition(_tiles);
      view.SetTilesSprite(field);
   }

   private void SwapTiles(Tile tile, Vector2 tilePosition)
   {
      if (_selectedTile == tile)
      {
         _selectedTile = null;
      }
      else if (_selectedTile == null)
      {
         _selectedTile = tile;
         _selectedTilePosition = tilePosition;
      }
      else
      {
         bool isAdjacent = false;
         foreach (var direction in _directions)
         {
            if (direction == _selectedTilePosition - tilePosition)
            {
               isAdjacent = true;
               break;
            }
         }

         if (isAdjacent)
         {
            view.ChangePosition(_selectedTile.transform ,tile.transform);
            model.ChangePosition(_selectedTilePosition, tilePosition);
         }
         _selectedTile.UpdateInfo();
         tile.UpdateInfo();

         _selectedTile = null;
      }
   }

   private void DestroyTiles()
   {
        
   }

   private void GenerateTiles()
   {
        
   }
}
