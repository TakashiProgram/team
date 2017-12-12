using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour {

     protected static CoroutineHandler m_instance;
     public static CoroutineHandler GetInstance()
    {
        if(m_instance == null)
        {
            GameObject instance = new GameObject("CoroutineHandler");
            DontDestroyOnLoad(instance);
            m_instance = instance.AddComponent<CoroutineHandler>();
        }

        return m_instance;
    }
    public void OnDisable()
    {
        if (m_instance)
        {
            Destroy(m_instance.gameObject);
        }
    }
    public static Coroutine StartStaticCoroutine(IEnumerator _coroutine) {
        return GetInstance().StartCoroutine(_coroutine);
    }
}
