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

  public static IEnumerator LoadSceneAtListAsync(SceneNameList _listName)
    {
        Debug.Log("LoadScene To :" + _listName.ToString());
        if ((int)_listName < 0)
        {
            yield break;
        }
      
        Fader fadeTarget =  GameObject.Find("FadeCamera").GetComponent<Fader>();
        if (fadeTarget != null)
        {
            fadeTarget.FadeIn();
            yield return new WaitForSeconds(fadeTarget.GetFadeTime());
        }


        AsyncOperation asyncData =  SceneManager.LoadSceneAsync((int)_listName,LoadSceneMode.Single);
        asyncData.allowSceneActivation = false;

        while(asyncData.progress < 0.9f)
        {
            
            Debug.Log((int)((asyncData.progress / 0.9f) * 100) +"%");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Secene Loaded");
        asyncData.allowSceneActivation = true;
    }
}
