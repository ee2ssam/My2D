using UnityEngine;
using UnityEngine.InputSystem;

namespace My2D
{
    //플레이어를 제어하는 클래스
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //강체
        private Rigidbody2D rb2D;
        //애니메이터
        public Animator animator;
        //그라운드, 벽 체크
        private TouchingDirection touchingDirection;
        //데미지
        private Damageable damageable;

        //이동        
        //걷는 속도 - 좌우로 걷는다
        [SerializeField] private float walkSpeed = 4f;
        //뛰는 속도 - 좌우로 뛴다
        [SerializeField] private float runSpeed = 7f;
        //점프시 좌우 이동 속도
        [SerializeField] private float airSpeed = 2f;

        //이동 입력값
        private Vector2 inputMove;

        //이동 키입력
        private bool isMoving = false;
        //런 키입력
        private bool isRunning = false;

        //반전
        //캐릭터 이미지가 바라보는 방향 상태 : 오른쪽 바라보면 true
        private bool isFacingRight = true;

        //점프키를 눌렀을때 위로 올라가는 속도값
        [SerializeField] private float jumpForce = 5f;
        #endregion

        #region Property
        //이동 키입력값 - 애니메이션 파라미터 셋팅
        public bool IsMoving
        {
            get
            {
                return isMoving;
            }
            set
            {
                isMoving = value;
                animator.SetBool(AnimationString.isMoving, value);
            }
        }

        //런 키입력값 - 애니메이션 파라미터 셋팅
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                isRunning = value;
                animator.SetBool(AnimationString.isRunning, value);
            }
        }

        //현재 이동 속도 셋팅
        public float CurrentSpeed
        {
            get
            {
                //공격시 이동 제어
                if(CannotMove)
                {
                    return 0f;
                }

                //인풋값이 들어왔을때 and 벽에 부딪히지 않을때
                if (IsMoving && touchingDirection.IsWall == false)
                {
                    if(touchingDirection.IsGround) //땅에 있을때
                    {
                        if (IsRunning) //시프트를 누르고 있을때
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else //공중에 떠 있을때
                    {
                        return airSpeed;
                    }
                }
                else
                {
                    return 0f;  //idle state, 벽에 부딪히고 있는 경우
                }
            }
        }

        //캐릭터 이미지가 바라보는 방향 상태 : 오른쪽 바라보면 true
        public bool IsFacingRight
        {
            get
            {
                return isFacingRight;
            }
            set
            {
                //반전 구현
                if(isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
                isFacingRight = value;
            }
        }

        //공격시 이동 제어값 읽어오기
        public bool CannotMove
        {
            get
            {
                return animator.GetBool(AnimationString.cannotMove);
            }
        }

        //죽음 체크
        public bool IsDeath => animator.GetBool(AnimationString.isDeath);
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            touchingDirection = this.GetComponent<TouchingDirection>();

            damageable = this.GetComponent<Damageable>();
            //델리게이트 함수 등록
            damageable.hitAction += OnHit;
        }

        private void FixedUpdate()
        {
            //인풋값에 따라 플레이어 좌우 이동
            if(damageable.LockVelocity == false)
            {
                rb2D.linearVelocity = new Vector2(inputMove.x * CurrentSpeed, rb2D.linearVelocityY);
            }

            //애니메이터 속도값 셋팅
            animator.SetFloat(AnimationString.yVelocity, rb2D.linearVelocityY);
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();

            if(IsDeath == false)
            {
                //입력값에 따른 반전
                SetFacingDirection(inputMove);

                //인풋 값이 들어오면 IsMoving 파라미터 셋팅
                IsMoving = (inputMove != Vector2.zero);
            }
            else
            {
                IsMoving = false;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if(context.started) //button down
            {
                IsRunning = true;
            }
            else if(context.canceled)   //button up
            {
                IsRunning = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && touchingDirection.IsGround && IsDeath == false) //button down
            {
                //속도 연산 - 위로 이동하는 속도값 셋팅
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocityX, jumpForce);
                
                //애니메이션
                animator.SetTrigger(AnimationString.jumpTrigger);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started && touchingDirection.IsGround) //mouse left button down
            {
                //공격하라 애니메이션
                animator.SetTrigger(AnimationString.attackTrigger);
            }
        }

        public void OnBowAttack(InputAction.CallbackContext context)
        {
            if (context.started && touchingDirection.IsGround) //F key down
            {
                //활을 쏘라
                animator.SetTrigger(AnimationString.bowAttackTrigger);
            }
        }


        //반전, 바라보는 방향 전환 - 입력값에 따라
        void SetFacingDirection(Vector2 moveInput)
        {            
            if(moveInput.x > 0f && IsFacingRight == false) //왼쪽을 바라보고 있고 우로 이동 입력
            {
                IsFacingRight = true;
            }
            else if(moveInput.x < 0f && IsFacingRight == true) //오른쪽을 바라보고 있는데 좌로 이동
            {
                IsFacingRight = false;
            }
        }

        //데미지 입을때 호출되는 함수 - 데미지 입을때의 속도 셋팅
        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocityY + knockback.y);
        }
        #endregion

    }
}
