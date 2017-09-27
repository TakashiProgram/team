// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/Mask" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture",2D) = "white"{}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", float) = 8
		_Stencil("Stencil ID",float) = 0
		_StencilOp("Stencil Operation",float) = 0
		_StencilWriteMask("Stencil Write Mask",float) = 255
		_StencilReadMask("Stencil Read Mask",float) = 255

		_Range("Range",Range(0,1)) = 0
		_MaskTex("Mask Texture", 2D) = "white"{}

		_ColorMask("Color Mask",float) = 15
	}
	SubShader{
			Tags {
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]

			}
			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

	#include "UnityCG.cginc"
	#include "UnityUI.cginc"

				struct appdata_t {
					float4 vertex	: POSITION;
					float4 color	: COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex	: SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord	: TEXCOORD0;
					float4 worldPosition	: TEXCOORD1;

				};

				fixed4 _Color;
				fixed4 _TextureSampleAdd;

				bool _UseClipRect;
				float4 _ClipRect;

				bool _UseAlphaClip;



				v2f vert(appdata_t IN) {
					v2f outPut;
					outPut.worldPosition = IN.vertex;
					outPut.vertex = UnityObjectToClipPos(outPut.worldPosition);
					
					outPut.texcoord = IN.texcoord;

	#ifdef UNITY_HALF_TEXEL_OFFSET
					output.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1, 1);
	#endif
					outPut.color = IN.color * _Color;
					return outPut;
				}


			sampler2D _MainTex;
			sampler2D _MaskTex;
			float _Range;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex,IN.texcoord) + _TextureSampleAdd) * IN.color;
				float4 maskTex = tex2D(_MaskTex, IN.texcoord);

				half mask = min(min(maskTex.r, maskTex.g), maskTex.b);
				half alpha = mask - (-1 + _Range * 2);
				color.rgb *= min(alpha,1);

				if (_UseClipRect) {
					color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

				}
				if (_UseAlphaClip) {
					clip(color.a - 0.001);
				}
				return color;
			}


			ENDCG
		}
	}
}