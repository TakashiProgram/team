using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Tools/CreateStageData")]
public class StageData : ScriptableObject {
    [System.Serializable]
    public struct Data
    {
        public string name;
        public bool ReleaseFlag;
        public ClearRank rank;
    }

    [SerializeField]
    public Data[] data;

    /// <summary>
    /// 存在するデータから指定の名前のランク情報を上書きします
    /// </summary>
    /// <param name="_name">検索対象の名前</param>
    /// <param name="_rank">検索対象に上書きするランク情報</param>
    /// <returns>true = 検索対象への上書き完了 : false = 検索対象の不一致</returns>
    public bool SetClearRank(string _name,ClearRank _rank)
    {
        for (int i = 0;i < data.Length;i++)
        {
            if(data[i].name == _name)
            {
                data[i].rank = _rank;
                return true;
            }
        }
        return false;
    }
	

    public ClearRank GetRank(string _name)
    {
        for(int i = 0; i < data.Length; i++)
        {
            if(data[i].name == _name)
            {
                return data[i].rank;
            }
        }
        return ClearRank.rank_none;
    }
}
