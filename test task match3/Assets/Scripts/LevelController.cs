using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
   public LevelModel model;
   public LevelView view;

   private void OnEnable()
   {
      model.onFieldGenerated += view.GenerateField;
   }

   private void OnDisable()
   {
      model.onFieldGenerated -= view.GenerateField;
   }

   private void GetSwap()
   {
      
   }
}
