using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    public class Damageable : MonoBehaviour
    {
        #region Variables
        private Animator animator;

        //데미지 입을때 등록된 함수 호출
        public UnityAction<float, Vector2> hitAction;

        //체력
        [SerializeField] private float maxHealth = 100f;
        public float MaxHealth
        {
            get { return maxHealth; }
            private set { maxHealth = value; }
        }

        [SerializeField] private float currentHealth;
        public float CurrentHealth
        {
            get {  return currentHealth; }
            private set
            {
                currentHealth = value;
                
                //죽음 처리
                if(currentHealth <= 0)
                {
                    IsDeath = true;
                }
            }
        }

        private bool isDeath = false;
        public bool IsDeath
        {
            get { return isDeath; }
            private set
            {
                isDeath = value;
                //애니메이션
                animator.SetBool(AnimationString.IsDeath, value);
            }
        }

        //무적모드
        private bool isInvincible = false;
        [SerializeField] private float invincibleTimer = 3f;
        private float countdown = 0f;

        //
        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationString.LockVelocity);
            }
            private set
            {
                animator.SetBool(AnimationString.LockVelocity, value);
            }
        }
        #endregion

        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
            countdown = invincibleTimer;
        }

        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;
            countdown = invincibleTimer;
        }

        private void Update()
        {
            //무적상태이면 무적 타이머를 돌린다
            if (isInvincible)
            {
                if(countdown <= 0)
                {
                    isInvincible = false;

                    //타이머 초기화
                    countdown = invincibleTimer;
                }
                countdown -= Time.deltaTime;
            }
        }

        public void TakeDamage(float damage, Vector2 knocback)
        {
            if(!IsDeath && !isInvincible)
            {
                //무적모드 초기화
                isInvincible = true;

                //데미지 전의 hp
                float beforeHealth = CurrentHealth;

                CurrentHealth -= damage;
                Debug.Log($"{transform.name}가 현재 체력은 {CurrentHealth}");

                LockVelocity = true;
                animator.SetTrigger(AnimationString.HitTrigger); //애니메이션

                //데미지 효과
                /*if (hitAction != null)
                {
                    hitAction.Invoke(damage, knocback);
                }*/
                float realDamage = beforeHealth - CurrentHealth;

                hitAction?.Invoke(realDamage, knocback);
                CharacterEvents.characterDamaged?.Invoke(gameObject, realDamage);

            }
        }

        //
        public bool Heal(float amount)
        {
            if (CurrentHealth >= MaxHealth)
            {
                return false;
            }
            //힐 전의 hp
            float beforeHealth = CurrentHealth;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            //실제 힐 hp값
            float realHealth = CurrentHealth - beforeHealth;

            CharacterEvents.characterHealed?.Invoke(gameObject, realHealth);

            return true;
        }
    }
}