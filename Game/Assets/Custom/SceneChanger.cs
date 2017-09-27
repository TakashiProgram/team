using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class SceneChanger {

	
   public static void LoadSceneAtList(SceneNameList _listName)
    {
        if((int)_listName < 0)
        {
            Debug.LogError("_listNameの値が不正です。_listNameの値は正の値でなくてはなりません");
            return;
        }
       
        SceneManager.LoadScene((int)_listName,LoadSceneMode.Single);
    }

   public static void LoadSceneAtListAsync(SceneNameList _listName)
    {
        if ((int)_listName < 0)
        {
            
            Debug.LogError("_listNameの値が不正です。_listNameの値は正の値でなくてはなりません");
            return;
        }
        
        SceneManager.LoadSceneAsync((int)_listName,LoadSceneMode.Single);
    }
}
