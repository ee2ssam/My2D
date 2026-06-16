using UnityEngine;

namespace MyBird
{
    public class GroundMove : MonoBehaviour
    {   
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2f; // 왼쪽으로 이동하는 속도
        
        private void Update()
        {
            // 게임 오버 상태 이면 이동하지 않음
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            {
                return;
            }

            // 왼쪽으로 지속 이동 (Translate)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

            // 화면 왼쪽 끝으로 이동하면 오른쪽 끝으로 순간 이동
            if (transform.localPosition.x < -8.4f) // 화면 왼쪽 끝 위치 (예시)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 8.4f, transform.localPosition.y, transform.localPosition.z); // 화면 오른쪽 끝 위치 (예시)
            }
        }
    }
}
