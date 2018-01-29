using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRank : MonoBehaviour {
    [SerializeField]
    private Sprite[] m_sprites;

    private ClearRank m_rank;
    

	// Use this for initialization
	void Start () {
        string stageName = GetComponent<StageChanger>().GetChangeTarget().ToString();
        m_rank = Resources.Load<StageData>("StagesData").GetRank(stageName);
        GetComponent<SpriteRenderer>().sprite = m_sprites[(int)m_rank];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
