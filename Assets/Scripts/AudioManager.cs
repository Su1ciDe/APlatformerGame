using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;

    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2, randomPitch / 2));
        source.volume = volume * (1 + Random.Range(-randomVolume / 2, randomVolume / 2));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;

    [SerializeField]
    private Sound[] sounds;

    private void Awake()
    {
        if (am == null)
        {
            am = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (am != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        PlaySound("LevelMusic");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
}