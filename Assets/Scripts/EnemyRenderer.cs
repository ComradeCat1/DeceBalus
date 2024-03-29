using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

[System.Serializable]
public class EnemyPositionsData
{
    public List<List<int>> positions;

    public EnemyPositionsData(List<List<int>> positions) {
        this.positions = positions;
    }
}

public class EnemyRenderer : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public string jsonFilePath;
    public float prefabSpacing = 1f;
    public float destroyDistance = 10f; // maximum distance between Balus and prefab to destroy it

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private GameObject balus;
    private Dictionary<Vector3, GameObject> prefabPositions = new Dictionary<Vector3, GameObject>();
    private GameManager gameManager;
    private string jsonString;

    private void Start()
    {
        balus = GameObject.FindGameObjectWithTag("Balus");
        gameManager = balus.GetComponent<GameManager>();
        if (gameManager.isDataDownloaded) {
            jsonString = File.ReadAllText(Application.persistentDataPath + "/" + jsonFilePath + ".json");
        } else {
            jsonString = Resources.Load<TextAsset>(jsonFilePath).text;
        }
        EnemyPositionsData data = JsonConvert.DeserializeObject<EnemyPositionsData>(jsonString);
        List<List<int>> positions = data.positions;
        for (int i = 0; i < positions.Count; i++)
        {
            List<int> row = positions[i];
            for (int j = 0; j < row.Count; j++)
            {
                int hasPrefab = row[j];
                float x = j - 2;
                float z = i * prefabSpacing;
                Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                {
                    GameObject spawnedPrefab = Instantiate(prefabs[hasPrefab], spawnPosition, Quaternion.identity);
                    spawnedPrefabs.Add(spawnedPrefab);
                    prefabPositions.Add(spawnPosition, spawnedPrefab);
                }
                /*
                if (hasPrefab == 1)
                {
                    float x = j - 2;
                    float z = i * prefabSpacing;
                    Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                    if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                    {
                        GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
                        spawnedPrefabs.Add(spawnedPrefab);
                        prefabPositions.Add(spawnPosition, spawnedPrefab);
                    }
                } else if (hasPrefab == 2) {
                    float x = j - 2;
                    float z = i * prefabSpacing;
                    Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                    if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                    {
                        GameObject spawnedPrefab = Instantiate(prefab2, spawnPosition, Quaternion.identity);
                        spawnedPrefabs.Add(spawnedPrefab);
                        prefabPositions.Add(spawnPosition, spawnedPrefab);
                    }
                } else if (hasPrefab == 3) {
                    float x = j - 2;
                    float z = i * prefabSpacing;
                    Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                    if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                    {
                        GameObject spawnedPrefab = Instantiate(prefab3, spawnPosition, Quaternion.identity);
                        spawnedPrefabs.Add(spawnedPrefab);
                        prefabPositions.Add(spawnPosition, spawnedPrefab);
                    }
                } */
            }
        }
    }

    private void Update()
    {
        EnemyPositionsData data = JsonConvert.DeserializeObject<EnemyPositionsData>(jsonString);
        List<List<int>> positions = data.positions;
        for (int i = 0; i < positions.Count; i++)
        {
            List<int> row = positions[i];
            float z = i * prefabSpacing;
            for (int j = 0; j < row.Count; j++)
            {
                int hasPrefab = row[j];
                float x = j - 2;
                Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                {
                    GameObject spawnedPrefab = Instantiate(prefabs[hasPrefab], spawnPosition, Quaternion.identity);
                    spawnedPrefabs.Add(spawnedPrefab);
                    prefabPositions.Add(spawnPosition, spawnedPrefab);
                }
                /*
                if (hasPrefab == 1)
                {
                    float x = j - 2;
                    Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                    if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                    {
                        GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
                        spawnedPrefabs.Add(spawnedPrefab);
                        prefabPositions.Add(spawnPosition, spawnedPrefab);
                    }
                } else if (hasPrefab == 2) {
                    float x = j - 2;
                    Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                    if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                    {
                        GameObject spawnedPrefab = Instantiate(prefab2, spawnPosition, Quaternion.identity);
                        spawnedPrefabs.Add(spawnedPrefab);
                        prefabPositions.Add(spawnPosition, spawnedPrefab);
                    }
                } else if (hasPrefab == 3) {
                    float x = j - 2;
                    Vector3 spawnPosition = new Vector3(x, 0.55f, z);
                    if (spawnPosition.z - balus.transform.position.z < 25 && !prefabPositions.ContainsKey(spawnPosition))
                    {
                        GameObject spawnedPrefab = Instantiate(prefab3, spawnPosition, Quaternion.identity);
                        spawnedPrefabs.Add(spawnedPrefab);
                        prefabPositions.Add(spawnPosition, spawnedPrefab);
                    }
                } */
            }
        }
        
        

        for (int i = spawnedPrefabs.Count - 1; i >= 0; i--)
        {
            GameObject spawnedPrefab = spawnedPrefabs[i];
            if (spawnedPrefab == null)
            {
                spawnedPrefabs.RemoveAt(i);
                continue;
            }

            if (balus.transform.position.z - spawnedPrefab.transform.position.z >= destroyDistance)
            {
                Destroy(spawnedPrefab);
                spawnedPrefabs.RemoveAt(i);
            }
        }
    }

    public void UpdateData() {
        jsonString = File.ReadAllText(Application.persistentDataPath + "/" + jsonFilePath + ".json");
    }

    public void clearPrefabPositions() {
        prefabPositions.Clear();
        spawnedPrefabs.Clear();
    }
}