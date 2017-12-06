Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SpecularPower("SpecPower",Range(0,30))=1
		_SpecularColor("SpecularColor",Color) = (0.5,0.5,0.5,1)


	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
#include "Lighting.cginc"
#include"AutoLight.cginc"

			//fixed4 _LightColor0;
			half _SpecularPower;
			fixed4 _SpecularColor;
			//fixed4 _LightColor0;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal:NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 wpos:TEXCOORD1;
				float3 normal:TEXCOORD2;
				float3 diffuse : TEXCOORD3;
				float3 specular : TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.wpos = v.vertex;// mul(unity_ObjectToWorld, v.vertex);

				o.normal = UnityObjectToWorldNormal(v.normal);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
				// sample the texture
				fixed4 tex = tex2D(_MainTex, i.uv);

				float3 wpos = mul(unity_ObjectToWorld, i.wpos);
			//平行光源ならベクトルは常に同じなので -wposしなくていい
			float3 L = normalize(_WorldSpaceLightPos0.xyz);
			float3 V = normalize(_WorldSpaceCameraPos.xyz - wpos.xyz);
			float3 H = normalize(L + V);
			float3 N = i.normal;
			float3 lightCol = _LightColor0.rgb*LIGHT_ATTENUATION(i);

			float3 NdotL = dot(N, H);
			float3 diffuse = (NdotL*0.5f + 0.5f)*lightCol;

			float3 specular = max(0.0, pow(dot(H, N), _SpecularPower)*_SpecularColor.xyz*_LightColor0.xyz);

			fixed4 col = fixed4((ambient + diffuse)*tex.rgb +specular,pow(max(0.0,dot(H,N)),_SpecularPower));
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
