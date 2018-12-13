using UnityEngine;
using System.Runtime.Remoting.Messaging;
public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		private int _maxHealth = 100;
		private int _curHealth = 100;
		public int damage = 10;
		public int point = 10;

		public int curHealth {
			get {
				return _curHealth;
			}
			set {
				_curHealth = (int)Mathf.Clamp(value,0f, maxHealth);
			}
		}

		public int maxHealth {
			get {
				return _maxHealth;
			}
		}

		public void Init() {
			curHealth = maxHealth;
		}
	}

    [Header("Object Constants")]
    public EnemyStats enemyStats = new EnemyStats ();

    [Header("Prefabs")]
    public GameObject prefabDeathParticle;

    [Header("Temporary Variables")]
    Player tPlayer;

    [Header("Optional")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	private bool isDying;
	void Start () {
		isDying = false;
		enemyStats.Init ();

		if(statusIndicator != null) {
			statusIndicator.SetHealth (enemyStats.curHealth, enemyStats.maxHealth);
		}
	}


	public void damageEnemy(int damageValue) {
		enemyStats.curHealth -= damageValue;

		if(enemyStats.curHealth <= 0) {
			killEnemy ();
		}

		if(statusIndicator != null) {
			statusIndicator.SetHealth (enemyStats.curHealth, enemyStats.maxHealth);
		}
	}

	public void killEnemy() {

		GameObject deathParticle = (GameObject)Instantiate (prefabDeathParticle, transform.position, transform.rotation);

		Destroy (deathParticle, 0.8f);

		Destroy (gameObject);

		GameManager.instance.totalPoints += enemyStats.point;
		GameManager.instance.changeUserInfo ();
	}


	void OnCollisionEnter2D(Collision2D col) {
		tPlayer = col.collider.GetComponent<Player> ();
		if(tPlayer != null) {
            tPlayer.damagePlayer (enemyStats.damage);
			damageEnemy (9999);
		}
	}
}
