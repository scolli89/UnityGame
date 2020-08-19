using Unity.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    //private static Dictionary<string, float> soundTimerDictionary;

    public static AudioManager instance;

    private bool pitchDown = false;

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

        // soundTimerDictionary = new Dictionary<string, float>();
        // resetSoundTimers();
    }

    // private void resetSoundTimers(){
    //     soundTimerDictionary["Footstep (base)"] = 0;
    //     soundTimerDictionary["Footstep (gravel)"] = 0;
    //     soundTimerDictionary["Footstep (bridge)"] = 0;
    // }

    public void playSound (string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }

        s.source.volume = s.volume;
        // give a random
        if (s.name == "Footstep (base)" || s.name == "Footstep (gravel)" || s.name == "Footstep (bridge)"){
            s.source.pitch = s.pitch + UnityEngine.Random.Range (-0.4f, 0.4f);
        }else{
            s.source.pitch = s.pitch;
        }

        //if (CanPlaySound (s)){
        s.source.Play();
        //}
    }

    // added this method to create a delay in between footstep sounds
    // private static bool CanPlaySound(Sound sound){
    //     string name = sound.name;
    //     if (soundTimerDictionary.ContainsKey(name)){
    //         float lastTimePlayed = soundTimerDictionary[name];
    //         float playerMoveTimerMax = 0.33f;
    //         if (lastTimePlayed + playerMoveTimerMax < Time.time) {
    //             soundTimerDictionary[name] = Time.time;
    //             return true;
    //         }
    //         else{
    //             return false;
    //         }
    //     } else {
    //         return true;
    //     }
    // }
}