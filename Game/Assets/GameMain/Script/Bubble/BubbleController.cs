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
        burst,//破裂中の状態
        vibrate,//振動中の状態
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

    //生成されてからシャボンが震える時間
    [SerializeField] float _vibrateTime;
    float _vibrateRate;
    //シェーダに渡して使うタイマ　振動の制御に使う
    float _vibrateTimer;

    //浮遊中の回転成分
    Vector3 _euler;

    Material _material;

    Rigidbody _rigidBody;
    //1フレーム前のワールド座標
    Vector3 _lastPos;
    //前フレームの座標との差分ベクトル
    [SerializeField]
    Vector3 _diffVec;


    private void Awake()
    {
        _burstRate = 0.0f;
        _vibrateRate = 1.0f;
        _material = GetComponent<Renderer>().material;
        _vibrateTimer = 0.0f;

        _currentStateFrame = 0;
        //連想配列にステートごとの処理追加
        _stateAction.Add(BubbleState.floating, FloatingUpdate);
        _stateAction.Add(BubbleState.burst, BurstUpdate);
        _stateAction.Add(BubbleState.spawn, SpawnUpdate);
        _stateAction.Add(BubbleState.vibrate, VibrationUpdate);

        //最初のステートを決定
        ChangeState(BubbleState.spawn);
        _material.SetFloat("_Fluffy", 0.01f);
        _material.SetVector("_HitPosition", new Vector4(0, 0, 0, 0));
        _material.SetFloat("_VibrateRate", 0.0f);
        //_material.SetVector("_WindVector", new Vector4(1, 0, 0, 1));

        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _lastPos = gameObject.transform.position;
    }
    void Start ()
    {
        
	}
	
	void Update ()
    {
        
        if ((transform.position - _lastPos).magnitude > 0.0f)
        {
            _diffVec = (transform.position - _lastPos).normalized;
        }
        
        _rigidBody.velocity *= 0.995f;

        if (_currentAct != null)
        {
            ++_currentStateFrame;
            _currentAct();
        }

        
	}

    private void LateUpdate()
    {
        _lastPos = transform.position;

        Vector3 p = Camera.main.transform.position;
        //Z軸がカメラと同じ方向を向いてほしいのでposition-cameraPosition
        Quaternion q = Quaternion.LookRotation(transform.position - p, Vector3.up);
        //gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.1f);
    }

    void ChangeState(BubbleState state)
    {
        //破裂の最中は状態遷移させない
        if (_currentState != BubbleState.burst)
        {
            _currentStateFrame = 0;
            _currentState = state;
            _currentAct = _stateAction[_currentState];
        }
    }

    /*破裂中の処理
     * この中でマテリアルのパラメータを変化させる
     */
    void BurstUpdate()
    {
        _burstRate = Math.Min(_burstRate + ((1.0f / _burstTime) * Time.deltaTime), 1.0f);
        _material.SetFloat("_BurstRatio",_burstRate);
        if(_burstRate>=1.0f)
        {
            //gameObject.GetComponent<Bubble>().ParentRelease();
            Time.timeScale = 1.0f;
            Destroy(gameObject);
        }
    }
    //浮いている時の処理（特に何もしない）
    void FloatingUpdate()
    {
        //Vibrateのテスト用コード
        //if (_currentStateFrame > 60)
        //{
        //    BubbleVibrate();
        //}

        //回転させてみる
        Quaternion q = Quaternion.Euler(_euler*Time.deltaTime);
        //transform.rotation = q * transform.rotation;

        //一旦保留
        _euler.x = Mathf.Max(5.0f, _euler.x - 2.0f * Time.deltaTime);
        _euler.y = Mathf.Max(5.0f, _euler.y - 2.0f * Time.deltaTime);
        _euler.z = Mathf.Max(5.0f, _euler.z - 2.0f * Time.deltaTime);
    }

    //生成途中の処理
    void SpawnUpdate()
    {
        //特に何もしない
    }

    //振動中の処理
    void VibrationUpdate()
    {
        _vibrateTimer += Time.deltaTime;
        _material.SetFloat("_VibrateTimer", _vibrateTimer);
        _vibrateRate = Mathf.Max(0.0f, _vibrateRate - ((1.0f / _vibrateTime) * Time.deltaTime));
        _material.SetFloat("_VibrateRate", _vibrateRate);
        if(_vibrateRate<=0.0f)
        {
            Time.timeScale = 1.0f;
            ChangeState(BubbleState.floating);
        }
    }
    //バブルを振動状態に遷移させる
    public void BubbleVibrate()
    {
        _vibrateRate = 1.0f;
        _vibrateTimer = 0.0f;
        _material.SetFloat("_VibrateRate", _vibrateRate);
        _material.SetFloat("_VibrateTimer", _vibrateTimer);

        //Time.timeScale = 0.1f;
        ChangeState(BubbleState.vibrate);
    }
    //受け取った風のベクトルによって振動ベクトルも変える
    public void BubbleVibrate(Vector3 windVec)
    {
        //Time.timeScale = 0.1f;
        _vibrateRate = 1.0f;
        _vibrateTimer = 0.0f;
        _material.SetFloat("_VibrateRate", _vibrateRate);
        _material.SetFloat("_VibrateTimer", _vibrateTimer);

        //windVec = transform.InverseTransformVector(windVec).normalized;
        _material.SetVector("_WindVector", new Vector4(windVec.x, windVec.y, windVec.z, 1));
        ChangeState(BubbleState.vibrate);
    }

    //浮遊状態へ遷移　rot:浮遊中の初期回転成分
    public void BubbleFloat(Vector3 rot)
    {
        _material.SetFloat("_Fluffy", 0.03f);
        _euler = rot;
        ChangeState(BubbleState.floating);
    }

    public void BubbleFloat()
    {
        //バブルの柔らかさ
        _material.SetFloat("_Fluffy", 0.03f);
        //適当に回転成分決める
        float minRange = 30.0f;
        float maxRange = 50.0f;
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
    //引数無しならBubbleControllerに設定された破裂時間を使用する
    public void Burst(Collision col)
    {
        Time.timeScale = 0.1f;
        foreach (ContactPoint point in col.contacts)
        {
            //w要素は1にしておく
            Vector3 hitpos = point.point;
            //オブジェクト空間に移動する
            hitpos = transform.InverseTransformPoint(hitpos);
            _material.SetVector("_HitPosition", new Vector4(hitpos.x,hitpos.y,hitpos.z,1));

            //反射ベクトルを計算する
            //球体との接触点から球の中心へのベクトルなので法線として扱う
            Vector3 n = (gameObject.transform.position - point.point).normalized;
            Vector3 f = _diffVec;
            float a = Vector3.Dot(-f, n);
            //法線方向への成分を強く反映させる（本来は2*a*n) 正規化するので大きくなってもいい
            Vector3 refl = f + 10 * a * n;
            //ワールド座標系からローカル座標系に変換
            refl = transform.InverseTransformVector(refl).normalized;
            if (gameObject.GetComponent<SphereCollider>().sharedMaterial==null)
            {
                _material.SetVector("_ReflectVector", new Vector4(refl.x, refl.y, refl.z, 1));
                //Debug.Log("SetReflectVector");
            }
            //Debug.Log("Reflect = (" + refl.x+","+refl.y+","+refl.z+")");

        }
        ChangeState(BubbleState.burst);
    }

    //引数無し　時間経過で破裂する時等に呼ばれる
    public void Burst()
    {
        ChangeState(BubbleState.burst);
    }

    public void Burst(Collider col)
    {
        Time.timeScale = 0.1f;
        gameObject.GetComponent<SphereCollider>().isTrigger = false;
        /*Collider.ClosestPointOnBounds(Vector3) 返り値Vector3
             * 設定した座標に一番近いColliderオブジェクトの座標を返す
             * Vector4型に代入するとw要素は０になってるので1にしておく
             */
        Vector3 hitpos = col.ClosestPointOnBounds(transform.position);
        //オブジェクト空間に移動する
        hitpos = transform.InverseTransformPoint(hitpos);
        _material.SetVector("_HitPosition", hitpos);

        //反射ベクトルを計算する
        //球体との接触点から球の中心へのベクトルなので法線として扱う
        Vector3 n = (gameObject.transform.position - col.ClosestPointOnBounds(transform.position)).normalized;
        Vector3 f = _diffVec;
        float a = Vector3.Dot(-f, n);
        //法線方向への成分を強く反映させる（本来は2*a*n) 正規化するので大きくなってもいい
        Vector3 refl = f + 10 * a * n;
        //ワールド座標系からローカル座標系に変換
        refl = transform.InverseTransformVector(refl).normalized;
        if (gameObject.GetComponent<SphereCollider>().sharedMaterial == null)
        {
            _material.SetVector("_ReflectVector", new Vector4(refl.x, refl.y, refl.z, 1));
            //Debug.Log("SetReflectVector");
        }

        ChangeState(BubbleState.burst);
        //とりあえず動かないようにする
        //gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.GetComponent<Collider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag != "Player")
        //{
        //    foreach (ContactPoint point in col.contacts)
        //    {
        //        Vector3 hitpos = point.point;

        //        Vector3 f = _diffVec;
        //        Vector3 n = (transform.position - point.point).normalized;
        //        float a = Vector3.Dot(-f, n);
        //        Vector3 refl = f + 2 * a * n;
        //        Move(refl);
        //    }
        //}
    }
    private void OnTriggerEnter(Collider col)
    {
        //if (col.gameObject.tag != "Player")
        //{
        //    Vector3 hitpos = col.ClosestPointOnBounds(transform.position);

        //    Vector3 tempWind = gameObject.transform.position - hitpos;
        //    BubbleVibrate(tempWind);
        //}

        //Vector3 f = _diffVec;
        //Vector3 n=(transform.position - col.ClosestPointOnBounds(transform.position)).normalized;
        //float a = Vector3.Dot(-f, n);
        //Vector3 refl = f + 2 * a * n;
        //Move(refl);
    }

    public void Move(Vector3 windVec)
    {
        _rigidBody.velocity = windVec * 10.0f;//*windSpdとかにする
        BubbleVibrate(windVec);
    }
}
