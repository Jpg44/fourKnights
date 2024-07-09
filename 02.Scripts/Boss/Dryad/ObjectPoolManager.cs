using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;  // 싱글톤 인스턴스

    public GameObject missilePrefab;  // 미사일 프리팹
    private Queue<GameObject> missilePool = new Queue<GameObject>();  // 미사일 풀

    public GameObject vineCubePrefab;  // Vine Cube 프리팹
    private Queue<GameObject> vineCubePool = new Queue<GameObject>();  // Vine Cube 풀

    public GameObject entanglePrefab;  // Vine Cube 프리팹
    private Queue<GameObject> entanglePool = new Queue<GameObject>();  // Vine Cube 풀

    public int poolSize = 20;  // 각 풀의 초기 크기

    void Awake()
    {
        Instance = this;
        InitializeMissilePool();
        InitializeVineCubePool();
        InitializeEntanglePool();
    }

    void InitializeMissilePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject missile = Instantiate(missilePrefab);
            missile.SetActive(false);
            missilePool.Enqueue(missile);
        }
    }

    void InitializeVineCubePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject vineCube = Instantiate(vineCubePrefab);
            vineCube.SetActive(false);
            vineCubePool.Enqueue(vineCube);
        }
    }

    void InitializeEntanglePool()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject entangle = Instantiate(entanglePrefab);
            entangle.SetActive(false);
            entanglePool.Enqueue(entangle);
        }
    }

    public GameObject GetMissile()
    {
        return GetPooledObject(missilePool, missilePrefab);
    }

    public GameObject GetVineCube()
    {
        return GetPooledObject(vineCubePool, vineCubePrefab);
    }

    public GameObject GetEntangle()
    {
        return GetPooledObject(entanglePool, entanglePrefab);
    }

    private GameObject GetPooledObject(Queue<GameObject> pool, GameObject prefab)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnMissile(GameObject missile)
    {
        ReturnPooledObject(missile, missilePool);
    }

    public void ReturnVineCube(GameObject vineCube)
    {
        ReturnPooledObject(vineCube, vineCubePool);
    }

    public void ReturnEntangle(GameObject entangle)
    {
        ReturnPooledObject(entangle, entanglePool);
    }

    private void ReturnPooledObject(GameObject obj, Queue<GameObject> pool)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
