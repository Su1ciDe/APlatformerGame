using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public float maxHealth = 100;
    public float healthRegeneration = 1f;
    public float healthUpgradeMultiplier = 1.2f;
    private float _curHealth;
    public float curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public float movementSpeed = 10;
    public float movementSpeed_UpgradeMultiplier = 1.25f;

    void Awake()
    {
        if (playerStats == null)
        {
            playerStats = this;
        }
    }
}
