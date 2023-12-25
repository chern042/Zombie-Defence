using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class WaveController : MonoBehaviour
{


	[SerializeField]
	private SpawnPath path;

    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private float waveTextDuration = 2f;

    [SerializeField]
    private float waveTextFadeSpeed = 3f;

    [SerializeField]
    private float zombieKillCheckSpeed = 15f;

    [SerializeField]
    private GameObject zombiePrefab;

    private List<GameObject> instantiatedZombies = new List<GameObject>();


    private Vector3 spawnPoint;
	private int waveCount;
    private int prevWave;
    private int zombieCount;
    private int zombieSpawnCount;
    private float zombieKillCheckTimer;
	private float waveLength; //Duration that zombies get spawn over. Wave ends when they are all dead.
    private float waveLengthTimer;
    private float waveTextDurationTimer;

	
	// Use this for initialization
	void Start()
	{
		waveCount = 1;
        prevWave = 1;
        spawnPoint = new Vector3(path.spawnWaypoints[0].position.x, path.spawnWaypoints[0].position.y, (path.spawnWaypoints[0].position.z + path.spawnWaypoints[1].position.z) / 2);
        waveTextDurationTimer = 0;
        waveLength = 60f* Mathf.Pow(waveCount, 0.5f);
        zombieCount = Mathf.RoundToInt(4f * Mathf.Pow(waveCount, 0.875f));
        waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 1f);
        waveText.text = "WAVE " + waveCount;
        waveLengthTimer = 0;
        zombieSpawnCount = 0;
        zombieKillCheckTimer = 0;


    }

    private void FadeWaveTextIn()
    {
        float tempAlpha = waveText.color.a;
        tempAlpha += Time.deltaTime * waveTextFadeSpeed;
        waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, Mathf.Clamp01(tempAlpha));
    }

    private void FadeWaveTextOut()
    {
        float tempAlpha = waveText.color.a;
        tempAlpha -= Time.deltaTime * waveTextFadeSpeed;
        waveText.color = new Color(waveText.color.r, waveText.color.g, waveText.color.b, Mathf.Clamp01(tempAlpha));
    }

    // Update is called once per frame
    void Update()
	{
        if (waveText.color.a >= 0.99f)
        {
            waveTextDurationTimer += Time.deltaTime;
            if (waveTextDurationTimer > waveTextDuration)
            {
                FadeWaveTextOut();
            }
        }
        if(prevWave != waveCount)
        {
            spawnPoint = new Vector3(path.spawnWaypoints[0].position.x, path.spawnWaypoints[0].position.y, (path.spawnWaypoints[0].position.z + path.spawnWaypoints[1].position.z) / 2);
            waveTextDurationTimer = 0;
            waveLength = 60f * Mathf.Pow(waveCount, 0.5f);
            zombieCount = Mathf.RoundToInt(4f * Mathf.Pow(waveCount, 0.875f));
            FadeWaveTextIn();
            waveText.text = "WAVE " + waveCount;
            prevWave = waveCount;
            zombieSpawnCount = 0;
            waveLengthTimer = 0;

        }
        else
        {
            float timePerZombie = waveLength / zombieCount;
            waveLengthTimer += Time.deltaTime;
            zombieKillCheckTimer += Time.deltaTime;
            if((waveLengthTimer >= timePerZombie)&&(zombieSpawnCount != zombieCount))
            {
                Debug.Log("ZOMBIE BOUTTA BE SPAWNED");
                GameObject zombie = Instantiate(zombiePrefab, spawnPoint, Quaternion.identity);
                Debug.Log("ZOMBIE MADE: " + zombie.name);
                Debug.Log("ZOMBIE MADE: " + zombie);

                instantiatedZombies.Add(zombie);
                spawnPoint = new Vector3(path.spawnWaypoints[0].position.x, path.spawnWaypoints[0].position.y, (path.spawnWaypoints[0].position.z + path.spawnWaypoints[1].position.z) / 2);
                waveLengthTimer = 0;
                zombieSpawnCount++;
                Debug.Log("ZOMBIE SPAWNED COUNT: "+zombieSpawnCount);

            }
            if (zombieKillCheckTimer >= zombieKillCheckSpeed)
            {
                bool allDead = true;
                foreach(GameObject zombie in instantiatedZombies)
                {
                    Debug.Log("ZOMBIE DEAD CHECK: " + zombie);
                    if(zombie != null)
                    {
                        allDead = false;
                    }
                }
                if (allDead)
                {
                    waveCount++;
                }
                zombieKillCheckTimer = 0;
            }

        }
    }
}

