using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Image lifeGage;
    [SerializeField]
    private Image lifeRedGage;
    [SerializeField]
    private GameObject DamageHp;


    
    private float m_Life = 1;

    private float m_LifeMax = 1;

    private float m_Damage = 0.1f;

    private float m_Time=1;

    private float m_Gradually=0.01f;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update()
    {
      
    }
  
}
