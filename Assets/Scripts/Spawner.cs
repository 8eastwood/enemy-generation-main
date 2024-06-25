using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _speed = 150f;
    [SerializeField] private Enemy _enemyPrefab;

    private ObjectPool<Enemy> _enemiesPool;
    private int _enemyPoolCapacity = 5;
    private int _enemyPoolMaxSize = 5;
    private int _repeatRate = 2;

    private void Awake()
    {
        _enemiesPool = new ObjectPool<Enemy>(CreateEnemy, GetFromPool, ReleaseInPool, Destroy, true, _enemyPoolCapacity, _enemyPoolMaxSize);
    }

    private void Start()
    {
        _spawnPoints = new List<Transform>(_spawnPoints);

        StartCoroutine(SpawnEnemyWithRate(_repeatRate));
    }

    private Enemy CreateEnemy()
    {
        int spawnPoint = Random.Range(0, _spawnPoints.Count);
        Enemy enemy = Instantiate(_enemyPrefab, _spawnPoints[spawnPoint].transform.position, Quaternion.identity);
        _spawnPoints.RemoveAt(spawnPoint);

        return enemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);
    }

    private void GetFromPool(Enemy enemy)
    {
        enemy.transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        enemy.gameObject.SetActive(true);
    }

    private void ReleaseInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
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
