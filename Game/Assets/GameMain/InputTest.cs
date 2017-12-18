using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTest : MonoBehaviour {
    [SerializeField]
    Vector2 _flickStartPos;
    Vector2 _flickEndPos;

    Vector2 _flickVector;
    //フリック操作中かどうか
    bool _isFlicking;

    public GameObject _bubblePrefab;
    public GameObject _physicBounceBubble;

    public GameObject _currentBubble;

    [SerializeField]
    GameObject _creatingBubble;
    float _creatingBubbleScale;
    bool _isCreating;


    bool _isCanvasEnable;

    // Use this for initialization
    void Start () {
        _flickEndPos = new Vector2(0, 0);
        _flickStartPos = new Vector2(0, 0);
        _flickVector = new Vector2(0, 0);
        _isFlicking = false;
        _isCreating = false;

        _isCanvasEnable = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameObject obj;
            obj = GameObject.Instantiate(_physicBounceBubble, new Vector3(7, 0, 0), Quaternion.identity);
            obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            obj = GameObject.Instantiate(_bubblePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            obj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }

        //動かすバブルが存在していればフリック処理
        if (_currentBubble != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //ポインタがボタンの上に無ければ処理する
                //タッチ検出の時は引数にPointerID(0)を入れる
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    _flickStartPos = Input.mousePosition;
                    _isFlicking = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_isFlicking)
                {
                    Flick();
                }
            }
        }

        //バブルの生成途中か
        if (_isCreating)
        {
            AddBubbleScale();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _isCanvasEnable = !_isCanvasEnable;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = _isCanvasEnable;
        }

    }

    public void CreateBubble()
    {
        if (_creatingBubble != null)
        {

        }
        else
        {
            _creatingBubble = GameObject.Instantiate(_bubblePrefab, new Vector3(-5, 0, 0), Quaternion.identity);
            _creatingBubbleScale = 0.3f;
            _creatingBubble.transform.localScale = new Vector3(_creatingBubbleScale, _creatingBubbleScale, _creatingBubbleScale);
            _isCreating = true;
        }

    }

    //比較用
    public void CreateBubble_old()
    {
        if (_creatingBubble != null)
        {

        }
        else
        {
            _creatingBubble = GameObject.Instantiate(_physicBounceBubble, new Vector3(3, 0, 0), Quaternion.identity);
            _creatingBubbleScale = 0.3f;
            _creatingBubble.transform.localScale = new Vector3(_creatingBubbleScale, _creatingBubbleScale, _creatingBubbleScale);
            _isCreating = true;
        }

    }
    void AddBubbleScale()
    {
        if (_creatingBubble != null)
        {
            _creatingBubbleScale += 1 * Time.deltaTime;
            _creatingBubble.transform.localScale = new Vector3(_creatingBubbleScale, _creatingBubbleScale, _creatingBubbleScale);
            //生成（拡大）中に生成ボタンから指が離れたらその時点で生成を終わる
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                ExitBubbleCreate();
            }
        }
    }
    public void ExitBubbleCreate()
    {
        if (_creatingBubble != null)
        {
            _creatingBubble.GetComponent<BubbleEffectTest>().BubbleVibrate();
            //今生成したバブルを動かすバブルに設定
            SetBubble(_creatingBubble);

            _creatingBubble = null;
            _isCreating = false;


        }
    }

    void Flick()
    {
        _flickEndPos = Input.mousePosition;
        if ((_flickEndPos - _flickStartPos).magnitude > 0.01f)
        {
            _flickVector = (_flickEndPos - _flickStartPos).normalized;

            _currentBubble.GetComponent<BubbleEffectTest>().Move(
                new Vector3(_flickVector.x, _flickVector.y, 0));
        }
        _isFlicking = false;
    }

    //フリックで動かすバブルを設定
    void SetBubble(GameObject obj)
    {
        _currentBubble = obj;
    }
}
