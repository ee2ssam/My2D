using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    /// <summary>
    /// 플레이어 캐릭터의 움직임과 행동을 제어하는 클래스입니다.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        private Rigidbody2D rb;
        private Vector2 moveInput = Vector2.zero;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("PlayerController requires a Rigidbody2D component.");
            }
        }

        // New Input System -> Input Action "Move"에서 Invoke Unity Event 로 이 메서드를 연결하세요.
        // signature: Vector2 (x: 좌우 입력, y: 상하 입력) 를 받습니다.
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            // Rigidbody2D.velocity를 직접 설정하여 좌우 이동을 수행
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }
}
