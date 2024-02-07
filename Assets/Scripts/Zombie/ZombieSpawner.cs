using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZombieSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING, FINISHED};

    // VARIABLES
    [SerializeField] private Wave[] waves;

    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float startingWaveCountdown = 0;

    public SpawnState state = SpawnState.COUNTING;

    private int currentWave;
    public AudioSource source;
    public AudioClip endRoundSound;
    public AudioClip startRoundSound;
    public AudioClip winMusic;

    // REFERENCES
    [SerializeField] private Transform[] spawners;
    [SerializeField] private List<GameObject> zombiesList;

    public Door door;

    private void Start()
    {
        startingWaveCountdown = timeBetweenWaves;
        currentWave = 0;
    }

    private void Update()
    {
        //Prevent spawning dupplication and wait for the second 
        /*if (PhotonNetwork.IsMasterClient == false || PhotonNetwork.CurrentRoom.PlayerCount != 3)
        {
            return;
        }*/
        if (state != SpawnState.FINISHED)
        {
            if (state == SpawnState.WAITING)
            {
                // Check if all zombies are dead
                if (!ZombiesAreDead())
                {
                    return;
                }
                // Wave finished
                else
                {
                    CompleteWave();
                }
            }
            if (startingWaveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave(waves[currentWave]));
                }
            }
            else
            {
                startingWaveCountdown -= Time.deltaTime;
            }
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        source.PlayOneShot(startRoundSound);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.zombiesAmount; i++)
        {
            SpawnZombie(wave.zombie);
            yield return new WaitForSeconds(wave.delay);
        }
       

        state = SpawnState.WAITING;

        yield break;
    }


    private void SpawnZombie(GameObject zombie)
    {
        int length = 5;
        if (door == null)
        {
            length = spawners.Length;
        }
        int spawner_id = Random.Range(0, length);
        Transform selectedSpawner = spawners[spawner_id];
        GameObject newZombie = PhotonNetwork.Instantiate(zombie.name, selectedSpawner.position, selectedSpawner.rotation);
        newZombie.GetComponent<Zombie>().SetHealth(3 + currentWave * 1.4f); 

        zombiesList.Add(newZombie);
    }

    private bool ZombiesAreDead()
    {
        foreach (GameObject zombie in zombiesList)
        {
            // Check if the zombie is still active
            if (zombie != null)
            {
                return false;
            }
        }
        return true;
    }

    private void CompleteWave()
    {
        Debug.Log("Wave completed");
        if (currentWave != waves.Length - 1)
        {
            source.PlayOneShot(endRoundSound);
        }
        state = SpawnState.COUNTING;
        // BREAKTIME
        startingWaveCountdown = timeBetweenWaves;

        if (currentWave == waves.Length - 1)
        {
            // Reroll the last wave
            source.PlayOneShot(winMusic);
            state = SpawnState.FINISHED;
            Debug.Log("END GAME");
        }
        else
        {
            currentWave++; 
        }
    }
}
