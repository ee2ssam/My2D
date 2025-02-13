using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public CustomButton settingButton;
        #endregion

        private void Start()
        {
            //버튼 이벤트에 함수 등록
            settingButton.onClick.AddListener(SettingButtonClicked);
        }

        //옵션 버튼 클릭시
        private void SettingButtonClicked()
        {
            MenuManager.Instance.ShowPopup<Settings>();
        }
    }
}
