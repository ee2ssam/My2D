using UnityEngine;
using TMPro;

namespace My2D
{
    //UI를 관리하는 클래스
    public class UIManager : MonoBehaviour
    {
        #region Variables
        //데미지 텍스트 프리팹
        public GameObject damageTextPrefab;
        //힐 텍스트 프리팹
        public GameObject healTextPrefab;

        //캔바스
        public Canvas gameCanvas;

        //캔바스위의 스폰 위치 가져오기
        private Camera camera;
        [SerializeField] private Vector3 offset;    //캐릭터 머리위로 위치 보정값
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            camera = Camera.main;
        }

        private void OnEnable()
        {
            //이벤트 함수에 함수 등록
            CharacterEvents.chararcterDamaged += CharaecterTakeDamage;
            CharacterEvents.characterHealed += CharacterHeal;
        }

        private void OnDisable()
        {
            //이벤트 함수에 등록된 함수 제거
            CharacterEvents.chararcterDamaged -= CharaecterTakeDamage;
            CharacterEvents.characterHealed -= CharacterHeal;
        }
        #endregion

        #region Custom Method
        //데미지 텍스트 프리팹 생성, character:데미지를 입는 캐릭터
        public void CharaecterTakeDamage(GameObject character, float damage)
        {
            //프리팹 생성 - 생성된 프리팹의 부모를 Canvas로 지정
            //텍스트에 매개변수로 들어온 데미지량 셋팅            
            Vector3 spawnPosition = camera.WorldToScreenPoint(character.transform.position);

            GameObject textGo =Instantiate(damageTextPrefab, spawnPosition + offset, Quaternion.identity, gameCanvas.transform);
            //텍스트 객체
            TextMeshProUGUI damageText = textGo.GetComponent<TextMeshProUGUI>();
            if(damageText)
            {
                damageText.text = damage.ToString();
            }
        }

        //힐 텍스트 프리팹 생성, character:힐한 캐릭터
        public void CharacterHeal(GameObject character, float healAmount)
        {
            //프리팹 생성 - 생성된 프리팹의 부모를 Canvas로 지정
            //텍스트에 매개변수로 들어온 힐량 셋팅
            Vector3 spawnPosition = camera.WorldToScreenPoint(character.transform.position);
            GameObject textGo = Instantiate(healTextPrefab, spawnPosition + offset, Quaternion.identity, gameCanvas.transform);
            //텍스트 객체
            TextMeshProUGUI healText = textGo.GetComponent<TextMeshProUGUI>();
            if (healText)
            {
                healText.text = healAmount.ToString();
            }
        }
        #endregion
    }
}
