using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class SceneChanger{

    private const float DEFAULT_FADE_TIME = 1.0f;

    private static bool m_once = false;
	
   public static void LoadSceneAtList(SceneNameList _listName)
    {
        if((int)_listName <= 0)
        {
            Debug.LogError("_listNameがNoneのためシーンを移行できません。");
            return;
        }
       
        SceneManager.LoadScene(((int)_listName) - 1,LoadSceneMode.Single);
    }

  public static void LoadSceneAtListAsync(SceneNameList _listName)
    {

        CoroutineHandler.StartStaticCoroutine(LoadAtListAsync(_listName));
    }
    static IEnumerator LoadAtListAsync(SceneNameList _listName)
    {
        if (m_once) yield break;
        m_once = true;
        Debug.Log("LoadScene To :" + _listName.ToString());
        if ((int)_listName <= 0)
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

        AsyncOperation asyncData =  SceneManager.LoadSceneAsync(((int)_listName) - 1,LoadSceneMode.Single);
        asyncData.allowSceneActivation = false;

        while(asyncData.progress < 0.9f)
        {
            
            Debug.Log((int)((asyncData.progress / 0.9f) * 100) +"%");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Secene Loaded");

        asyncData.allowSceneActivation = true;
        m_once = false;
    }
}

