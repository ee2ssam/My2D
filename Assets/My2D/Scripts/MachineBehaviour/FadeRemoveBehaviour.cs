using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    //게임 스프라이트 오브젝트를 페이드아웃 후 킬
    public class FadeRemoveBehaviour : StateMachineBehaviour
    {
        #region Variables
        private SpriteRenderer spriteRenderer;
        private GameObject removeObject;
        private Color startColor;

        //fade효과
        public float fadeTimer = 1f;
        private float countdown = 0f;

        //딜레이 시간후에 fade 효과 처리
        public float delayTime = 2f;
        private float delayCountdown = 0f;
        #endregion


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //참조
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            startColor = spriteRenderer.color;
            removeObject = animator.gameObject;

            //초기화
            countdown = fadeTimer;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //delayTime 만큼 딜레이
            if (delayCountdown < delayTime)
            {
                delayCountdown += Time.deltaTime;
                return;
            }

            //페이드 아웃 효과 spriteRenderer.color.a : 1 -> 0
            countdown -= Time.deltaTime;

            float newAlpha = startColor.a * (countdown / fadeTimer);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            //페이드 타임 끝
            if (countdown <= 0)
            {
                Destroy(removeObject);
            }   
        }
    }
}