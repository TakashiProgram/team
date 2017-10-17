using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
//すべてのステージの情報を統括するクラス
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
    [System.Serializable]
    public struct SaveData
    {
        public StageData[] stageData;
    }

    [SerializeField, Tooltip("シーン上に配置されるすべてのステージを格納します。")]
    private GameObject[] m_stages;

    private StageData[] m_stagesData;



    // Use this for initialization
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
    }

    
    private void OnApplicationQuit()
    {


        m_stagesData = new StageData[m_stages.Length];

        for (int i = 0; i < m_stages.Length; i++)
        {
            m_stagesData[i].name = m_stages[i].name;
            m_stagesData[i].ReleaseFlag = m_stages[i].GetComponent<StageStatus>().IsRelease();
            m_stagesData[i].rank = m_stages[i].GetComponent<StageStatus>().GetRank() + 1;
        }
        //自身が終了するときにステージの全情報を保存する
        SaveManager.SaveFile();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //引数に設定されたステージのクリア状況を返す。
    //true : クリアされている(ランク問わず)
    //false : クリアされていない
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

    //_nameと同一のStageDataをout_dataに格納して返却する
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

    //ステージデータの読み込み
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
