using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    private int maxLives = 3;
    private static int remainingLives;
    public static int RemainingLives
    {
        get { return remainingLives; }
    }

    public static GameMaster gm;
    public Transform PlayerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;

    private AudioManager audioManager;

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgrageMenuCallback(bool active);
    public UpgrageMenuCallback onToggleUpgradeMenu;

    [SerializeField]
    private int startingMoney;
    public static int money;

    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }

        waveSpawner = GetComponent<WaveSpawner>();
    }

    private void Start()
    {
        remainingLives = maxLives;
        money = startingMoney;

        audioManager = AudioManager.am;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUpgradeMenu();
        }
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        remainingLives--;
        if (remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound("Spawn");

        Instantiate(PlayerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        audioManager.PlaySound(_enemy.deathSound_Name);

        audioManager.PlaySound("Money");
        money += _enemy.moneyDrop;

        GameObject particle = Instantiate(_enemy.deathParticles.gameObject, _enemy.transform.position, Quaternion.identity) as GameObject;
        Destroy(particle, 3);

        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeDuration);
        Destroy(_enemy.gameObject);
    }

    public void EndGame()
    {
        audioManager.PlaySound("GameOver");

        gameOverUI.SetActive(true);
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        waveSpawner.enabled = !upgradeMenu.activeSelf;

        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }
}
