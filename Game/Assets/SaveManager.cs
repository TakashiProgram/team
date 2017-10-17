using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveManager {
   
    [System.Serializable]
    public struct SaveData
    {
        public StageManager.StageData[] stageData;
    }
    const string FILE_NAME = "/saveData.dat";
    public static void SaveFile()
    {
        SaveData save;
        save.stageData = new StageManager.StageData[GameObject.Find("StageManager").GetComponent<StageManager>().GetStagsData().Length];
        StageManager.StageData[] stageData = GameObject.Find("StageManager").GetComponent<StageManager>().GetStagsData();
       for (int i = 0;i < save.stageData.Length; i++)
        {
            save.stageData[i] = stageData[i];
        }

        string sData = JsonUtility.ToJson(save);
        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream file = null;
        try
        {
            file = File.Create(Application.dataPath + FILE_NAME);
            bFormatter.Serialize(file,sData);
            file.Close();
        }
        catch (IOException)
        {
            Debug.LogError("FileOpenError");
        }
    }
    public static StageManager.StageData[] LoadFile()
    {
        if (File.Exists(Application.dataPath + FILE_NAME))
        {
            string loadData;
            StageManager.StageData[] ret;
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Open(Application.dataPath + FILE_NAME, FileMode.Open);
                loadData = bFormatter.Deserialize(file) as string;
                ret = JsonUtility.FromJson<SaveData>(loadData).stageData;
                if (ret != null)
                {
                    file.Close();
                    return ret;
                }
            }
            catch (IOException)
            {
                Debug.LogError("FileOpenError");
            }
        }
        return null;
    }
}
