using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSoundManager : MonoBehaviour
{
    public static cSoundManager Instance { get; private set; } = null;
    private void Awake()
    {
        Instance = this;
    }

    [HideInInspector]
    public AudioClip brickBreak;
    public AudioClip brickBG;
    public AudioClip shot;
    public AudioClip shotBG;
    public AudioClip shotExplosion;
    public AudioClip snakeMove;
    public AudioClip snakeBG;
    public AudioClip snakeGetScore;

    AudioSource audioSource;
    void Start()
    {
        brickBreak = Resources.Load<AudioClip>("Daeho/Sound/Breaking blocks");
        brickBG = Resources.Load<AudioClip>("Daeho/Sound/Breaking blocksBG");
        shot = Resources.Load<AudioClip>("Daeho/Sound/Shoot"); 
        shotBG = Resources.Load<AudioClip>("Daeho/Sound/ShootingBG");
        shotExplosion = Resources.Load<AudioClip>("Daeho/Sound/Explosion");
        snakeMove = Resources.Load<AudioClip>("Daeho/Sound/Snake");
        snakeBG = Resources.Load<AudioClip>("Daeho/Sound/SnakeBG");
        snakeGetScore = Resources.Load<AudioClip>("Daeho/Sound/Ding");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    public void AudioPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void AudioPlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
