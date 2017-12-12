using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class SceneChanger{

    private const float DEFAULT_FADE_TIME = 1.0f;
	
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

        CoroutineHandler.StartStaticCoroutine(LoadAtListAsync(_listName));
    }
    static IEnumerator LoadAtListAsync(SceneNameList _listName)
    {
        Debug.Log("LoadScene To :" + _listName.ToString());
        if ((int)_listName < 0)
        {
            yield break;
        }
        GameObject target = GameObject.Find("FadeCamera");
       
        if (target != null)
        {
            Fader fadeTarget = target.GetComponent<Fader>();
            fadeTarget.FadeIn();
            yield return new WaitForSeconds(fadeTarget.GetFadeTime());
        }else
        {
            Debug.Log("NonFade");
            yield return new WaitForSeconds(DEFAULT_FADE_TIME);
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

