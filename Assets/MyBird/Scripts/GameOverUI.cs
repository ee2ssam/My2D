using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    public class GameOverUI : MonoBehaviour
    {
        //다시 하기 버튼 클릭
        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
