using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private Dictionary<string, AudioClip> _clipDict;
    private Dictionary<string, AudioSource> _sourceDict;
    private AudioSource _aSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        _aSource = GetComponent<AudioSource>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        _clipDict = new Dictionary<string, AudioClip>();
        _sourceDict = new Dictionary<string, AudioSource>();

        foreach(AudioClip clip in clips)
        {
            _clipDict.Add(clip.name, clip);

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.playOnAwake = false;
            _sourceDict.Add(clip.name, source);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(string audioName)
    {
        PlayAudio(audioName, 1f, false);
    }

    public void PlayAudio(string audioName, float volume)
    {
        PlayAudio(audioName, volume, false);
    }

    public void PlayAudio(string audioName, bool loop)
    {
        PlayAudio(audioName, 1f, loop);
    }


    public void PlayAudio(string audioName, float volume, bool loop)
    {
        try
        {
            AudioSource source = _sourceDict[audioName];
            source.volume = volume;
            source.loop = loop;
            source.Play();
        }
        catch (KeyNotFoundException ex)
        {
            Debug.LogError(ex);
        };
    }

    public AudioClip getClip(string name)
    {
        return _clipDict[name];
    }
}
