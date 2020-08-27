using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    private PlayerStats playerStats;

    public int fallThreshold = -20;

    public string deathSoundName = "DeathVoice";
    public string gruntSoundName = "GruntVoice";

    private AudioManager audioManager;

    [SerializeField]
    private StatusBar statusBar;

    void Start()
    {
        playerStats = PlayerStats.playerStats;

        playerStats.curHealth = playerStats.maxHealth;

        if (statusBar != null)
        {
            statusBar.SetHealth(playerStats.curHealth, playerStats.maxHealth);
        }

        audioManager = AudioManager.am;

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        InvokeRepeating("RegenHealth", 1, 1);
        
    }

    void Update()
    {
        if (transform.position.y <= fallThreshold)
        {
            DamagePlayer(999);
        }
    }

    public void DamagePlayer(int damage)
    {
        playerStats.curHealth -= damage;

        if (playerStats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSoundName);
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(gruntSoundName);
        }

        statusBar.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    void OnUpgradeMenuToggle(bool active)
    {
        if (active)
        {
            CancelInvoke();
        }
        else
        {
            InvokeRepeating("RegenHealth", 1, 1);
        }

        GetComponent<Platformer2DUserControl>().enabled = !active;

        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.enabled = !active;
        }        
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

    void RegenHealth()
    {
        playerStats.curHealth += playerStats.healthRegeneration;
        statusBar.SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }
}
