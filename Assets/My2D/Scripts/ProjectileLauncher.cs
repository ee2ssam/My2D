using UnityEngine;

namespace My2D
{
    //발사체를 발사하는 클래스
    public class ProjectileLauncher : MonoBehaviour
    {
        #region Vaiables
        //발사체 프리팹
        public GameObject projectilePrefab;
        //발사 위치
        public Transform firePoint;
        #endregion

        #region Custom Method
        //발사체 발사
        public void FireProjectile()
        {
            //Debug.Log("발사체를 발사위치에서 발사하다");
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectilePrefab.transform.rotation);

            Vector3 oringScale = projectile.transform.localScale;
            //캐릭터의 발사 방향으로 화살의 앞 방향 설정
            projectile.transform.localScale = new Vector3(
                oringScale.x * transform.parent.localScale.x > 0 ? 1 : -1,
                oringScale.y,
                oringScale.z
                );

            //발사체를 3초후에 킬
            Destroy(projectile, 3f);
        }
        #endregion
    }
}
