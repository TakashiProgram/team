using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private GameObject UiManager;

    [SerializeField]
    private GameObject Right, Left;

    [SerializeField]
    private GameObject[] PlayerHp;

    private int m_PlayerDesCount = 0;

    private int m_PlayerDesCountMax = 2;

    public bool bubbleFlag = false;
    void Start()
    {
       _animator= GetComponent<Animator>();
    //   UI = UiManager.GetComponent<UIManager>();
    }
    
    void Update()
    {

    }
    //playerのダメージ処理
    void DamageSart()
    {
        if (m_PlayerDesCount == m_PlayerDesCountMax)
        {
            Destroy(gameObject);
        }
        Left.GetComponent<BoxCollider2D>().enabled = false;
        Right.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(PlayerHp[m_PlayerDesCount]);
        m_PlayerDesCount++;
        

    }

    //playerの移動するときの処理
   void MoveBox()
    {
        Left.GetComponent<BoxCollider2D>().enabled = true;
        Right.GetComponent<BoxCollider2D>().enabled = true;
    }

    //プレイヤーダメージモーション終了
    void DamageEnd()
    {
        
        _animator.SetBool("Damage", false);
        
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Enemy")
        {
            StartCoroutine("CreateCube");
            
            _animator.SetBool("Damage", true);
            
            iTween.MoveTo(gameObject, iTween.Hash("position", transform.position - (transform.forward * 1f),
                "time", 1.0f
                ));
        }

        if (collision.gameObject.tag == "Bubble")
        {
            Debug.Log("sgfn");
            transform.parent = GameObject.Find("Bubble").transform;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bubble")
        {
            bubbleFlag = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            transform.parent = GameObject.Find("Bubble(Clone)").transform;
            this.transform.position = this.transform.parent.position;
            Debug.Log("sgfn");
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Bubble")
        {
           
            this.transform.position = this.transform.parent.position;
           // Debug.Log("sgfn");
        }
    }

    //private void OnTriggerExit(Collider collision)
    //{
    //    if (collision.gameObject.tag == "Bubble")
    //    {
    //        bubbleFlag = false;
    //        transform.parent = null;
    //        Debug.Log("sgfn");
    //    }
    //}
    //無敵時間
    IEnumerator CreateCube()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
       
       
            yield return new WaitForSeconds(2.0f);
        
        gameObject.layer = LayerMask.NameToLayer("Player");
    }



}
