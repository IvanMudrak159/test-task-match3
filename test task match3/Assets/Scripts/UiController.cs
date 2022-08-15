using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
   [SerializeField] private Text scoreText;
   private int _score;

   private void OnEnable()
   {
      FieldController.SendScoreEvent += DisplayScore;
   }

   private void OnDisable()
   {
      FieldController.SendScoreEvent -= DisplayScore;
   }

   private void DisplayScore(int score)
   {
      _score += score;
      scoreText.text = "Score: " + _score;
   }
}
