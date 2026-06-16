using UnityEngine;

namespace MyBird
{
    public class Player : MonoBehaviour
    {
        [Header("Jump")]
        [SerializeField] private float jumpVelocity = 5f;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2f; // 오른쪽으로 이동하는 속도

        [Header("Idle")]
        [SerializeField] private float idleUpForce = 20f; // 대기 상태에서 아래로 떨어질 때 주는 위로 향하는 힘

        [Header("Rotation")]
        [SerializeField] private float maxUpAngle = 30f;     // 최대 위 회전
        [SerializeField] private float maxDownAngle = -90f;  // 최대 아래 회전
        [SerializeField] private float rotationSpeed = 10f;  // 회전 보간 속도
        [SerializeField] private float upVelocityForMax = 5f;   // 이 속도 이상이면 최대 위각
        [SerializeField] private float downVelocityForMax = 10f; // 이 속도 이상이면 최대 아래각

        private Rigidbody2D rb;

        private enum PlayerState { Idle, Playing }
        private PlayerState state = PlayerState.Idle;
        private bool canMoveForward = true;

        public bool IsPlaying => state == PlayerState.Playing;

        //UI
        public GameObject readyUI;
        public GameObject gameOverUI;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Player requires a Rigidbody2D component.");
            }
        }

        private void Update()
        {
            //인풋 처리
            InputBird();

            // 플레이 중일 때만 오른쪽으로 이동
            if (state == PlayerState.Playing)
            {
                if (canMoveForward)
                {
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
            }

            UpdateRotation();
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            // 대기 상태에서는 아래로 떨어질 때만 위로 향하는 힘을 줘서 제자리에서 떠 있게 한다
            if (state == PlayerState.Idle)
            {
                if (rb.linearVelocity.y < 0f)
                {
                    rb.AddForce(Vector2.up * idleUpForce);
                }
            }
        }

        private void InputBird()
        {
            bool keyJump = false;

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                keyJump = true;
            }
#else
            //터치 처리
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    keyJump = true;
                }
            }
#endif      

            if (keyJump)
            {
                // 첫 입력이면 대기 상태에서 플레이 상태로 전환
                if (state == PlayerState.Idle)
                {
                    readyUI.SetActive(false);
                    state = PlayerState.Playing;
                }

                Jump();
            }
            
        }

        private void Jump()
        {
            //이동 불가 상태면 점프 안함
            if (!canMoveForward)
            {
                return;
            }

            if (rb == null) return;

            // 수직 속도를 직접 설정해서 즉시 점프하도록 한다
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        private void UpdateRotation()
        {
            if (rb == null) return;

            float velY = rb.linearVelocity.y;
            float targetAngle;

            if (velY > 0f)
            {
                float t = Mathf.Clamp01(velY / Mathf.Max(0.0001f, upVelocityForMax));
                targetAngle = Mathf.Lerp(0f, maxUpAngle, t);
            }
            else
            {
                float t = Mathf.Clamp01(-velY / Mathf.Max(0.0001f, downVelocityForMax));
                targetAngle = Mathf.Lerp(0f, maxDownAngle, t);
            }

            float currentZ = transform.eulerAngles.z;
            float newZ = Mathf.LerpAngle(currentZ, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;

            if (other.CompareTag("Point"))
            {
                GameManager.Instance?.AddScore(1);
                Destroy(other.gameObject, 2);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null) return;

            if (collision.collider != null && collision.collider.CompareTag("Pipe"))
            {
                gameOverUI.SetActive(true);
                GameManager.Instance?.GameOver();
            }
        }

        public void DisableForwardMovement()
        {
            canMoveForward = false;
        }
    }
}