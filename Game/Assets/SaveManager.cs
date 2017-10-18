using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//StageManagerの情報をもとに、セーブファイルを管理するクラスです。
public static class SaveManager {
   
    //セーブ情報のすべてを格納する構造体
    [System.Serializable]
    public struct SaveData
    {
        public StageManager.StageData[] stageData;
    }

    //セーブファイルの名前
    const string FILE_NAME = "/saveData.dat";

    //現在のStageManagerの情報から、セーブデータを作成し、FILE_NAMEの名前で保存します。
    public static void SaveFile()
    {
        SaveData save;
        //ステージのデータ分の配列を確保
        save.stageData = new StageManager.StageData[GameObject.Find("StageManager").GetComponent<StageManager>().GetStagsData().Length];
        //ステージデータを取得
        StageManager.StageData[] stageData = GameObject.Find("StageManager").GetComponent<StageManager>().GetStagsData();

        //ステージデータの内容をsaveに格納
       for (int i = 0;i < save.stageData.Length; i++)
        {
            save.stageData[i] = stageData[i];
        }

       //saveに格納されたデータをJsonをもとにテキストデータへ変換
        string sData = JsonUtility.ToJson(save);

        BinaryFormatter bFormatter = new BinaryFormatter();

        FileStream file = null;
        try
        {
            //変換されたデータをFILE_NAMEで保存
            file = File.Create(Application.dataPath + FILE_NAME);
            bFormatter.Serialize(file,sData);
            file.Close();
        }
        catch (IOException)
        {
            Debug.LogError("FileOpenError");
        }
    }

    //FILE_NAMEのデータからStageDataの情報を取得して返します。
    public static StageManager.StageData[] LoadFile()
    {
        //FILE_NAMEが存在している場合にローディングを実行
        if (File.Exists(Application.dataPath + FILE_NAME))
        {
            string loadData;
            StageManager.StageData[] ret;
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream file = null;
            try
            {
                //データをstring形式で取得
                file = File.Open(Application.dataPath + FILE_NAME, FileMode.Open);
                loadData = bFormatter.Deserialize(file) as string;
                //Jsonを用いて取得したデータをSaveData形式に変換
                ret = JsonUtility.FromJson<SaveData>(loadData).stageData;
                //情報が存在する場合はその値を返して終了
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
        //セーブファイルが存在しないorセーブファイルにデータが存在しないならnullを返して終了
        return null;
    }
}
