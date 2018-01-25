using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour {

    [SerializeField]
    private AudioClip[] audioClip;

    [SerializeField]
    private GameObject m_BGM;

    [SerializeField]
    private GameObject m_resultSound;

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

    public void ResultBGM()
    {
        m_BGM.GetComponent<AudioSource>().mute = true;
        m_resultSound.GetComponent<AudioSource>().mute = false;
        m_resultSound.SetActive(true);
    }
    public void ContinuousSound(int count)
    {
        audioSource.clip = audioClip[count];
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
