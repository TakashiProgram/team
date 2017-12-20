Shader "Custom/River" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	//FlowTexはR,Gしか使ってないので後でNoiseTexをBに入れるように変更する
		//_FlowTex("FlowTex",2D) = "gray"{}
		_NoiseTex("NoiseTex",2D) = "white"{}
		_FlowSpeed("FlowSpeed",Range(0.0,1.0))=0.0

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		//sampler2D _FlowTex;
		sampler2D _NoiseTex;
		float _FlowSpeed;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input i, inout SurfaceOutputStandard o)
		{
			fixed4 col;
			float noise = tex2D(_NoiseTex, i.uv_MainTex).g;

			//float2 floatDir = (tex2D(_FlowTex, i.uv_MainTex) * 2.0 - 1.0);
			//floatDir.y *= -1;
			//単純に下スクロールなのでテクスチャ使わなくていい
			float2 floatDir = float2(0, -1);
			floatDir *= _FlowSpeed;
			float phase0 = frac(_Time.y + noise);
			float phase1 = frac(_Time.y + 0.5f + noise);

			half3 tex0 = tex2D(_MainTex, floatDir.xy*phase0 + i.uv_MainTex);
			half3 tex1 = tex2D(_MainTex, floatDir.xy*phase1 + i.uv_MainTex);

			float flowLerp = abs((0.5f - phase0) / 0.5f);
			col.rgb = lerp(tex0, tex1, flowLerp);
			col.a = 1.0;

			// Albedo comes from a texture tinted by color
			col*= _Color;
			o.Albedo = col.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = col.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
