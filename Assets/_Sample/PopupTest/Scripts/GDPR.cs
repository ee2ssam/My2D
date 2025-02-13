using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 개인 정보 동의 창: 동의, 취소, 개인정보 페이지 링크
    /// </summary>
    public class GDPR : Popup
    {
        //개인 정보 동의
        public void OnUserClickAccept()
        {
            //개인 정보 동의 저장
            Close();
        }

        //개인 정보 동의 창 취소
        public void OnUserClickCancel()
        {
            //개인 정보 거부 저장
            Close();
        }

        //개인 정보 동의 인터넷 페이지 연결
        public void OnUserClickPrivacyPolicy()
        {
            //Application.OpenURL("home_page_url");
        }
    }
}
