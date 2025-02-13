using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 메뉴 팝업을 관리하는 싱글톤 클래스
    /// </summary>
    public class MenuManager : PersistentSingleton<MenuManager>
    {
        #region Variables
        public List<Popup> popupStack = new List<Popup>();
        [SerializeField] private Canvas canvas;             //팝업 메뉴의 부모 캔버스 오브젝트
        #endregion

        private void OnEnable()
        {
            //팝업의 스태틱 이벤트 함수 등록하기
            Popup.OnClosePopup += ClosePopup;
            Popup.OnBeforeClosePopup += OnBeforeCloseAction;
        }

        private void OnDisable()
        {
            //팝업의 스태틱 이벤트 함수 제거하기
            Popup.OnClosePopup -= ClosePopup;
            Popup.OnBeforeClosePopup -= OnBeforeCloseAction;
        }

        private void OnBeforeCloseAction(Popup popupClose)
        {
            //TDDO: 페이드 효과
        }

        private void ClosePopup(Popup popupClose)
        {
            if (popupStack.Count > 0)
            {
                popupStack.Remove(popupClose);
                if (popupStack.Count > 0)
                {
                    var popup = popupStack.Last();
                    popup.Show();
                }
            }
        }

        //팝업창 열기
        public T ShowPopup<T>(Action onShow = null, Action<PopupResult> onClose = null) where T : Popup
        {
            //이미 창이 열렸는지 체크
            if(popupStack.OfType<T>().Any())
            {
                return popupStack.OfType<T>().First();
            }

            return (T)ShowPopup("Popups/" + typeof(T).Name, onShow, onClose);
        }

        public Popup ShowPopup(string pathWithType, Action onShow = null, Action<PopupResult> onClose = null)
        {
            //이미 창이 열렸는지 체크
            if(popupStack.Any(p => p.GetType().Name == pathWithType.Split('/').Last()))
            {
                return popupStack.First(p => p.GetType().Name == pathWithType.Split('/').Last());
            }

            //없으면
            var popupPrefab = Resources.Load<Popup>(pathWithType);
            if (popupPrefab == null)
            {
                Debug.Log("찾는 팝업 프리팹이 없습니다");
                return null;
            }

            return ShowPopup(popupPrefab, onShow, onClose);
        }

        public Popup ShowPopup(Popup popupPrefab, Action onShow = null, Action<PopupResult> onClose = null)
        {
            var popup = Instantiate(popupPrefab, canvas.transform);

            if(popupStack.Count > 0)
            {
                popupStack.Last().Hide();
            }

            popupStack.Add(popup);
            popup.Show<Popup>(onShow, onClose);
            var rectTransform = popup.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;

            return popup;
        }

        //특정 타입(T)의 팝업 찾기
        public T GetPopupOpened<T>() where T : Popup
        {
            foreach (var popup in popupStack)
            {
                if(popup.GetType() == typeof(T))
                {
                    return (T)popup;
                }
            }

            return null;
        }

        //열려 있는 모든 창 닫기
        public void CloseAllPopups()
        {
            for (int i = 0; i < popupStack.Count; i++)
            {
                var popup = popupStack[i];
                popup.Close();
            }

            popupStack.Clear();
        }

        //창이 하나라도 열려 있나?
        public bool IsAnyPopupOpened()
        {
            return popupStack.Count > 0;
        }

        public Popup GetLastPopup()
        {
            return popupStack.Last();
        }
    }
}
