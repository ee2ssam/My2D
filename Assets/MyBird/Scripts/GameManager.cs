using UnityEngine;

namespace MyBird
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int Score { get; private set; }

        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private Player player;

        private bool isGameOver = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            Score = 0;
            isGameOver = false;
        }

        public void AddScore(int amount)
        {
            if (isGameOver) return;
            Score += amount;
            Debug.Log("Score: " + Score);
        }

        public void GameOver()
        {
            if (isGameOver) return;
            isGameOver = true;

            // 플레이어가 더 이상 앞으로 못가게 한다
            if (player != null)
            {
                player.DisableForwardMovement();
            }

            // 스폰 매니저 중지
            if (spawnManager != null)
            {
                spawnManager.StopSpawning();
            }

            Debug.Log("Game Over");
        }
    }
}