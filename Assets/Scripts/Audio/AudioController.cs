using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioController : MonoBehaviour
{
    //** AUDIO CONTROLLER VARIABLES **//

    //Array of Sound objects that I will be using in my project.
    public Sound[] sounds;
    
    //singleton reference to Audio Controller so it can be accessed globally.
    public static AudioController instance;

    //** AWAKE METHOD **//
    void Awake()
    {
        //Testing if the Audio Controller is in the Scene or not. If it isn't create an instance of it...
        if (instance == null)
            instance = this;
        //otherwise destroy the Audio Controller so there isn't two instances of it.
        else
        {
            Destroy(gameObject);
            return;
        }

        //Prevents the AudioController game object from being destroyed on load, will always be present throughout the whole game.
        DontDestroyOnLoad(gameObject);

        //Tests that for each sound in the sounds array
        foreach (Sound s in sounds)
        {
            //tests if the Sound object is null. If it Log Error is called and it skips to next loop iteration. This is to make sure there are no consistencies in the sounds array.
            if (s == null)
            {
                Debug.LogError("A Sound in the sounds array is null!");
                continue;
            }

            //Makes any game object the audio is assigned to, a source.
            s.source = gameObject.AddComponent<AudioSource>();

            //Allows you to play the audio data stored in s.clip.
            s.source.clip = s.clip;

            //Allows you to change the volume settings of each sound in the Inspector by providing an input box to do so.
            s.source.volume = s.volume;

            //Allows you to change the pitch settings of each sound in the Inspector, this is mainly in the case of creating the occurence of the Doppler Effect in any future modifications of the game.
            s.source.pitch = s.pitch;

            //Allows you to determine whether or not a certain sound will be looped or not.
            s.source.loop = s.loop;
        }
    }

    //** START METHOD **//
    void Start()
    {
        //When the game is first loaded, play the game's Main Theme which only plays in the background of MainMenu Scene, the TimesPage Scene and the TutorialPage Scene and as MainMenu is the first openned scene, it needs to play on start.
        Play("MainTheme");
    }

    //** AUDIO CONTROL FUNCTIONS **//
    //METHOD: Plays audio when called to (audio in parameter "name")
    public void Play(string name)
    {
        //Finds the sound in sounds array that is labelled "name" which will be provided as a parameter to the method.
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //If there is no occurence of the requested sound, Debug.LogError to indicate there is no instance of such a sound of the name "name" and return.
        if (s == null)
        {
            Debug.LogError("No Sound with the name " + name + " found!");
            return;
        }

        //If an instance of a sound labelled "name" is found, play it.
        s.source.Play();
    }

    //METHOD: Stops specified audio when called to (audio in parameter "name")
    public void Stop(string name)
    {
        //Finds the sound in sounds array that is labelled "name" which will be provided as a parameter to the method.
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //If there is no occurence of the requested sound, Debug.LogError to indicate there is no instance of such a sound of the name "name" and return.
        if (s == null)
        {
            Debug.LogError("No Sound with the name " + name + " found!");
            return;
        }

        //If an instance of a sound labelled "name" is found, try to stop it.
        s.source.Stop();
    }
}
