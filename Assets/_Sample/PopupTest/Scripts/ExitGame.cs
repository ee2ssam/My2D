using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 게임 종료 창 : 예(게임종료), 아니오
    /// </summary>
    public class ExitGame : Popup
    {
        #region Variables
        [SerializeField] private CustomButton yes;
        #endregion

        private void OnEnable()
        {
            yes.onClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            //게임종료
            Application.Quit();
            Debug.Log("Application.Quit");
        }
    }
}
