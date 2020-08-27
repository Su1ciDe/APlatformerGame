using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCounterUI : MonoBehaviour
{
    private Text livesText;

    // Start is called before the first frame update
    void Awake()
    {
        livesText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = "LIVES: " + GameMaster.RemainingLives;
    }
}
