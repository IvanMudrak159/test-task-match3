using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
   
   [SerializeField] private int width, height;
   [SerializeField] private Tile tilePrefab;
   [SerializeField] private SpriteRenderer backGroundImage;
   public Sprite[] iconSprites;
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
      OnGameStart?.Invoke(width, height, iconSprites.Length);
   }

   private void GenerateField(int[,] field)
   {
      _tiles = new Tile[width, height];
        
      float startX = transform.position.x;
      float startY = transform.position.y;
      Vector2 offset = tilePrefab.tile.bounds.size;
      Debug.Log(offset);
      for (int y = 0; y < height; y++)
      {
         for (int x = 0; x < width; x++)
         {
            Tile newTile = Instantiate(tilePrefab.gameObject, 
               new Vector3(startX + offset.x * x, startY + offset.y * y, 0), 
               Quaternion.identity).GetComponent<Tile>();

            newTile.tileSelected += GetSwap;
            newTile.transform.parent = transform;
            _tiles[y, x] = newTile;
         }
      }
      FillTileArray(field);
   }
    
   private void FillTileArray(int[,] field)
   {
      for (int x = 0; x < width; x++)
      {
         for (int y = 0; y < height; y++)
         {
            _tiles[x, y].icon.sprite = iconSprites[field[x,y]];
         }
      }
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
         _selectedTile.UpdateInfo();
         tile.UpdateInfo();
         
         ChangePosition(_selectedTile.transform ,tile.transform);
         model.ChangePosition(_selectedTilePosition, tilePosition);
         _selectedTile = null;
      }
   }
   
   private void ChangePosition(Transform firstTile, Transform secondTile)
   {
      Vector2 tempPos = firstTile.position;
      firstTile.position = secondTile.position;
      secondTile.position = tempPos;
   }

   private void DestroyTiles()
   {
        
   }

   private void GenerateTiles()
   {
        
   }

   private void UpdateScore()
   {
        
   }
}
