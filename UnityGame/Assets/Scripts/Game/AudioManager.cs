using Unity.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake(){

        if (instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoadManager.DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = s.playOnAwake;
            s.source.clip = s.clip;         
        }
    }

    public void playSound (string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        //Debug.Log(s.source);
        s.source.Play();
    }
}
