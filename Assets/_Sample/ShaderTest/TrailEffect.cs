using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class TrailEffect : MonoBehaviour
    {
        #region Variables
        //플레이어 잔상 효과 : 2초동안 잔상효과 발생
        private bool isTrailActive = false;
        [SerializeField] private float trailActiveTime = 2f;        //잔상 효과 유효 시간
        [SerializeField] private float trailRefreshRate = 0.1f;     //잔상들의 발생 간격
        [SerializeField] private float trailDestroyDelay = 1f;      //1후에 킬 -> 페이드 아웃

        private SpriteRenderer playerRenderer;

        public Material ghostMaterial;      //잔상 메터리얼
        [SerializeField] private string shaderValueRef = "_Alpha";
        [SerializeField] private float shaderValueRate = 0.1f;      //알파값 감소 비율
        [SerializeField] private float shaderValueRefreshRate = 0.05f; //알파값 감소되는 시간 간격
        #endregion

        private void Awake()
        {
            playerRenderer = GetComponent<SpriteRenderer>();
        }

        //2초동안 잔상효과 발생
        public void StartActiveTrail()
        {
            if (isTrailActive) return;

            isTrailActive = true;
            StartCoroutine(ActiveTrail(trailActiveTime));
        }

        //activTime동안 잔상효과 발생
        IEnumerator ActiveTrail(float activTime)
        {
            while (activTime > 0)
            {
                activTime -= trailRefreshRate;

                //잔상 만들기 - 현재 위치에
                GameObject ghostObejct = new GameObject();  //하이라키창에 빈 오브젝트 만들기
                //트랜스폼 셋팅
                ghostObejct.transform.SetPositionAndRotation(transform.position, transform.rotation); 
                ghostObejct.transform.localScale = transform.localScale;
                //SpriteRenderer 셋팅
                SpriteRenderer renderer = ghostObejct.AddComponent<SpriteRenderer>();
                renderer.sprite = playerRenderer.sprite;
                renderer.sortingLayerName = playerRenderer.sortingLayerName;
                renderer.sortingOrder = playerRenderer.sortingOrder - 1;
                renderer.material = ghostMaterial;

                //메터리얼속성(알파값) 감소
                StartCoroutine(AnimateMaterialFloat(renderer.material, shaderValueRef, 0f, shaderValueRate, shaderValueRefreshRate));

                Destroy(ghostObejct, trailDestroyDelay);

                yield return new WaitForSeconds(trailRefreshRate);
            }
            isTrailActive = false;
        }

        //메터리얼속성(알파값) 감소
        IEnumerator AnimateMaterialFloat(Material mat, string valueRef, float goal, float rate, float refreshRate)
        {
            float valueToAminate = mat.GetFloat(valueRef);

            while (valueToAminate > goal)
            {
                valueToAminate -= rate;
                mat.SetFloat(valueRef, valueToAminate);
                yield return new WaitForSeconds(refreshRate);
            }
        }

    }
}