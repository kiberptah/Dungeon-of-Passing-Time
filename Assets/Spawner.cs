using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform objectsToSpawnParent;

    List<GameObject> objectsToSpawn = new List<GameObject>();
    List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < objectsToSpawnParent.childCount; ++i)
        {
            objectsToSpawn.Add(objectsToSpawnParent.GetChild(i).gameObject);
            objectsToSpawnParent.GetChild(i).gameObject.SetActive(false);
        }

        
    }
    public void Spawn()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            GameObject spawnedObject = Instantiate(obj, transform);
            spawnedObject.SetActive(true);
            spawnedObjects.Add(spawnedObject);


        }
    }
    public void Despawn()
    {
        foreach(GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
    }
}
