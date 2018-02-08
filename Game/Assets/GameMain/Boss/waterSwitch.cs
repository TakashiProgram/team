using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterSwitch : MonoBehaviour {

    [SerializeField]
    private GameObject m_soundManager;

    private const int SWITCH_SOUND = 10;

    // Use this for initialization
    void Start () {
        this.GetComponent<Animator>().speed = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void stop()
    {
        this.GetComponent<Animator>().speed = 0;
        
        Invoke("Reset", 5);
    }
    private void stop2()
    {
        this.GetComponent<Animator>().speed = 0;
    }
    private void Sound()
    {
        m_soundManager.GetComponent<SoundManage>().Sound(SWITCH_SOUND, 1);
    }

    public void Reset()
    {
        this.GetComponent<Animator>().speed = 1;
    }

}
