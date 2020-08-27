using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;

    public void SetHealth(float _currentHealth, float _maxHealth)
    {
        float ratio = _currentHealth / _maxHealth;

        healthBarRect.localScale = new Vector3(ratio, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = (int)_currentHealth + "/" + (int)_maxHealth;
    }
}
