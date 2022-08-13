using System;
using UnityEngine;

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

   private void Awake()
   {
      model = new LevelModel();
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
            Tile newTile = Instantiate(tilePrefab.gameObject).GetComponent<Tile>();

            newTile.tileSelected += GetSwap;
            _tiles[y, x] = newTile;
         }
      }
      view.SetTilesPosition(_tiles);
      view.SetTilesSprite(field);
   }
   

   private void GetSwap(Tile tile, Vector2 tilePosition)
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
         view.ChangePosition(_selectedTile.transform ,tile.transform);
         model.ChangePosition(_selectedTilePosition, tilePosition);
         
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
