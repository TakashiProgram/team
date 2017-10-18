using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//SceneNameListをもとにしてシーンロードを行うクラスです。
public static class SceneChanger {

	//SceneNameListからシーンをロードします。
   public static void LoadSceneAtList(SceneNameList _listName)
    {
        if((int)_listName < 0)
        {
            Debug.LogError("_listNameの値が不正です。_listNameの値は正の値でなくてはなりません");
            return;
        }
       
        SceneManager.LoadScene((int)_listName,LoadSceneMode.Single);
    }

    //SceneNameListからシーンを非同期でロードします。
    //シーンが読み込まれている場合はAsyncOperationを、失敗した場合はnullを返します。
   public static AsyncOperation LoadSceneAtListAsync(SceneNameList _listName)
    {
        if ((int)_listName < 0)
        {
            
            Debug.LogError("_listNameの値が不正です。_listNameの値は正の値でなくてはなりません");
            return null;
        }
       
       return SceneManager.LoadSceneAsync((int)_listName,LoadSceneMode.Single);
    }
}
