using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour {
    enum MoveState
    {
        ms_wait,ms_select,ms_cancel
    }

    [SerializeField,Range(0.1f,2.0f)]
    private float m_moveEndTime;
    private float m_moveTime;


    private readonly Vector2 SELECT_POS = new Vector2(-5.84f, 0);
    private Animator m_animator;

    private MoveState m_state;
    private Vector3 m_initPos;

    private bool m_animationEndFlag;

    // Use this for initialization
    void Start()
    {
        m_moveTime = m_moveEndTime;
        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator, gameObject.name + "にアニメーターが存在しません");
        m_state = MoveState.ms_wait;
        m_initPos = transform.position;
        m_animationEndFlag = true;
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_state)
        {
            case MoveState.ms_wait:
                break;
            case MoveState.ms_select:
                if (m_moveTime < m_moveEndTime)
                {
                    m_moveTime += Time.deltaTime;
                    Vector3 tmp = Vector3.Lerp(m_initPos, SELECT_POS, m_moveTime / m_moveEndTime);
                    tmp.z = transform.position.z;
                    transform.position = tmp;
                }
                else if(!m_animator.GetBool("WindowFlag"))
                {
                    m_animator.SetBool("WindowFlag", true);
                }
                break;
            case MoveState.ms_cancel:
                if (m_moveTime < m_moveEndTime && m_animationEndFlag)
                { 
                    m_moveTime += Time.deltaTime;
                    Vector3 tmp = Vector3.Lerp(SELECT_POS, m_initPos, m_moveTime / m_moveEndTime);
                    tmp.z = transform.position.z;
                    transform.position = tmp;
                }

                break;
        }
	}
    public void OpenWindow()
    {
        if ((m_state == MoveState.ms_wait || m_state == MoveState.ms_cancel) && m_moveTime >= m_moveEndTime && m_animationEndFlag)
        {
            m_animationEndFlag = false;
            m_state = MoveState.ms_select;
            m_moveTime = 0;
        }
    }

    public void CloseWindow()
    {
        if (m_animator && m_state == MoveState.ms_select && m_animator.GetBool("WindowFlag") && m_animationEndFlag)
        {
            m_animationEndFlag = false;
            m_animator.SetBool("WindowFlag", false);
            m_moveTime = 0;
            m_state = MoveState.ms_cancel;
        }
    }

    public bool IsOpen()
    {
        return m_animator.GetBool("WindowFlag");
    }

    public float GetMoveEndTime()
    {
        return m_moveEndTime;
    }

    public void AnimEnd()
    {
        m_animationEndFlag = true;
    }
}
