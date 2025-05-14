using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    // Health를 관리하는 클래스, takedamage, die 구현
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        public Animator animator;

        //체력
        [SerializeField] private float currentHealth;
        //초기 체력(최대 체력)
        [SerializeField] private float maxHealth = 100;
        //죽음 체크
        private bool isDeath = false;

        //무적 타이머
        private bool isInvincible = false;  //true이면 데미지를 입지 않는다
        [SerializeField]
        private float invincibleTime = 3f;  //무적 타임
        private float countdown = 0f;

        //델리게이트 이벤트 함수
        //매개변수로 float, Vector2가 있는 함수 등록 가능
        public UnityAction<float, Vector2> hitAction;
        #endregion

        #region Property
        //체력
        public float CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            private set
            {
                currentHealth = value;

                if(currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        //최대 체력
        public float MaxHealth
        {
            get
            {
                return maxHealth;
            }
            private set
            {
                maxHealth = value;
            }
        }

        //죽음 체크
        public bool IsDeath
        {
            get
            {
                return isDeath;
            }
            private set
            {
                isDeath = value;
            }
        }

        //이동 속도 잠그기
        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationString.lockVelocity);
            }
            set
            {
                animator.SetBool(AnimationString.lockVelocity, value);
            }
        }

        //hp 풀 체크
        public bool IsHealthFull => CurrentHealth >= MaxHealth;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;            
        }

        private void Update()
        {
            //무적 타이머
            if(isInvincible)
            {
                countdown += Time.deltaTime;
                if (countdown >= invincibleTime)
                {
                    //타이머 기능
                    isInvincible = false;
                }
            }
        }
        #endregion

        #region Custom Method
        //Health 감산
        //매개변수로 데미지량과 뒤로 밀리는 값을 받아온다
        public bool TakeDamage(float damage, Vector2 knockback)
        {
            if(IsDeath || isInvincible)
            {
                Debug.Log("무적 모드");
                return false;
            }

            CurrentHealth -= damage;
            //Debug.Log($"CurrentHealth : {CurrentHealth}");

            //무적 모드 셋팅 - 타이머 초기화
            isInvincible = true;
            countdown = 0;

            //애니메이션
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;

            //효과: SFX, VFX, 넉백효과, UI 효과 

            //델리게이트 함수에 등록된 함수들 호출: 효과 연출에 필요한 함수 등록
            /*if(hitAction != null)
            {
                hitAction.Invoke(damage, knockback);
            }*/
            hitAction?.Invoke(damage, knockback);

            //UI 효과 -데미지text 프리팹 생성하는 함수가 등록된 이벤트 함수 호출
            //Health Text, Health Bar 관련함수 호출
            CharacterEvents.chararcterDamaged?.Invoke(gameObject, damage);

            return true;
        }

        private void Die()
        {
            IsDeath = true;
            animator.SetBool(AnimationString.isDeath, true);
        }

        //Health 가산 - 매개 변수 만큼 Health 충전
        //health를 실질적으로 충전하면 참을 반환, hp가 풀이어서 충전하지 않았다면 거짓반환
        public bool Heal(float healAmount)
        {
            //죽음 체크, 풀 체크
            if (IsDeath || IsHealthFull)
            {
                return false;
            }

            //힐하기 전의 hp
            float beforeHealth = CurrentHealth;

            //힐하기
            CurrentHealth += healAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            //실제 힐 값은
            float actualHealth = CurrentHealth - beforeHealth;

            //UI 효과 -힐text 프리팹 생성하는 함수가 등록된 이벤트 함수 호출
            //Health Text, Health Bar 관련함수 호출
            CharacterEvents.characterHealed?.Invoke(gameObject, actualHealth);

            return true;
        }
        #endregion
    }
}
