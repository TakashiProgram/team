using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRank : MonoBehaviour {
    [SerializeField]
    private Sprite[] m_sprites;

    [SerializeField]
    private GameObject m_rankSpriteObj;

    private ClearRank m_rank;

    bool isLoad;

	// Use this for initialization
	void Start () {
        Debug.Assert(m_rankSpriteObj, "RankSpriteObjが存在しません。\n RankSpriteObjを指定してください。");
        isLoad = false;

        StageChanger target;
        target = transform.parent.GetComponentInChildren<StageChanger>();
        if (target != null)
        {
            LoadRankData();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLoad)
        { 
            LoadRankData();
        }
	}


    private void LoadRankData()
    {
        StageChanger target;
        target = transform.parent.GetComponentInChildren<StageChanger>();
        if (target == null || m_rankSpriteObj == null)
        {
            return;
        }
        string stageName = target.GetChangeTarget().ToString();
        m_rank = StageData.GetInstance().GetRank(stageName);
        m_rankSpriteObj.GetComponent<SpriteRenderer>().sprite = m_sprites[(int)m_rank];
        isLoad = true;
    }
}
