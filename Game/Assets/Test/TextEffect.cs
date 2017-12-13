using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class TextEffect : UIBehaviour,IMeshModifier {

    int _frame;

    Text _text;

    void Awake()
    {
        _frame = 0;
        _text = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        ++_frame;
    }

    public new void OnValidate()
    {
        base.OnValidate();

        Graphic graphics = base.GetComponent<Graphic>();

        if(graphics==null)
        {
            graphics.SetVerticesDirty();
        }
    }

    public void ModifyMesh(Mesh mesh) { }

    public void ModifyMesh(VertexHelper verts)
    {
        var stream = ListPool<UIVertex>.Get();
        verts.GetUIVertexStream(stream);

        ModifyVerts(ref stream);

        verts.Clear();
        verts.AddUIVertexTriangleStream(stream);

        ListPool<UIVertex>.Release(stream);

    }

    void ModifyVerts(ref List<UIVertex> stream)
    {
        //Debug.Log("ModifyVerts");
        // 頂点を云々する。１テキスト６頂点
        for (int i = 0,vertCnt = stream.Count;i<vertCnt;i+=6)
        {
            //1文字の中心を求める
            Vector3 center = Vector2.Lerp(stream[i].position, stream[i + 3].position, 0.5f);

            for(int j=0;j<6;++j)
            {
                UIVertex element = stream[i + j];
                Vector3 localPos = element.position - center;
                Vector3 localVec = localPos.normalized;
                //拡大縮小
                float f = 1.0f - ((Mathf.Cos(Time.time*10) + 1.0f) * 0.5f);
                localPos = localVec * _text.fontSize*0.5f;
                //回転
                localPos = Quaternion.Euler(0, 0, Time.time * 50) *localPos;

                element.position = center + localPos;
                stream[i + j] = element;
            }
        }

    }


    // Update is called once per frame
    void Update () {

        _text.SetAllDirty();
	}
}
