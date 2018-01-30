using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_stageObjects;

    delegate void UpdateFunc();

    private GameObject m_selectObject;

    UpdateFunc m_func;
	// Use this for initialization
	void Start () {
        m_func = SelectStage;
	}
	
	// Update is called once per frame
	void Update () {
        m_func();
	}


    private void SelectStage()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 50.0f);

           
            if (hit && hit.collider.GetComponent<StageObject>() )
            {
                StageChanger checkTarget = hit.collider.gameObject.transform.FindChild("TargetStage").GetComponent<StageChanger>();
                Debug.Log(checkTarget);
                if (checkTarget.IsRelease())
                {
                    hit.collider.GetComponent<StageObject>().OpenWindow();
                    m_func = OpenWindow;
                    m_selectObject = hit.collider.gameObject;
                }
            }
        }
    }

    private void OpenWindow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 50.0f);
            Debug.Log(hit.collider.tag);

            if (hit && hit.collider.tag == "SelectIcon" && hit.collider.name == "TargetStage")
            {
                hit.collider.GetComponent<StageChanger>().ChangeScene();
            }else
            {
                m_selectObject.GetComponent<StageObject>().CloseWindow();
                m_func = SelectStage;
                m_selectObject = null;
            }
        }
    }


    private IEnumerator WaitTime(float _time)
    {
        yield return new WaitForSeconds(_time);
    }
}
