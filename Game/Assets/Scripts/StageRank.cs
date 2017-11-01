using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Tools/StageRank")]
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
	
}
