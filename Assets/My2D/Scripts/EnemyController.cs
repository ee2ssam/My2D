using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace My2D
{
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirections touchingDirections;
        private Damageable damageable;

        //플레이어 감지
        public DetectionZone detectionZone;
        //낭떨어지 감지
        public DetectionZone detectionCliff;

        //이동속도
        [SerializeField] private float runSpeed = 4f;
        //이동방향
        private Vector2 directionVector = Vector2.right;

        //이동 가능 방향
        public enum WalkableDirection { Left, Right }

        //현재 이동 방향
        private WalkableDirection walkDirection = WalkableDirection.Right;
        public WalkableDirection WalkDiretion
        {
            get {  return walkDirection; }
            private set {
                //이미지 플립
                transform.localScale *= new Vector2(-1, 1);

                //실제 이동하는 방향값
                if (value == WalkableDirection.Left)
                {
                    directionVector = Vector2.left;
                }
                else if (value == WalkableDirection.Right)
                {
                    directionVector = Vector2.right;
                }

                walkDirection = value;
            }
        }

        //공격 타겟 설정
        [SerializeField] private bool hasTarget = false;
        public bool HasTarget
        {
            get { return hasTarget; }
            private set {
                hasTarget = value;
                animator.SetBool(AnimationString.HasTarget, value);
            }
        }

        //이동 가능 상태/불가능 상태 - 이동 제한
        public bool CanMove
        {
            get { return animator.GetBool(AnimationString.CanMove); }
        }

        //감속 계수
        [SerializeField] private float stopRate = 0.2f;
        #endregion

        private void Awake()
        {
            //참조
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();

            damageable = GetComponent<Damageable>();
            damageable.hitAction += OnHit;

            detectionCliff.noColliderRamain += OnCliffDetection;
        }

        private void Update()
        {
            //적 감지 충돌체의 리스트 갯수가 0보다 크면 적이 감지 된것이다
            HasTarget = (detectionZone.detectedColliders.Count > 0);
        }

        private void FixedUpdate()
        {
            //땅에서 이동시 벽을 만나면 방향 전환
            if(touchingDirections.IsWall && touchingDirections.IsGround)
            {
                //방향전환 반전
                Flip();
            }

            if(!damageable.LockVelocity)
            {
                //이동
                if (CanMove)
                {
                    rb2D.velocity = new Vector2(directionVector.x * runSpeed, rb2D.velocity.y);
                }
                else
                {
                    //rb2D.velocity.x -> 0 : Lerp 멈춤
                    rb2D.velocity = new Vector2(Mathf.Lerp(rb2D.velocity.x, 0f, stopRate), rb2D.velocity.y);
                }
            }            
        }

        //방향전환 반전
        private void Flip()
        {
            if (WalkDiretion == WalkableDirection.Left)
            {
                WalkDiretion = WalkableDirection.Right;
            }
            else if(WalkDiretion == WalkableDirection.Right)
            {
                WalkDiretion = WalkableDirection.Left;
            }
            else
            {
                Debug.Log("Error Flip Direction");
            }
        }

        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.velocity = new Vector2(knockback.x, rb2D.velocity.y + knockback.y);
        }

        public void OnCliffDetection()
        {
            if (touchingDirections.IsGround)
            {
                Flip();
            }
        }
    }
}
