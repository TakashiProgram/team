using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

///<summary>
///StageManagerの情報をもとに、セーブファイルを管理するクラスです。
/// </summary>
public static class SaveManager
{

    //セーブ情報のすべてを格納する構造体
    [System.Serializable]
    public struct SaveData
    {
        public StageData.Data[] stageData;
    }

    //セーブファイルの名前
    const string FILE_NAME = "/saveData.dat";
    ///<summary>
    ///現在のStageManagerの情報から、セーブデータを作成し、FILE_NAMEの名前で保存します。
    ///</summary>
    public static void SaveFile(StageData _data)
    {
        SaveData save;

        save.stageData = new StageData.Data[_data.data.Length];
        //ステージデータを取得
        StageData.Data[] stageData = _data.data;

        //ステージデータの内容をsaveに格納
        for (int i = 0; i < save.stageData.Length; i++)
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
            file = File.Create(Application.persistentDataPath + FILE_NAME);
            bFormatter.Serialize(file, sData);
            file.Close();
        }
        catch (IOException)
        {
            Debug.LogError("FileOpenError");
        }
    }

    /// <summary>
    /// FILE_NAMEのデータからStageDataの情報を取得して返します。
    /// </summary>
    public static StageData.Data[] LoadFile()
    {
        //FILE_NAMEが存在している場合にローディングを実行
        if (File.Exists(Application.persistentDataPath + FILE_NAME))
        {
            string loadData;
            StageData.Data[] ret;
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream file = null;
            try
            {
                //データをstring形式で取得
                file = File.Open(Application.persistentDataPath + FILE_NAME, FileMode.Open);
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
