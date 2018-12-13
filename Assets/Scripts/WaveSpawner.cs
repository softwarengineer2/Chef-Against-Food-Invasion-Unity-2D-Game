using UnityEngine;
using System.Collections;
public class WaveSpawner : MonoBehaviour {

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform[] enemy;
		public int count;
		public float rate;
	}

    [Header("Object Constants")]
    public Transform enemySpawnRoot;
    public Transform[] spawnPoints;
    public int totalWave = 100;
    public float timeBetweenWaves = 5f;
    public float waveCountDown;
    private float searchCountdown = 1f;
    public SpawnState state = SpawnState.COUNTING;
    public int nextWave = 0;
    private Wave[] waves;

    [Header("Temporary Variables")]
    private Transform tSpawn;

    void Start()
	{
		initialize ();

		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

        waveCountDown = timeBetweenWaves;
	}

	void initialize() {

        waveCountDown = timeBetweenWaves;

		spawnPoints = new Transform[enemySpawnRoot.childCount];
		for(int i=0;i<enemySpawnRoot.childCount;i++) {
			spawnPoints[i] = enemySpawnRoot.GetChild(i);
		}


        waves = new Wave[totalWave];

        for(int i=0;i< totalWave; i++)
        {
            waves[i] = new Wave();
            waves[i].count = i + 1;
            waves[i].name = "Level " + (i+1).ToString();
            waves[i].rate = 1.0f;

            waves[i].enemy = new Transform[GameManager.instance.enemyPrefabs.Length];   

            for(int j=0;j< GameManager.instance.enemyPrefabs.Length;j++) 
            {
                waves[i].enemy[j] = GameManager.instance.enemyPrefabs[j].transform;
            }
        }
	}

	void Update()
	{
		if(GameManager.instance.isDead != true && GameManager.instance.isPaused != true) {
			if (state == SpawnState.WAITING)
			{
				if (!EnemyIsAlive())
				{
					WaveCompleted();
				}
				else
				{
					return;
				}
			}

			if (waveCountDown <= 0)
			{
				if (state != SpawnState.SPAWNING && totalWave > 0)
				{
                    GameManager.instance.changeLevelInfo(nextWave+1);
					StartCoroutine( SpawnWave ( waves[nextWave] ) );
				}
			}
			else
			{
				waveCountDown -= Time.deltaTime;
			}
		}
	}

	void WaveCompleted()
	{
		//Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			//Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		//Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy(_wave.enemy[Random.Range(0, _wave.enemy.Length)]);
			yield return new WaitForSeconds( 1f/_wave.rate );
		}

		state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		//Debug.Log("Spawning Enemy: " + _enemy.name);

        tSpawn = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate(_enemy, tSpawn.position, tSpawn.rotation);
	}

}
