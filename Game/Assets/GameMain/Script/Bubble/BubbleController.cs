using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BubbleController : MonoBehaviour {
    enum BubbleState
    {
        none,
        spawn,//生成途中（膨らませている時）の状態
        floating,//浮かんでいる状態
        burst//破裂中の状態
    }
    Dictionary<BubbleState, Action> _stateAction =new Dictionary<BubbleState, Action>();
    Action _currentAct;
    BubbleState _currentState;
    //現在のステートが始まってからのフレーム数
    int _currentStateFrame;
    //バブルの破裂が始まってから完全に破裂するまでにかかる時間（秒）
    [SerializeField] float _burstTime;
    //破裂開始からの経過フレーム
    int _burstFrame;
    //破裂率
    float _burstRate;

    //浮遊中の回転成分
    Vector3 _euler;

    Material _material;

	void Start ()
    {
        _burstRate = 0.0f;
        _material = GetComponent<Renderer>().material;

        _currentStateFrame = 0;
        //連想配列にステートごとの処理追加
        _stateAction.Add(BubbleState.floating, FloatingUpdate);
        _stateAction.Add(BubbleState.burst, BurstUpdate);

        //最初のステートを決定
        float minRange = 30.0f;
        float maxRange = 40.0f;
        Float(new Vector3(UnityEngine.Random.Range(minRange, maxRange), UnityEngine.Random.Range(minRange, maxRange), UnityEngine.Random.Range(minRange, maxRange)));
	}
	
	void Update ()
    {
        ++_currentStateFrame;
        _currentAct();
	}

    void ChangeState(BubbleState state)
    {
        _currentStateFrame = 0;
        _currentState = state;
        _currentAct = _stateAction[_currentState];
    }

    /*破裂中の処理
     * この中でマテリアルのパラメータを変化させる
     */
    void BurstUpdate()
    {
        _burstRate = Math.Min(_burstRate + ((1.0f / _burstTime) * Time.deltaTime), 1.0f);
        //float b = Math.Min((_currentStateFrame * Time.deltaTime) / _burstTime, 1.0f);
        _material.SetFloat("_BurstRatio",_burstRate);
        Debug.Log("_burstRate = " + _burstRate);
        if(_burstRate>=1.0f)
        {
            Destroy(gameObject);
            //Time.timeScale=1.0f;
        }
    }
    //浮いている時の処理（特に何もしない）
    void FloatingUpdate()
    {
        Debug.Log("浮遊中");
        //回転させてみる
        Quaternion q = Quaternion.Euler(_euler*Time.deltaTime);
        transform.rotation = q * transform.rotation;

        //一旦保留
        _euler.x = Mathf.Max(5.0f, _euler.x - 2.0f * Time.deltaTime);
        _euler.y = Mathf.Max(5.0f, _euler.y - 2.0f * Time.deltaTime);
        _euler.z = Mathf.Max(5.0f, _euler.z - 2.0f * Time.deltaTime);
    }

    //浮遊状態へ遷移　rot:浮遊中の初期回転成分
    void Float(Vector3 rot)
    {
        _euler = rot;
        ChangeState(BubbleState.floating);
    }

    void Float()
    {
        float minRange = 30.0f;
        float maxRange = 40.0f;
        _euler = new Vector3(UnityEngine.Random.Range(minRange, maxRange), UnityEngine.Random.Range(minRange, maxRange), UnityEngine.Random.Range(minRange, maxRange));
        ChangeState(BubbleState.floating);
    }

    //破裂処理に遷移 burstTime:破裂にかかる時間
    public void Burst(float burstTime)
    {
        _burstTime = burstTime;
        ChangeState(BubbleState.burst);
        //Time.timeScale = 0.1f;
    }
    public void Burst()
    {
        //_burstTime
        ChangeState(BubbleState.burst);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player")
        {
            foreach (ContactPoint point in col.contacts)
            {
                //w要素は1にしておく
                Vector4 hitpos = point.point;
                hitpos.w = 1;
                _material.SetVector("_HitPosition", hitpos);

                Burst(_burstTime);
            }
            //とりあえず何か当たったら止まるようにしとく（テスト用
            //GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Player")
        {
            /*Collider.ClosestPointOnBounds(Vector3) 返り値Vector3
             * 設定した座標に一番近いColliderオブジェクトの座標を返す
             * Vector4型に代入するとw要素は０になってるので1にしておく
             */
            Vector4 hitpos = col.ClosestPointOnBounds(transform.position);
            hitpos.w = 1;
            _material.SetVector("_HitPosition", hitpos);

            Burst(_burstTime);
            //とりあえず何か当たったら止まるようにしとく（テスト用
            //GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
