using TMPro;
using UnityEngine;

namespace My2D
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public GameObject damageTextPrefab;
        public GameObject healTextPrefab;

        private Canvas canvas;
        [SerializeField] private Vector3 healthTextOffset = Vector3.zero;
        #endregion

        private void Awake()
        {
            //참조
            canvas = FindObjectOfType<Canvas>();
        }

        private void OnEnable()
        {
            //캐릭터 관련 이벤트 함수 등록
            CharacterEvents.characterDamaged += CharacterDamaged;
            CharacterEvents.characterHealed += CharacterHealed;
        }

        private void OnDisable()
        {
            //캐릭터 관련 이벤트 함수 제거
            CharacterEvents.characterDamaged -= CharacterDamaged;
            CharacterEvents.characterHealed -= CharacterHealed;
        }


        public void CharacterDamaged(GameObject character, float damage)
        {
            //damageTextPrefab 스폰
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

            GameObject textGo = Instantiate(damageTextPrefab, spawnPosition + healthTextOffset, Quaternion.identity, canvas.transform);
            TextMeshProUGUI damageText = textGo.GetComponent<TextMeshProUGUI>();
            damageText.text = damage.ToString();
        }

        public void CharacterHealed(GameObject character, float restore)
        {
            //damageTextPrefab 스폰
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

            GameObject textGo = Instantiate(healTextPrefab, spawnPosition + healthTextOffset, Quaternion.identity, canvas.transform);
            TextMeshProUGUI healText = textGo.GetComponent<TextMeshProUGUI>();
            healText.text = restore.ToString();
        }
    }
}