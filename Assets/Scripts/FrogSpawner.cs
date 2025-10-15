using UnityEngine;
using System.Collections.Generic;

public class FrogSpawner : MonoBehaviour
{
    [SerializeField] GameObject frogPrefab;
    [SerializeField] int maxFrogs = 5;
    [SerializeField] float spawnInterval = 2f;

    public static FrogSpawner Instance;
    private List<GameObject> activeFrogs = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Check if prefab is assigned
        if (frogPrefab == null)
        {
            Debug.LogError("FrogPrefab is not assigned in the Inspector!");
            return;
        }

        // Spawn initial frogs
        for (int i = 0; i < maxFrogs; i++)
        {
            SpawnFrog();
        }
        
        // Start continuous spawning
        InvokeRepeating("TrySpawnFrog", spawnInterval, spawnInterval);
    }

    void TrySpawnFrog()
    {
        // Only spawn new frog if we're under the limit
        if (activeFrogs.Count < maxFrogs)
        {
            SpawnFrog();
        }
    }

    public void SpawnFrog()
    {
        if (frogPrefab == null)
        {
            Debug.LogError("Cannot spawn frog - prefab is not assigned!");
            return;
        }

        // Get camera bounds
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        
        // Spawn within camera view with padding
        float x = Random.Range(-cameraWidth + 1f, cameraWidth - 1f);
        float y = Random.Range(-cameraHeight + 1f, cameraHeight - 1f);
        Vector3 spawnPosition = new Vector3(x, y, 0);

        // Instantiate frog
        GameObject frog = Instantiate(frogPrefab, spawnPosition, Quaternion.identity);

        // Ensure it has a collider
        if (frog.GetComponent<Collider2D>() == null)
        {
            frog.AddComponent<BoxCollider2D>();
        }

        // Random scale
        float scale = Random.Range(0.5f, 1.5f);
        frog.transform.localScale = new Vector3(scale, scale, 1);

        // Random rotation
        float angle = Random.Range(0f, 360f);
        frog.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Add to active frogs list
        activeFrogs.Add(frog);
    }

    public void RemoveFrog(GameObject frog)
    {
        if (activeFrogs.Contains(frog))
        {
            activeFrogs.Remove(frog);
        }
    }

    public void ClearAllFrogs()
    {
        foreach (GameObject frog in activeFrogs)
        {
            if (frog != null)
                Destroy(frog);
        }
        activeFrogs.Clear();
    }
}