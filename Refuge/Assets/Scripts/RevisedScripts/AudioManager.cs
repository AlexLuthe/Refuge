using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChannel {

    public string name;
    public List<AudioSource> sources = new List<AudioSource>();
    public float volume = 1;
    public GameObject gameObject;

    public AudioChannel(string name) {
        this.name = name;
    }

    public void Add(AudioSource source) {
        sources.Add(source);
    }

    public void Remove(AudioClip clip) {
        foreach (AudioSource s in sources) {
            if (s == null)
                Debug.Log("Shits fucked yo");
            if (s.clip == clip) {
                sources.Remove(s);
                Object.Destroy(s.gameObject);
                return;
            }
        }
    }

    public void Stop() {
        foreach (AudioSource s in sources)
            Object.Destroy(s.gameObject);
        sources.Clear();
    }
}

public class AudioManager : MonoBehaviour {

    // Singleton
    private static AudioManager _Instance;
    public static AudioManager Instance {
        get {
            if (_Instance == null)
                if (GameObject.Find("GameManager").GetComponent<GameManager_r>()._AudioManager == null) {
                    _Instance = new GameObject().AddComponent<AudioManager>();
                    GameObject.Find("GameManager").GetComponent<GameManager_r>()._AudioManager = _Instance;
                }
                else
                    _Instance = GameObject.Find("GameManager").GetComponent<GameManager_r>()._AudioManager;
            return _Instance;
        }
    }

    public List<AudioChannel> channels = new List<AudioChannel>();
    public float masterVolume = 1;
    public AudioClip clickSound;
    public AudioClip BGM;

    bool fadeClip;
    AudioChannel fadeChannel;
    float fadeInterval = 0;
    float fadeDuration;

    void Update() {
        if (fadeClip && fadeChannel != null) {
            bool done = true;
            foreach (AudioSource s in fadeChannel.sources) {
                s.volume = Mathf.Lerp(s.volume, 0, fadeInterval);
                fadeInterval += Time.deltaTime * (1 / fadeDuration);
                if (s.volume > 0) {
                    done = false;
                }
                else
                    fadeInterval = 0;
            }
            if (done) {
                fadeChannel.Stop();
                fadeClip = false;
                fadeInterval = 0;
                fadeChannel = null;
            }
        }

    }

    public void CreateChannel(string name) {
        AudioChannel channel = new AudioChannel(name);
        channels.Add(channel);
    }

    public AudioChannel GetChannel(string name) {
        foreach (AudioChannel channel in channels)
            if (channel.name == name)
                return channel;
        return null;
    }

    public void PlayClip(AudioClip clip, AudioChannel channel, float volume = 1, bool loop = false) {
        channel.gameObject = new GameObject();
        channel.gameObject.AddComponent<AudioSource>();
        AudioSource source = channel.gameObject.GetComponent<AudioSource>();
        channel.Add(source);
        source.clip = clip;
        source.loop = loop;
        source.volume = masterVolume * volume * channel.volume;
        source.Play();
        if (!loop)
            Destroy(channel.gameObject, clip.length);
    }

    public void PlayClip(AudioClip clip, string channel, float volume = 1, bool loop = false) {
        PlayClip(clip, GetChannel(channel), volume, loop);
    }

    public void StopChannel(AudioChannel channel, bool fade = false, float duration = 0) {
        if (fade) {
            fadeChannel = channel;
            fadeClip = true;
            fadeDuration = duration;
            fadeInterval = 0;
        }
        else
            channel.Stop();
    }

    public void StopChannel(string channel, bool fade = false, float duration = 0) {
        if (fade) {
            fadeChannel = GetChannel(channel);
            fadeClip = true;
            fadeDuration = duration;
            fadeInterval = 0;
        }
        else
            GetChannel(channel).Stop();
    }

    public void StopClip(AudioClip clip) {
        foreach (AudioChannel channel in channels) {
            channel.Remove(clip);
        }
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        CreateChannel("SFX");
        CreateChannel("SFX2");
        CreateChannel("SFX3");
        CreateChannel("Music");
        CreateChannel("Music2");
        CreateChannel("Music3");
        CreateChannel("Ambient");
        CreateChannel("Ambient2");
        CreateChannel("Ambient3");
    }
}
