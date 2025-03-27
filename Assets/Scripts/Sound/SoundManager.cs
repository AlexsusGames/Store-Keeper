using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioClip[] audioClips;
    [Header("Steps")]
    [SerializeField] private AudioSource stepsSource;
    [SerializeField] private AudioSource cartSource;
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip menuMusic;

    [SerializeField] private float walkingPitch;
    [SerializeField] private float runningPitch;

    private void Awake()
    {
        Core.Sound = this;
        Core.Camera.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(CameraType state)
    {
        AudioClip clip = state == CameraType.MainMenuCamera
            ? menuMusic
            : gameMusic;

        if(clip != musicSource.clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void EnableStepsSound(bool isWalking, bool hasCart, bool isRunning = false)
    {
        float pitch = isRunning ? runningPitch : walkingPitch;
        stepsSource.enabled = isWalking;
        stepsSource.pitch = pitch;

        if(isWalking && hasCart)
        {
            cartSource.enabled = true;
        }
        else cartSource.enabled = false;
    }

    public void PlayClip(AudioType type)
    {
        var source = GetFreeSource();
        var clip = audioClips[(int)type];

        source.PlayOneShot(clip);
    }

    private AudioSource GetFreeSource()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }

        throw new System.Exception("There is no free audio sourse");
    }
}
public enum AudioType
{
    BoxFold = 0,
    StorageFold = 1,
    Door = 2,
    ButtonClick = 3,
    Rokla = 4,
    RotationSound = 5,
    MouseClick = 6,
    UIAppearence = 7,
    Note = 8,
    Grab = 9,
    Transfer = 10,
    DoorClose = 11,
    ShoppingCart = 12,
    Tablet = 13
}
