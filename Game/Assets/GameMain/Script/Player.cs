using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;
   /// UIManager UI;
    [SerializeField]
    private GameObject UiManager;
    [SerializeField]
    private GameObject Right, Left;

    void Start()
    {
       _animator= GetComponent<Animator>();
    //   UI = UiManager.GetComponent<UIManager>();
    }
    
    void Update()
    {
       
    }
    void DamageSart()
    {
        Left.GetComponent<BoxCollider>().enabled = false;
        Right.GetComponent<BoxCollider>().enabled = false;
    }
   void MoveBox()
    {
        Left.GetComponent<BoxCollider>().enabled = true;
        Right.GetComponent<BoxCollider>().enabled = true;
    }
    //プレイヤーダメージモーション終了
    void DamageEnd()
    {
        
        _animator.SetBool("Damage", false);
        
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Damage")
        {
            StartCoroutine("CreateCube");
            
            _animator.SetBool("Damage", true);
        //   
          // 
            iTween.MoveTo(gameObject, iTween.Hash("position", transform.position - (transform.forward * 1f),
                "time", 1.0f
                ));
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
