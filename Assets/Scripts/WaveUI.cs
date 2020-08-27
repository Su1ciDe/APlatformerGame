using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

    private void Update()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                Update_CountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                Update_SpawningUI();
                break;
        }

        previousState = spawner.State;
    }

    void Update_CountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
        }

        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }

    void Update_SpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveIncoming", true);
            waveAnimator.SetBool("WaveCountdown", false);

            waveCountText.text = (spawner.NextWave + 1).ToString();
        }
    }
}
