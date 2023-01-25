using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu
{
    public class MaxScoreText : MonoBehaviour
    {
        [SerializeField] private Text ScoreText1;
        [SerializeField] private Text ScoreText2;
        private void Start()
        {
            var score = KeyManager.GetKey("Score");
            if (KeyManager.GetKey("MaxScore") < score)
                KeyManager.SetKey("MaxScore", score);

            var maxScore = $"MAX SCORE:\n{KeyManager.GetKey("MaxScore")}";
            ScoreText1.text = maxScore;
            ScoreText2.text = maxScore;
        }
    }
}
