using UnityEngine;

namespace My2D
{
    //플레이어 이동 따른 시차효과 거리 구하기
    public class ParallaxEffect : MonoBehaviour
    {
        #region Variables
        public Camera camera;           //카메라
        public Transform followTarget;  //플레이어

        //시작 위치
        private Vector2 startingPostion;    //시작 위치 (배경, 카메라)
        private float startingZ;            //시작할때 배경의 z축 위치값

        //시작지점으로 부터의 카메라가 있는 위치까지의 거리
        private Vector2 CamMoveSinceStart => startingPostion - (Vector2)camera.transform.position;

        //배경과 플레이어와의 z축 거리
        private float zDistanceFromTarget => transform.position.z - followTarget.position.z;
        //
        private float ClippingPlane => camera.transform.position.z + (zDistanceFromTarget > 0 ? camera.farClipPlane : camera.nearClipPlane);
        //시차 거리 factor
        private float ParallaxFactor => Mathf.Abs(zDistanceFromTarget) / ClippingPlane;
        #endregion

        private void Start()
        {
            //초기화
            startingPostion = transform.position;
            startingZ = transform.position.z;
        }


        private void Update()
        {
            Vector2 newPositon = startingPostion + CamMoveSinceStart * ParallaxFactor;
            transform.position = new Vector3(newPositon.x, newPositon.y, startingZ);
        }

    }
}