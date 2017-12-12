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

    private bool m_isFade;

    // Use this for initialization
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_isFade);
        if (m_isFade)
        {
            m_range = Mathf.Min(m_range + ((1 / m_fadeTime) * Time.deltaTime), 1);
        }else
        {
            m_range = Mathf.Max(m_range - ((1 / m_fadeTime) * Time.deltaTime), 0);
        }
    }
    public void FadeIn()
    {
        m_isFade = true;
        Debug.Log(m_isFade);
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
