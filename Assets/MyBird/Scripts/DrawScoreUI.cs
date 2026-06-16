using UnityEngine;
using TMPro;

namespace MyBird
{
    public class DrawScoreUI : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        // Update is called once per frame
        void Update()
        {
            if(scoreText != null)
            {
                scoreText.text = GameManager.Instance?.Score.ToString();
            }
        }
    }
}