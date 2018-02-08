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
    [SerializeField]
    private AudioSource[] audioSource;
    
    private AudioSource audioaSource;

    void Start () {
       
        //audioSource = gameObject.GetComponent<AudioSource>();
       
    }
	
	void Update () {
       
    }
    public void Sound(int count , int array)
    {

        audioSource[array].clip = audioClip[count];

        audioSource[array].Play();

    }

    public void ResultBGM()
    {
        m_BGM.GetComponent<AudioSource>().mute = true;
        m_resultSound.GetComponent<AudioSource>().mute = false;
        m_resultSound.SetActive(true);
    }
    public void ContinuousSound(int count,int array)
    {
        audioSource[array].clip = audioClip[count];
        if (!audioSource[array].isPlaying)
        {
            audioSource[array].Play();
        }
    }
}
