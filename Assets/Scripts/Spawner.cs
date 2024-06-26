using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] public List<Transform> _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;

    private ObjectPool<Enemy> _enemiesPool;
    private int _enemyPoolCapacity = 25;
    private int _enemyPoolMaxSize = 25;
    private int _repeatRate = 2;

    private void Awake()
    {
        _enemiesPool = new ObjectPool<Enemy>(CreateEnemy, GetFromPool, ReleaseInPool, Destroy, true, _enemyPoolCapacity, _enemyPoolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemyWithRate(_repeatRate));
    }

    private Enemy CreateEnemy()
    {
        float randomYAngle = Random.Range(0f, 360f);
        Enemy enemy = Instantiate(_enemyPrefab, GetSpawnPoint().transform.position, Quaternion.Euler(0f, randomYAngle, 0f));

        return enemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);
    }

    private void GetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);

        StartCoroutine(enemy.Move());
    }

    private void ReleaseInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private Transform GetSpawnPoint()
    {
        int spawnPoint = Random.Range(0, _spawnPoints.Count);

        return _spawnPoints[spawnPoint];
    }

    private IEnumerator SpawnEnemyWithRate(int repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (true)
        {
            yield return wait;

            GetEnemy();
        }
    }

    private void GetEnemy()
    {
        Enemy newEnemy = _enemiesPool.Get();
    }
}
