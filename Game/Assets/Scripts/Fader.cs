using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour {

    [SerializeField,Tooltip("フェード用のShaderMatrial")]
    private Material m_material;

    //フェードの影響値
    [SerializeField,Range(0,1)]
    private float m_range;

    [SerializeField, Range(0.1f, 3.0f),Tooltip("フェード終了までの時間")]
    private float m_fadeTime = 0.1f;

    private bool m_fadeFlag;

    // Use this for initialization
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        if (m_fadeFlag)
        {
            m_range = Mathf.Min(m_range + ((1 / m_fadeTime) * Time.deltaTime), 1);
            if(m_range >= 1.0)
            {
                GetComponent<AudioSource>().enabled = false;
            }
        }else
        {
            m_range = Mathf.Max(m_range - ((1 / m_fadeTime) * Time.deltaTime), 0);
        }
    }
    public void FadeIn()
    {
        m_fadeFlag = true;
        Debug.Log("FadeInStart");
        GetComponent<AudioSource>().enabled = true;
    }

    public void FadeOut()
    {
        m_fadeFlag = false;
        Debug.Log("FadeOutStart");
    }
    public bool IsFade()
    {
        return m_fadeFlag;
    }
    public bool IsFadeEnd()
    {
        if (m_fadeFlag && m_range >= 1.0f)
        {
            return true;
        }else if(!m_fadeFlag && m_range <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public void FadeReset()
    {
        m_fadeFlag = false;
        m_range = 0.0f;
    }
    public float GetFadeTime()
    {
       return m_fadeTime;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        m_material.SetFloat("_Range", m_range);
        Graphics.Blit(source, dest, m_material);
    }
}
