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
       //ダメージをくらったらHPバー減少
        if (Input.GetKeyDown(KeyCode.Space))
        {
            iTween.ValueTo(this.gameObject, iTween.Hash
                ("from", m_Life, 
                "to", m_Life -m_Damage ,
                "time", m_Time,
                "onupdate", "DamageRed"));
            m_Life -= m_Damage;
            lifeGage.fillAmount = m_Life / m_LifeMax;  
        }
        //シャボン玉を膨らましているとき
        if (Input.GetKey(KeyCode.Z))
        {
            lifeGage.fillAmount -= m_Gradually;
            lifeRedGage.fillAmount -= m_Gradually;
        }
    }
    //ダメージレットバーの減少
    void DamageRed(float m_Damage)
    {
        lifeRedGage.fillAmount = m_Damage / m_LifeMax;
        
    }
  
}
