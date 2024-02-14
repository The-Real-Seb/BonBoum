using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    #region Singleton

    private static WaveManager instance = null;
    public static WaveManager Instance => instance;

    #endregion

    public List<GameObject> pooledEnemies;
    public GameObject objectToPool;
    public int amountToPool;

    public List<Transform> spawnPoints;

    public int waveNumber = 0;
    public int nbEnemiesAlive;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void NewWave()
    {
        waveNumber++;
        for (int i = 0; i < 5 * waveNumber; i++)
        {
            GameObject enemy = GetPooledObject();
            enemy.transform.position = GetRandomSpawnPosition();
            enemy.SetActive(true);
            nbEnemiesAlive = i;
        }

        nbEnemiesAlive++;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int randomInt = Random.Range(0, spawnPoints.Count-1);
        
        return spawnPoints[randomInt].position;
    }
    
    void Start()
    {
        pooledEnemies = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, transform);
            tmp.SetActive(false);
            pooledEnemies.Add(tmp);
            
        }
        NewWave();
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledEnemies[i].activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }
        return null;
    }

    public void EnemyGetKilled()
    {
        nbEnemiesAlive--;

        if (nbEnemiesAlive == 0)
        {
            NewWave();
        }
    }
}
