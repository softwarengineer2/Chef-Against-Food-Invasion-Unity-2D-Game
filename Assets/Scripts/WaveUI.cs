using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {
    [Header("Object Constants")]
	[SerializeField]
	WaveSpawner spawner;
	[SerializeField]
	Animator waveAnimator;
	[SerializeField]
	Text waveCountdownText;
	[SerializeField]
	Text waveCountText;

	private WaveSpawner.SpawnState previousState;

	void Start () {

		initialize ();

		if (spawner == null)
		{
			Debug.LogError("No spawner referenced!");
			this.enabled = false;
		}
		if (waveAnimator == null)
		{
			Debug.LogError("No waveAnimator referenced!");
			this.enabled = false;
		}
		if (waveCountdownText == null)
		{
			Debug.LogError("No waveCountdownText referenced!");
			this.enabled = false;
		}
		if (waveCountText == null)
		{
			Debug.LogError("No waveCountText referenced!");
			this.enabled = false;
		}
	}

	void initialize () {
		waveAnimator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		switch (spawner.state)
		{
		    case WaveSpawner.SpawnState.COUNTING:
			    UpdateCountingUI();
			    break;
		    case WaveSpawner.SpawnState.SPAWNING:
			    UpdateSpawningUI();
			    break;
		}

		previousState = spawner.state;
	}

	void UpdateCountingUI ()
	{
		if (previousState != WaveSpawner.SpawnState.COUNTING)
		{
			waveAnimator.SetBool("WaveIncoming", false);
			waveAnimator.SetBool("WaveCountdown", true);
		}
		waveCountdownText.text = ((int)spawner.waveCountDown).ToString();
	}

	void UpdateSpawningUI()
	{
		if (previousState != WaveSpawner.SpawnState.SPAWNING)
		{
			waveAnimator.SetBool("WaveCountdown", false);
			waveAnimator.SetBool("WaveIncoming", true);

			waveCountText.text = (spawner.nextWave+1).ToString();
		}
	}
}
