using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    //캐릭터와 관련된 이벤트 함수들을 관리하는 클래스
    public class CharacterEvents
    {
        //캐릭터가 데미지를 입을때 등록된 함수 호출
        public static UnityAction<GameObject, float> characterDamaged;
        //캐릭터가 힐할때 등록된 함수 호출
        public static UnityAction<GameObject, float> characterHealed;

        //...
    }
}
