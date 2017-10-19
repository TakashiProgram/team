Shader "Custom/NewSurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_NoiseTex("NoiseTex",2D) = "white"{}
		
		//前面ポリゴン不透明度
		_Transparency("Transparency_Back",Range(0,1))=1.0
		//_CoatTickness("CoatTickness",Range(0,1))=0.1
		//背面ポリゴン不透明度
		_CoatTransparency("Transparency_Front",Range(0,1))=1.0
		_SpecularPower("SpecularPower",Range(0,80))=1
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
		
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Specular fullforwardshadows alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _NormalTex;
		float _CoatTransparency;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
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

			float nh = abs(dot(-s.Normal, h));// max(0, dot(-s.Normal, h));
			nh = pow(nh, 2);
			float spec = pow(nh, _SpecularPower+20);

			float alpha = max(0, pow(nh, _SpecularPower - 40));

			half4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb*diff + _LightColor0.rgb*spec)*atten;
			c.a = s.Alpha*diff+alpha;//s.Alpha*alpha*spec;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			//unity_DeltaTime使うとカクつくので使用を保留
			half2 uvOfs = half2(_Time.y, _Time.x);
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex+uvOfs) * _Color;
			o.Albedo = c.rgb;
			
			o.Alpha = _CoatTransparency;
		}
		ENDCG



			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Specular alpha:blend vertex:vert 

			// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

			sampler2D _MainTex;
		sampler2D _NormalTex;
		float _CoatTickness;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
			
		};

		half _Glossiness;
		half _Metallic;
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
			//v.vertex.xyz -= v.normal.xyz*_CoatTickness;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			//unity_DeltaTime使うとカクつくので使用を保留
			half2 uvOfs = half2(-_Time.y, -_Time.x);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex+uvOfs) * _Color;
			o.Albedo = c.rgb;
			//float f = max(0,dot(IN.viewDir, IN.worldNormal));
			o.Alpha = _Transparency;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
