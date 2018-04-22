using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ActorController
{
    public GameObject enemyPrefab;
    public GameObject spawnEffectsPrefab;
    public float maxEnemies;
    public int enemyWaveSize;
    public float timeTillNextWave;
    public Vector2 maxOffsetSize;
    private GameObject[] spawnEffects;
    private float timeSinceLastWave;
    private int enemies;
    public int enemyScoreValue;
    public int spawnerScoreValue;

    public event System.EventHandler OnSpawnKilled;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        timeSinceLastWave = 0;
        spawnEffects = new GameObject[enemyWaveSize];
        for (int i = 0; i < enemyWaveSize; i++)
        {
            spawnEffects[i] = Instantiate(spawnEffectsPrefab);
            spawnEffects[i].GetComponent<ParticleSystem>().Stop();
        }

        StartCoroutine(FadeIn(0.5f));
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (timeSinceLastWave >= timeTillNextWave && enemies < maxEnemies)
        {
            for (int i = 0; i < enemyWaveSize; i++)
            {
                float xOffset = Random.Range(-maxOffsetSize.x, maxOffsetSize.x);
                float yOffset = Random.Range(-maxOffsetSize.x, maxOffsetSize.y);
                GameObject newObject = Instantiate(enemyPrefab, this.transform.position + new Vector3(xOffset, yOffset), Quaternion.identity);
                newObject.GetComponent<ActorController>().OnActorKilled += ChildKilled;
                spawnEffects[i].transform.position = this.transform.position + new Vector3(xOffset, yOffset);
                spawnEffects[i].GetComponent<ParticleSystem>().Play();
                enemies++;
            }
            timeSinceLastWave = 0;
        }
        else
        {
            timeSinceLastWave += Time.deltaTime;
        }
	}

    public override void OnTakeDamage(Vector3 damagerPosition, bool activateHitStun)
    {
        this.GetComponent<HealthComponent>().SetInvulnerable(0.4f);
        
        StartCoroutine(Flash(0.5f, 3, Color.white));
    }

    public override void DestroyByDamage(Vector3 damagerPosition)
    {
        this.GetComponent<HealthComponent>().SetInvulnerable(0.5f);

        // Count spawned enemies with the spawner so we get points for them even if they die after the spawner dies (its cheating but shhh)
        SpawnKilled(spawnerScoreValue + enemies * enemyScoreValue);
        
        StartCoroutine(Flash(0.3f, 2, Color.white));
        StartCoroutine(FadeOut(0.5f));
    }

    private void ChildKilled(object sender, System.EventArgs e)
    {
        enemies--;
        SpawnKilled(enemyScoreValue);
    }

    private void SpawnKilled(int value)
    {
        if (OnSpawnKilled != null)
            OnSpawnKilled(this, new EventArgs<int>(value));
    }
}
