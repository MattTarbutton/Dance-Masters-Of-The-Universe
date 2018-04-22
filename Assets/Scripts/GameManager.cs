using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemySpawnerPrefab;
    public GameObject bigEnemySpawnerPrefab;

    public GameObject gameOverScreen;
    public GameObject instructionsScreen;
    public GameObject player;
    public float spawnerSpawnTime;
    public float bigSpawnerSpawnTime;
    public int maxSpawners;
    public int maxBigSpawners;
    private float timeToNextSpawn;
    private float timeToNextBigSpawn;
    private int numberOfSpawners;
    private int numberOfBigSpawners;
    private bool gameOver;
    private bool displayingInstructions;
    private int score;

    public event System.EventHandler OnScoreChanged;

    // Use this for initialization
    void Start ()
    {
        timeToNextSpawn = spawnerSpawnTime / 2.0f;
        timeToNextBigSpawn = bigSpawnerSpawnTime / 2.0f;

        player.GetComponent<PlayerController>().OnActorKilled += PlayerKilled;
        displayingInstructions = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (displayingInstructions && UnityEngine.Rendering.SplashScreen.isFinished)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"))
            {
                displayingInstructions = false;
                instructionsScreen.SetActive(false);
            }
        }
        else if (!gameOver)
        {
            if (timeToNextSpawn <= 0 && numberOfSpawners < maxSpawners)
            {
                timeToNextSpawn = spawnerSpawnTime;
                GameObject newSpawner = Instantiate(enemySpawnerPrefab);
                newSpawner.GetComponent<ActorController>().OnActorKilled += ChildKilled;
                newSpawner.GetComponent<EnemySpawner>().OnSpawnKilled += SpawnKilled;
                newSpawner.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                numberOfSpawners++;
            }
            else
            {
                timeToNextSpawn -= Time.deltaTime;
            }

            if (timeToNextBigSpawn <= 0 && numberOfBigSpawners < maxBigSpawners)
            {
                timeToNextBigSpawn = bigSpawnerSpawnTime;
                GameObject newSpawner = Instantiate(bigEnemySpawnerPrefab);
                newSpawner.GetComponent<ActorController>().OnActorKilled += BigChildKilled;
                newSpawner.GetComponent<EnemySpawner>().OnSpawnKilled += SpawnKilled;
                newSpawner.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                numberOfBigSpawners++;
            }
            else
            {
                timeToNextBigSpawn -= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ChildKilled(object sender, System.EventArgs e)
    {
        numberOfSpawners--;
    }

    private void BigChildKilled(object sender, System.EventArgs e)
    {
        numberOfBigSpawners--;
    }

    private void PlayerKilled(object sender, System.EventArgs e)
    {
        //Game over
        gameOverScreen.SetActive(true);
        gameOver = true;
    }

    private void SpawnKilled(object sender, System.EventArgs e)
    {
        score += (e as EventArgs<int>).Value;
        if (OnScoreChanged != null)
            OnScoreChanged(this, new EventArgs<int>(score));
    }
}
