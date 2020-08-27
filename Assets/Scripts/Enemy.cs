using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    [Header("Optional:")]
    [SerializeField]
    private StatusBar statusBar;

    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeDuration = 0.1f;

    public int moneyDrop = 10;

    public string deathSound_Name = "Explosion";

    private void Start()
    {
        stats.Init();

        if (statusBar != null)
        {
            statusBar.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
    }

    public void DamageEnemy(int damage)
    {
        stats.curHealth -= damage;

        if (stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusBar != null)
        {
            statusBar.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
        }

        if (collision.collider.CompareTag("Player"))
        {
            GameMaster.KillEnemy(this);
        }
    }
    void OnUpgradeMenuToggle(bool active)
    {
        if (this != null)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            GetComponent<EnemyAI>().enabled = !active;
        }
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
