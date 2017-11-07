﻿Shader "Custom/NewSurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex("NoiseTex",2D) = "white"{}
		_BurstRatio("BurstRatio",Range(0,1)) = 0
		
		//前面ポリゴン不透明度
		_Transparency("Transparency_Back",Range(0,1))=1.0
		//_CoatTickness("CoatTickness",Range(0,1))=0.1
		//背面ポリゴン不透明度
		_CoatTransparency("Transparency_Front",Range(0,1))=1.0
		_SpecularPower("SpecularPower",Range(0,80))=1

		_HitPosition("HitPosition",Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Transparent"
				"Queue" = "Transparent"
				"LightMode" = "ForwardBase" 
		}
		LOD 200

		Cull Front
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Specular fullforwardshadows alpha:blend vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		half _CoatTransparency;

		struct Input {
			float2 uv_MainTex:TEXCOORD0;
			half3 viewDir:TEXCOORD1;
			half3 worldNormal:TEXCOORD2;
		};

		fixed4 _Color;
		half _SpecularPower;

		half4 LightingSpecular(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize(lightDir + viewDir);
			half diff = max(0, dot(-s.Normal, lightDir));
			half3 nlh = normalize(-s.Normal + lightDir);
			diff = max(0, dot(viewDir, -s.Normal));
			diff = 1.0 - diff;
			//diff = pow(diff, 2);

			half nh = abs(dot(-s.Normal, h));// max(0, dot(-s.Normal, h));
			nh = pow(nh, 2);
			half spec = pow(nh, _SpecularPower+20);

			half alpha = max(0, pow(nh, _SpecularPower - 40));

			half4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb*diff + _LightColor0.rgb*spec)*atten;
			c.a = s.Alpha*diff+alpha;//s.Alpha*alpha*spec;
			return c;
		}

		half _BurstRatio;

		void vert(inout appdata_full v)
		{
			const float PI = 3.14159;
			half b = pow(_BurstRatio, 2);
			//そのまま_BurstRatio0~1だと大きすぎるので調整
			half a = 1.0 - ((cos(_BurstRatio*PI*3) + 1.0)*0.5);
			half f = (sin(_BurstRatio * 5) + 1.0)*0.5;
			//half f = abs(sin(_BurstRatio*5))*0.5;
			v.vertex.xyz += normalize(v.normal)*a*b*0.5;
		}
		void surf (Input IN, inout SurfaceOutput o) {
			half f = tex2D(_NoiseTex, IN.uv_MainTex);
			half b = pow(_BurstRatio, 2);
			clip(1.0-(b + f));
			// Albedo comes from a texture tinted by color
			//unity_DeltaTime使うとカクつくので使用を保留
			half2 uvOfs = half2(_Time.y, _Time.x)*unity_DeltaTime.z*2;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex+uvOfs) * _Color;
			//c.rgb = c*IN.worldNormal.xyz;

			o.Albedo = c.rgb;
			o.Alpha = _CoatTransparency;
		}
		ENDCG



			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Lighting Off
			
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Specular alpha:blend vertex:vert 

			// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

			sampler2D _MainTex;
		sampler2D _NoiseTex;
		half _BurstRatio;
		float _CoatTickness;

		struct Input {
			float2 uv_MainTex:TEXCOORD0;
			float3 viewDir:TEXCOORD1;
			float3 worldNormal:TEXCOORD2;
			
		};

		fixed4 _Color;
		float _Transparency;
		half _SpecularPower;

		half4 LightingSpecular(SurfaceOutput s, half3 lightDir,half3 viewDir, half atten)
		{
			half3 h = normalize(lightDir + viewDir);
			half diff = max(0,dot(s.Normal,h));
			half3 nlh = normalize(lightDir + s.Normal);
			diff = max(0, dot(viewDir, nlh));
			diff = 1.0 - diff;
			diff = pow(diff, 3);


			float nh = abs(dot(s.Normal, h));// max(0, dot(s.Normal, h));
			float spec = pow(nh, _SpecularPower);

			float alpha = max(0, pow(nh, _SpecularPower - 40));

			half4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb*diff + _LightColor0.rgb*spec)*atten;
			c.a = s.Alpha*diff+alpha;//*alpha*spec;
			return c;
		}

		void vert(inout appdata_full v)
		{
			//そのまま_BurstRatio0~1だと大きすぎるので調整
			const float PI = 3.14159;
			half b = pow(_BurstRatio, 2);
			half a = 1.0 - ((cos(_BurstRatio*PI*3) + 1.0)*0.5);
			half f = sin(_BurstRatio);
			v.vertex.xyz += normalize(v.normal)*a*b*0.5;
			//v.vertex.xyz -= v.normal.xyz*_CoatTickness;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half f = tex2D(_NoiseTex, IN.uv_MainTex);
			half b = pow(_BurstRatio, 2);
			clip(1.0 - (b + f));
			//o.Albedo = fixed4(f, f, f, 1);
			// Albedo comes from a texture tinted by color
			//unity_DeltaTime使うとカクつくので使用を保留
			half2 uvOfs = half2(-_Time.y, -_Time.x)*unity_DeltaTime.z*2;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex+uvOfs) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = _Transparency;
		}
		ENDCG
	}
	FallBack "Diffuse"
}