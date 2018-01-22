using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour {
    
    public AudioClip[] audioClip;

    private AudioSource audioSource;

    void Start () {
       
        audioSource = gameObject.GetComponent<AudioSource>();
       
    }
	
	void Update () {
       
    }
    public void sound(int count)
    {
       
        audioSource.clip = audioClip[count];

        audioSource.Play();

    }
}
