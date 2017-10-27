using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///すべてのステージの情報を統括するクラスです。
///</summary>
public class StageManager : MonoBehaviour
{

    //セーブデータ用構造体
    [System.Serializable]
    public struct StageData
    {
        public string name;
        public bool ReleaseFlag;
        public ClearRank rank;
    }
   

    [SerializeField, Tooltip("シーン上に配置されるすべてのステージを格納します。")]
    private GameObject[] m_stages;

    private StageData[] m_stagesData;




    void Start()
    {
        m_stagesData = SaveManager.LoadFile();
        //セーブファイルが存在していなかった場合はデータをシーンから読み取って保存する
        if (m_stagesData == null)
        {
            m_stagesData = new StageData[m_stages.Length];

            for (int i = 0; i < m_stages.Length; i++)
            {
                m_stagesData[i].name = m_stages[i].name;
                m_stagesData[i].ReleaseFlag = m_stages[i].GetComponent<StageStatus>().IsRelease();
                m_stagesData[i].rank = m_stages[i].GetComponent<StageStatus>().GetRank();
            }
            SaveManager.SaveFile();
        }
        else
        {
            LoadStagesData();
        }

        //データの設定が終わったらそのデータによるステージ開放の更新をかけていく
        foreach (var stage in m_stages)
        {
            stage.GetComponent<StageStatus>().Check();
        }
    }

    
    private void OnApplicationQuit()
    {
        m_stagesData = new StageData[m_stages.Length];

        for (int i = 0; i < m_stages.Length; i++)
        {
            m_stagesData[i].name = m_stages[i].name;
            m_stagesData[i].ReleaseFlag = m_stages[i].GetComponent<StageStatus>().IsRelease();
            m_stagesData[i].rank = m_stages[i].GetComponent<StageStatus>().GetRank();
        }
        //自身が終了するときにステージの全情報を保存する
        SaveManager.SaveFile();
    }

    ///<summary>
    ///引数に設定されたステージのクリア状況を返します。
    ///true : クリアされている(ランク問わず)
    ///false : クリアされていない
    ///</summary>
    public bool IsClearStage(SceneNameList _stageName)
    {
        StageStatus target = null;
        foreach (var stage in m_stages)
        {
            if (stage.GetComponent<StageStatus>().GetChangeTarget() == _stageName)
            {
                target = stage.GetComponent<StageStatus>();
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

    public StageData[] GetStagsData()
    {
        return m_stagesData;
    }
    ///<summary>
    ///_nameと同一のStageDataをout_dataに格納して返却します。
    ///</summary>
    public void GetStageData(string _name, out StageData out_data)
    {
        StageData target = new StageData();
        target.name = "";
        target.rank = ClearRank.rank_none;
        target.ReleaseFlag = false;
        foreach (var data in m_stagesData)
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
        foreach (var stage in m_stages)
        {
            foreach (var data in m_stagesData)
            {
                if (stage.name == data.name)
                {
                    stage.GetComponent<StageStatus>().SetRank(data.rank);
                }
            }
        }
    }


}
