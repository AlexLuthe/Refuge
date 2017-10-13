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
}

public class AudioManager : MonoBehaviour {

    // Singleton
    public static AudioManager _Instance;
    public static AudioManager Instance {
        get {
            if (_Instance == null)
                _Instance = new AudioManager();
            return _Instance;
        }
    }

    public List<AudioChannel> channels = new List<AudioChannel>();
    public float masterVolume = 1;
    public AudioClip clickSound;
    public AudioClip BGM;

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

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        CreateChannel("SFX");
        CreateChannel("Music");
        CreateChannel("Ambient");
	}
}
