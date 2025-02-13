using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 팝업 UI를 관리하는 부모 클래스: 
    /// show,hide 기능(애니메이션 구현), close (버튼) - 결과를 리턴
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(CanvasGroup))]
    public class Popup : MonoBehaviour
    {
        #region Variables
        public bool fade = false;           //팝업 페이드 기능

        private Animator animator;
        public CustomButton closeButton;    //창 닫기 버튼
        private CanvasGroup canvasGroup;

        //기능 구현
        public Action OnShowAction;         //팝업창 show할때 등록된 함수 호출
        public Action<PopupResult> OnCloseAction;   //팝업창 close할때 등록된 함수 호출 
        protected PopupResult result;

        public delegate void PopupEvents(Popup popup);

        public static event PopupEvents OnOpenPopup;
        public static event PopupEvents OnClosePopup;
        public static event PopupEvents OnBeforeClosePopup;
        #endregion

        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
            canvasGroup = GetComponent<CanvasGroup>();

            //close 버튼 클릭시 호츨되는 함수 등록
            if(closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }
        }

        public void Show<T>(Action onShow = null, Action<PopupResult> onClose = null)
        {
            if(onShow != null)
            {
                OnShowAction = onShow;
            }
            if (onClose != null)
            {
                OnCloseAction = onClose;
            }

            OnOpenPopup?.Invoke(this);
            PlayShowAnimation();
        }

        private void PlayShowAnimation()
        {
            if (animator != null)
            {
                animator.Play("popup_show");
            }
        }

        //Show 애니메이션 중간에 이벤트 함수 등록하여 등장 효과음 재생
        public virtual void ShowAnimationSound()
        {
            //TODO: 등장 효과음 재생
        }

        //Show 애니메이션 마지막에 이벤트 함수 등록하여 호출
        public virtual void AfterShowAnimation()
        {
            OnShowAction?.Invoke();
        }

        //Hide 애니메이션 중간에 이벤트 함수 등록하여 퇴장 효과음 재생
        public virtual void HideAnimationSound()
        {
            //TODO: 퇴장 효과음 재생
        }

        //Hide 애니메이션 마지막에 이벤트 함수 등록하여 호출
        public virtual void AfterHideAnimation()
        {
            OnClosePopup?.Invoke(this);
            OnCloseAction?.Invoke(result);
            Destroy(gameObject, 0.5f);
        }

        public virtual void Close()
        {
            if(closeButton != null)
            {
                closeButton.interactable = false;
            }

            canvasGroup.interactable = false;

            OnBeforeClosePopup?.Invoke(this);
            if (animator != null)
            {
                animator.Play("popup_hide");
            }
        }

        public void Show()
        {
            canvasGroup.interactable = true;
            //페이드 효과
            canvasGroup.alpha = 1.0f;
        }

        public virtual void Hide()
        {
            canvasGroup.interactable = false;
            //페이드 효과
            canvasGroup.alpha = 0.0f;
        }

        protected void StopInteraction()
        {
            canvasGroup.interactable = false;
        }

    }
}
