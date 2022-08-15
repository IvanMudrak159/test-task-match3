using System;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
   [SerializeField] private Text scoreText;
   private int _score;

   private void OnEnable()
   {
      BoardController.SendScoreEvent += DisplayScore;
   }

   private void OnDisable()
   {
      BoardController.SendScoreEvent -= DisplayScore;
   }

   private void DisplayScore(int score)
   {
      _score += score;
      scoreText.text = "Score: " + _score;
   }
}
