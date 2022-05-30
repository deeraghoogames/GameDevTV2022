using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float spawnTime;

    [SerializeField]
    float spawnCounter;

    public GameObject spawnFX;

    [SerializeField]
    float fXTime;

    public GameObject enemyToSpawn;

    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = spawnTime;

            StartCoroutine(LoadEnemyCo());
        }
    }

    IEnumerator LoadEnemyCo()
    {
        Instantiate(spawnFX, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(fXTime);

        Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
