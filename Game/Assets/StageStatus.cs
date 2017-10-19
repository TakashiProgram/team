﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClearRank
{
    rank_none = 0,rank_D,rank_C,rank_B,rank_A,rank_S
}

public class StageStatus : MonoBehaviour {
    //ClearRankをもとに、現在のクリアされたランクを格納します。
    private ClearRank m_rank;


    [SerializeField, Tooltip("移動先のステージ")]
    private SceneNameList m_changeTarget;

    [SerializeField, Tooltip("このステージを開放するためのクリア条件対象")]
    private SceneNameList m_checkTarget;


    //解放条件を満たしているかどうかを判定するbool型です。
    private bool m_isRelease;

    //すべてのステージの情報を統括するStageManager
    StageManager m_sMgr;

	// Use this for initialization
	void Start () {
        m_sMgr = GameObject.Find("StageManager").GetComponent<StageManager>();	
	}
	
    public void SetRank(ClearRank _rank)
    {
        m_rank = _rank;
    }
    public ClearRank GetRank()
    {
        return m_rank;
    }

    //自身の移動先を返す
    public SceneNameList GetChangeTarget()
    {
        return m_changeTarget;
    }
    /*
     * このステージが解放されているかのフラグを取得します
     * ステージが解放されている場合trueを返します
     */
    public bool IsRelease()
    {
        return m_isRelease;
    }

    /*
    * ステージが解放されるかどうかのチェックをする関数です。
    * 解放条件を満たしている場合、ステージの解放処理をします。
    */
    public void Check()
    {
        //すでに解放されているなら終了
        if (m_isRelease)
        {
            return;
        }
        //解放条件を満たしているかをチェックする
        if (ReleaseCheck())
        {
            //ステージ開放処理
            StageRelease();
        }
    }

    /*ステージの解放を行う関数です。
    * ステージの解放フラグをtrueに変更します。
    */
    public void StageRelease()
    {
        m_isRelease = true;
    }

    /*ステージ開放のチェック用関数です。
     */
    private bool ReleaseCheck()
    {
        //解放条件がない場合はtrue
        if (m_checkTarget == SceneNameList.None)
        {
            return true;
        }
        else if (m_sMgr.IsClearStage(m_checkTarget))
        {
            return true;
        }
        return false;
    }

    
}