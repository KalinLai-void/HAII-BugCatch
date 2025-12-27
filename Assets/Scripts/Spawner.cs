using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> bugList = new List<GameObject>();
    [SerializeField] private List<Transform> spawnPointList = new List<Transform>();

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> spawnBugSoundList = new List<AudioClip>();

    [SerializeField] private float minSecToSpawn = 1f;
    [SerializeField] private float maxSecToSpawn = 5f;

    private float nextTimeToSpawn;
    private float time;

    private void Start()
    {
        nextTimeToSpawn = Random.Range(minSecToSpawn, maxSecToSpawn);

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            spawnPointList.Add(gameObject.transform.GetChild(i));
        }
    }

    private void Update()
    {
        if (GameManager.instance.isPaused) return;

        time += Time.deltaTime;
        if (time >= nextTimeToSpawn)
        {
            time = 0;
            nextTimeToSpawn = Random.Range(minSecToSpawn, maxSecToSpawn);
            Spawn();
        }
    }

    private void Spawn()
    {
        try
        {
            int index = Random.Range(0, bugList.Count);

            GameObject bug = bugList[index];
            Transform spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)];

            if (spawnPoint.childCount == 0)
            {
                Instantiate(bug, spawnPoint);
                audioSource.PlayOneShot(spawnBugSoundList[index]);
                Debug.Log("[Spawn] Spawn " + bug.tag);
            }
            else Spawn();
        }
        catch { }
    }
}
