using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private MeshCollider _spawnPoint1;
    [SerializeField] private MeshCollider _spawnPoint2;
    [SerializeField] private MeshCollider _spawnPoint3;
    [SerializeField] private MeshCollider _spawnPoint4;

    private ObjectPool<Enemy> _enemyPool;
    private int _enemyPoolCapacity = 5;
    private int _enemyPoolMaxSize = 5;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, GetFromPool, ReleaseInPool, Destroy, true, _enemyPoolCapacity, _enemyPoolMaxSize);
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab);

        return enemy;
    }

    private void GetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void ReleaseInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

}
