using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
   public LevelModel model;
   public LevelView view;

   private void Awake()
   {
      model = new LevelModel();
   }

   private void OnEnable()
   {
      view.OnGameStart += model.GenerateField;
      model.OnFieldGenerated += view.GenerateField;
   }

   private void OnDisable()
   {
      view.OnGameStart -= model.GenerateField;
      model.OnFieldGenerated -= view.GenerateField;
   }
   
   private void GetSwap()
   {
      
   }
}
