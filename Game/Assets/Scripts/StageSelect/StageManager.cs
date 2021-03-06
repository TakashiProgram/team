﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///すべてのステージの情報を統括するクラスです。
///</summary>
public class StageManager : MonoBehaviour
{

    //セーブデータ用構造体


    [SerializeField, Tooltip("シーン上に配置されるすべてのステージを格納します。")]
    private GameObject[] m_stages;
    
   
    private bool m_once;

    private static bool m_LoadData = false;


    void Start()
    {
       

        if (!m_LoadData)
        {
            Debug.Log("Save");
            LoadSaveFile();
            m_LoadData = true;
        }

        LoadStagesData();
       
        m_once = false;

        SaveManager.SaveFile(StageData.GetInstance());
    }

    private void Update()
    {
        if (!m_once)
        {
            //データの設定が終わったらそのデータによるステージ開放の更新をかけていく
            foreach (var stage in m_stages)
            {
                stage.GetComponent<StageChanger>().Check();
            }
            m_once = true;
        }
    }


    private void OnApplicationQuit()
    {
        StageData.GetInstance().data = new StageData.Data[m_stages.Length];

        for (int i = 0; i < m_stages.Length; i++)
        {
            StageData.GetInstance().data[i].name = m_stages[i].GetComponent<StageChanger>().GetChangeTarget().ToString();
            StageData.GetInstance().data[i].ReleaseFlag = m_stages[i].GetComponent<StageChanger>().IsRelease();
            StageData.GetInstance().data[i].rank = m_stages[i].GetComponent<StageChanger>().GetRank();
        }
        //自身が終了するときにステージの全情報を保存する
        SaveManager.SaveFile(StageData.GetInstance());
    }

    ///<summary>
    ///引数に設定されたステージのクリア状況を返します。
    ///true : クリアされている(ランク問わず)
    ///false : クリアされていない
    ///</summary>
    public bool IsClearStage(SceneNameList _stageName)
    {
        StageChanger target = null;
        foreach (var stage in m_stages)
        {
            if (stage.GetComponent<StageChanger>().GetChangeTarget() == _stageName)
            {
                target = stage.GetComponent<StageChanger>();
                break;
            }
        }
        //ターゲットとなるステージが存在して、かつそのランクがクリアランクに達していたらtrue
        if (target != null && target.GetRank() != ClearRank.rank_none)
        {
            return true;
        }

        return false;

    }

    ///<summary>
    ///_nameと同一のStageDataをout_dataに格納して返却します。
    ///</summary>
    public void GetStageData(string _name, out StageData.Data out_data)
    {
        StageData.Data target = new StageData.Data();
        target.name = "";
        target.rank = ClearRank.rank_none;
        target.ReleaseFlag = false;
        foreach (var data in StageData.GetInstance().data)
        {
            if (data.name == _name)
            {
                target = data;
            }
           
        }
        out_data = target;
    }
    ///<summary>
    ///ステージデータの読み込みます。
    ///</summary>
    private void LoadStagesData() {
       if(StageData.GetInstance().data == null)
        {
            return;
        }

        foreach (var stage in m_stages)
        {
            foreach (var data in StageData.GetInstance().data)
            {
                if (stage.GetComponent<StageChanger>().GetChangeTarget().ToString() == data.name)
                {
                    stage.GetComponent<StageChanger>().SetRank(data.rank);
                }
            }
        }
    }

    private void LoadSaveFile()
    {
        StageData.GetInstance().data = SaveManager.LoadFile();
        //セーブファイルが存在していなかった場合はデータをシーンから読み取って保存する
        if (StageData.GetInstance().data == null)
        {

            StageData.GetInstance().data = new StageData.Data[m_stages.Length];

            for (int i = 0; i < m_stages.Length; i++)
            {
                StageData.GetInstance().data[i].name = m_stages[i].GetComponent<StageChanger>().GetChangeTarget().ToString();
                StageData.GetInstance().data[i].ReleaseFlag = m_stages[i].GetComponent<StageChanger>().IsRelease();
                StageData.GetInstance().data[i].rank = m_stages[i].GetComponent<StageChanger>().GetRank();
            }
            SaveManager.SaveFile(StageData.GetInstance());
        }
    }

}
