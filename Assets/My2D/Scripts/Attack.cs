using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class Attack : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float attackDamage = 10f;
        public Vector2 knockback = Vector2.zero;
        #endregion



        //충돌 체크해서 공격력 만큼 데미지 준다
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //데미지 입는 객체 찾기
            Damageable damageable = collision.GetComponent<Damageable>();

            if(damageable != null )
            {
                //knockback의 방향 설정
                Vector2 deliveredKnockback = (transform.parent.localScale.x > 0) ? knockback : new Vector2(-knockback.x, knockback.y);

                //Debug.Log($"{collision.name}가 데미지를 입었다");
                damageable.TakeDamage(attackDamage, deliveredKnockback);
            }
        }

    }
}
