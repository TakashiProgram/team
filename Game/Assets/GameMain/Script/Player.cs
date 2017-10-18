using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private GameObject UiManager,InputManager;

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
    }
    
    void Update()
    {
        //float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
        //Vector3 dir = new Vector3(Mathf.Cos(InputManager.GetComponent<InputManager>().angle), Mathf.Sin(InputManager.GetComponent<InputManager>().angle), 0.0f);

        //transform.position += dir * 10 * Time.deltaTime;
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

    //playerの移動処理
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
        //ノックバック
        if (collision.gameObject.tag=="Enemy")
        {
            //StartCoroutine("CreateCube");

            //_animator.SetBool("Damage", true);

            //iTween.MoveTo(gameObject, iTween.Hash("position", transform.position - (transform.forward * 1f),
            //    "time", 1.0f
            //    ));
            Debug.Log("wg");
            float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
            Vector3 dir = new Vector3(Mathf.Cos(InputManager.GetComponent<InputManager>().angle), Mathf.Sin(InputManager.GetComponent<InputManager>().angle), 0.0f);

            transform.position += dir *10* Time.deltaTime; 

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
            
        }


        
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Bubble")
        {
           
            this.transform.position = this.transform.parent.position;
        }
    }

    //無敵時間
    IEnumerator CreateCube()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
       
       
            yield return new WaitForSeconds(2.0f);
        
        gameObject.layer = LayerMask.NameToLayer("Player");
    }



}
