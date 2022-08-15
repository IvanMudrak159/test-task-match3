using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
   public SpriteRenderer icon;
   public SpriteRenderer tile;
   private static readonly Color SelectedColor = new Color(.5f, .5f, .5f, 1.0f);
   private bool _isSelected;

   private Vector2Int _position;
   public Vector2Int Position => _position;

   public delegate void TileSelected(Tile tile);

   public event TileSelected tileSelected;
   private void Start()
   {
      //fix later: can be called before LevelView exe SetTilesPosition()
      CalculatePosition();
   }

   private void Select()
   {
      CalculatePosition();
      _isSelected = true;
      icon.color = SelectedColor;
      tileSelected?.Invoke(this);
   }
   
   private void Deselect()
   {
      _isSelected = false;
      icon.color = Color.white;
   }

   private void CalculatePosition()
   {
      Vector2 offset = tile.bounds.size;

      float x = transform.localPosition.x / offset.x;
      float y = transform.localPosition.y / offset.y;

      _position = new Vector2Int((int)x, (int)y);
   }
   
   public void UpdateInfo()
   {
      Deselect();
      CalculatePosition();
   }
   
   private void OnMouseDown()
   {
      if (_isSelected)
      {
         Deselect();
      }
      else
      {
         Select();
      }
   }
}
