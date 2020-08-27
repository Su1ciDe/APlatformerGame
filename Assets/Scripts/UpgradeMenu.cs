using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private int healthUpgradeCost = 50;

    [SerializeField] private Text speedText;
    [SerializeField] private int speedhUpgradeCost = 50;

    private PlayerStats stats;

    private void OnEnable()
    {
        stats = PlayerStats.playerStats;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "HEALTH: " + ((int)stats.maxHealth).ToString();
        speedText.text = "SPEED: " + ((int)stats.movementSpeed).ToString();
    }

    public void UpdgradeHealth()
    {
        if (GameMaster.money<healthUpgradeCost)
        {
            return;
        }

        stats.maxHealth *= stats.healthUpgradeMultiplier;

        GameMaster.money -= healthUpgradeCost;
        healthUpgradeCost *= 2;

        UpdateValues();
    }

    public void UpdgradeSpeed()
    {
        if (GameMaster.money < healthUpgradeCost)
        {
            return;
        }

        stats.movementSpeed *= stats.movementSpeed_UpgradeMultiplier;

        GameMaster.money -= speedhUpgradeCost;
        speedhUpgradeCost *= 2;

        UpdateValues();
    }
}
