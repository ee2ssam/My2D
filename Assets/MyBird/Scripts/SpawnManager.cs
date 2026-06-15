using UnityEngine;

namespace MyBird
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject pipePrefab;

        [Header("Timing")]
        [SerializeField] private float minInterval = 0.95f;
        [SerializeField] private float maxInterval = 1.05f;

        [Header("Height Range")]
        [SerializeField] private float minY = -1.5f;
        [SerializeField] private float maxY = 3f;

        [Header("Parenting")]
        [SerializeField] private Transform spawnParent;

        private bool spawning = false;

        private void Start()
        {
            if (pipePrefab == null)
            {
                Debug.LogError("SpawnManager: pipePrefab is not assigned.");
                return;
            }

            StartSpawning();
        }

        public void StartSpawning()
        {
            if (spawning) return;
            spawning = true;
            StartCoroutine(SpawnLoop());
        }

        public void StopSpawning()
        {
            spawning = false;
            StopAllCoroutines();
        }

        private System.Collections.IEnumerator SpawnLoop()
        {
            while (spawning)
            {
                float wait = Random.Range(minInterval, maxInterval);
                yield return new WaitForSeconds(wait);

                SpawnPipe();
            }
        }

        private void SpawnPipe()
        {
            float y = Random.Range(minY, maxY);
            Vector3 spawnPos = transform.position;
            spawnPos.y = y;

            GameObject go = Instantiate(pipePrefab, spawnPos, Quaternion.identity);
            if (spawnParent != null)
            {
                go.transform.SetParent(spawnParent, true);
            }
        }
    }
}