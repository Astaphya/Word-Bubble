using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] AudioClip sound;
    private Button button {get {return GetComponent<Button>();}}
    private AudioSource source {get {return GetComponent<AudioSource>();}}
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound ;
        source.playOnAwake = false ;

        button.onClick.AddListener(() => PlaySound());
    }

    public void PlaySound()
    {
        source.PlayOneShot(sound);
    }

    
}
