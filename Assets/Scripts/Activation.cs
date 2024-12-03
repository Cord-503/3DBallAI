using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Activation : MonoBehaviour
{
    public UnityEvent enterActivator;
    public UnityEvent leaveActivator;

    public GameObject prefabToSpawn;
    public GameObject spawnLocation;

    public int maxSpawnCount = 5;
    public float spawnInterval = 5f;

    private int currentSpawnCount = 0;
    private Coroutine spawnCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb)
        {
            enterActivator.Invoke();

            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnPrefabRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb)
        {
            leaveActivator.Invoke();
        }
    }

    private IEnumerator SpawnPrefabRoutine()
    {
        while (currentSpawnCount < maxSpawnCount)
        {
            Instantiate(prefabToSpawn, spawnLocation.transform.position, Quaternion.identity);

            currentSpawnCount++;

            yield return new WaitForSeconds(spawnInterval);
        }

        spawnCoroutine = null;
    }

    public void ResetSpawnCount()
    {
        currentSpawnCount = 0;
    }
}