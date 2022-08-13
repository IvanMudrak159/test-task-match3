using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
   public SpriteRenderer icon;
   public SpriteRenderer tile;
   private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
   private bool _isSelected;
   private Vector2 _position;

   public delegate void TileSelected(Tile tile, Vector2 position);

   public event TileSelected tileSelected;
   private void Start()
   {
      CalculatePosition();
   }

   private void Select()
   {
      _isSelected = true;
      icon.color = selectedColor;
      tileSelected?.Invoke(this, _position);
   }
   
   private void Deselect()
   {
      _isSelected = false;
      icon.color = Color.white;
   }

   private void CalculatePosition()
   {
      Vector2 offset = tile.bounds.size;
      _position = new Vector2((int)(transform.localPosition.y / offset.y),
         (int)(transform.localPosition.x / offset.x));
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
      
      Debug.Log(_position);
   }
}
