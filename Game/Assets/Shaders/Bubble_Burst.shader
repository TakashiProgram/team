// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Bubble_Burst" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//ノイズテクスチャ　Rに破裂用　Gに振動用の成分を入れる
		_NoiseTex("NoiseTex",2D) = "white"{}
		_BurstRatio("BurstRatio",Range(0,1)) = 0
		_Fluffy("Fluffy",Range(0.01,0.5)) = 0.01
		_VibrateRate("VibrateRate",Range(0.0,1.0)) = 0.0
		_VibrateTimer("VibrateTimer",Float) = 0.0

		_VibrateTest("VibrateTest",Range(0.0,15.0)) = 1.0
		_WindVector("WindVector",Vector) = (1,0,0,1)
		//外積が0ベクトルになった時の対処にif文使いたくない
		//スクリプト側で計算して渡した方が軽い
		_WindCrossVector("WindCross",Vector)=(0,1,0,1)

		//前面ポリゴン不透明度
		_Transparency("Transparency_Back",Range(0,1))=1.0
		//_CoatTickness("CoatTickness",Range(0,1))=0.1
		//背面ポリゴン不透明度
		_CoatTransparency("Transparency_Front",Range(0,1))=1.0
		_SpecularPower("SpecularPower",Range(0,160))=1

		_HitPosition("HitPosition",Vector) = (0,0,0,0)
		//バブルの反射ベクトル　バースト時にスクリプト側で計算してセットする
		_ReflectVector("ReflectVector",Vector)=(0,0,0,1)
		_ReflectRatio("ReflectRatio",Range(0.0,1.0))=0.0

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
			float3 localPos:TEXCOORD3;
		};

		fixed4 _Color;
		half _SpecularPower;
		half _BurstRatio;
		float4 _HitPosition;
		half _Fluffy;
		half _VibrateRate;
		half _VibrateTest;
		float4 _WindVector;
		float4 _WindCrossVector;
		half _VibrateTimer;
		float4 _ReflectVector;
		half _ReflectRatio;

		half4 LightingSpecular(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize(lightDir + viewDir);
			half diff = max(0, dot(-s.Normal, lightDir));
			half3 nlh = normalize(-s.Normal + lightDir);
			diff = max(0, dot(viewDir, -s.Normal));
			diff = 1.0 - diff;

			half nh = abs(dot(-s.Normal, h));// max(0, dot(-s.Normal, h));
			nh = pow(nh, 2);
			half spec = pow(nh, _SpecularPower);

			half alpha = max(0, pow(nh, _SpecularPower - 40));

			half4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb*diff + _LightColor0.rgb*spec)*atten;
			c.a = s.Alpha*diff+alpha;//s.Alpha*alpha*spec;
			return c;
		}


		void vert(inout appdata_full v,out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//反射ベクトル方向にオフセット
			half rat = 1.0f - pow((1.0f - _BurstRatio), 2);
			v.vertex.xyz += _ReflectVector.xyz*rat*_ReflectRatio;
			//反射後の頂点位置を破裂の計算で使いたいのでここでlocalPosに入れる
			o.localPos = v.vertex.xyz;
			const float PI = 3.14159;
			half b = pow(_BurstRatio, 2.0f);
			//そのままだと大きすぎるので適当に調整
			//cos(0)～cos(3PI)までで1.5周期=拡大→縮小→拡大
			half a = 1.0 - ((cos(_BurstRatio*PI*3) + 1.0)*0.2);
			v.vertex.xyz += normalize(v.normal)*a*b;//*b*0.3;//

			

			//ふわふわする感じに頂点を動かす
			half3 normal = normalize(v.normal);
			half3 ofs = float3(normal.x*sin(_Time.w*3)*_Fluffy, normal.y*sin(_Time.w * 3+0.25)*_Fluffy, normal.z*sin(_Time.z*3+0.5)*_Fluffy);
			
			v.vertex.xyz += ofs;

			//Time.w(t*3)でも遅すぎるので適当に調整
			//Time.wの部分をスクリプトで制御するタイマに変えれば振動状態に遷移する度に０から始められる
			//後で検証

			half vib = 1.0 - ((cos(_VibrateTimer*3*_VibrateTest) + 1.0)*0.5);
			//風を受けた方向に応じて震えさせる
			
			//内積結果が０の時の区別が出来ないので2回に分ける
			half3 windVec = _WindVector;//
			half wdotn = dot(windVec, normal);
			v.vertex.xyz += windVec*wdotn*vib*0.1f*_VibrateRate;

			half3 wc = _WindCrossVector;
			//風と垂直な方向には多めにオフセットさせる
			half wcdotn = dot(wc, normal);
			v.vertex.xyz += wc*wcdotn*(1.0f-vib)*0.2f*_VibrateRate;

			//テクスチャ使って凸凹のテスト
			float2 uvOfs = {_Time.y,_Time.y};
			float value = tex2Dlod(_NoiseTex, float4(v.texcoord.xy+uvOfs, 0, 0)).g;
			value = value - 0.5f;
			v.vertex.xyz += normal*value*_VibrateRate*0.2f;
			
			
			v.vertex.xyz += windVec*sin(_VibrateTimer*3*_VibrateTest)*0.1f*_VibrateRate;

		}
		void surf (Input IN, inout SurfaceOutput o) {
			half hitDist = 0;

			float3 localHitPos = _HitPosition;//mul(unity_WorldToObject, _HitPosition).xyz;
			
			//シェーダでの反射と物理マテリアルでの反射の違いの原因はここのhitDist
			//物理マテリアルで反射させると頂点も遠ざかるのでhitDistが長くなる
			//衝突の瞬間のhitDistは0～2.0だが物理マテリアルで反射させると2.0を超える
			hitDist = length(localHitPos - IN.localPos);
			hitDist = hitDist*0.5;//hitDistは0～2.0なので0～1.0に

			half f = tex2D(_NoiseTex, IN.uv_MainTex).r;
			half t = _BurstRatio;
			clip(1.0 - (((1.0 - hitDist) + f)*0.5 + t));

			// Albedo comes from a texture tinted by color
			//unity_DeltaTime使うとカクつくので使用を保留
			half2 uvOfs = half2(_Time.y, _Time.x)*0.05f;//*unity_DeltaTime.z;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex+uvOfs) * _Color;

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
		float4 _HitPosition;
		half _Fluffy;
		half _VibrateRate;
		half _VibrateTest;
		float4 _WindVector;
		float4 _WindCrossVector;
		half _VibrateTimer;
		float4 _ReflectVector;
		half _ReflectRatio;

		struct Input {
			float2 uv_MainTex:TEXCOORD0;
			float3 viewDir:TEXCOORD1;
			float3 worldNormal:TEXCOORD2;
			float3 localPos:TEXCOORD3;
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
			//キャラが中に入った時顔付近が見えづらいので調整
			diff = pow(diff, 3);


			float nh = abs(dot(s.Normal, h));// max(0, dot(s.Normal, h));
			float spec = pow(nh, _SpecularPower);

			float alpha = max(0, pow(nh, _SpecularPower - 40));

			half4 c;
			c.rgb = (s.Albedo*_LightColor0.rgb*diff + _LightColor0.rgb*spec)*atten;
			c.a = s.Alpha*diff+alpha;//*alpha*spec;
			return c;
		}

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			//反射ベクトル方向にオフセット
			half rat = 1.0f - pow((1.0f - _BurstRatio), 2);
			v.vertex.xyz += _ReflectVector.xyz*rat*_ReflectRatio;
			//反射後の頂点位置を破裂の計算で使いたいのでここでlocalPosに入れる
			o.localPos = v.vertex.xyz;
			const float PI = 3.14159;
			half b = pow(_BurstRatio, 2);
			//そのまま_BurstRatio0~1だと大きすぎるので調整
			half a = 1.0 - ((cos(_BurstRatio*PI * 3) + 1.0)*0.3);
			//half f = abs(sin(_BurstRatio*5))*0.5;
			v.vertex.xyz += normalize(v.normal)*a*b;// *b*0.3;//

			

			//ふわふわする感じに頂点を動かす
			half3 normal = normalize(v.normal);
			half3 ofs = float3(normal.x*sin(_Time.w*3)*_Fluffy, normal.y*sin(_Time.w * 3 + 0.25)*_Fluffy, normal.z*sin(_Time.z*3 + 0.5)*_Fluffy);
			
			v.vertex.xyz += ofs;


			//Time.w(t*3)でも遅すぎるので適当に調整
			half vib = 1.0 - ((cos(_VibrateTimer*3 * _VibrateTest) + 1.0)*0.5);

			//風を受けた方向に応じて震えさせる
			half3 windVec = _WindVector;
			half wdotn = abs(dot(windVec, normal));
			v.vertex.xyz += windVec*wdotn*vib*0.1f*_VibrateRate;

			half3 wc = _WindCrossVector;
			half wcdotn = dot(wc, normal);
			v.vertex.xyz += wc*wcdotn*(1.0f - vib)*0.15f*_VibrateRate;

			//テクスチャ使って凸凹のテスト
			float2 uvOfs = { _Time.y,_Time.y };
			float value = tex2Dlod(_NoiseTex, float4(v.texcoord.xy + uvOfs, 0, 0)).g;
			value = value - 0.5f;
			v.vertex.xyz += normal*value*_VibrateRate*0.2f;

			
			v.vertex.xyz += windVec*sin(_VibrateTimer*3*_VibrateTest)*0.1f*_VibrateRate;
		}

		void surf(Input IN, inout SurfaceOutput o) {

			half hitDist = 0;
			float3 localHitPos = _HitPosition;//mul(unity_WorldToObject, _HitPosition).xyz;


			hitDist = length(localHitPos - IN.localPos);
			hitDist = hitDist*0.5;//hitDistは0～2.0なので0～1.0に

			half f = tex2D(_NoiseTex, IN.uv_MainTex).r;
			half t = _BurstRatio;
			clip(1.0 - (((1.0 - hitDist) + f)*0.5 + t));
			// Albedo comes from a texture tinted by color
			//unity_DeltaTime使うとカクつくので使用を保留
			half2 uvOfs = half2(-_Time.y, -_Time.x)*0.05f;//*unity_DeltaTime.z;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex+uvOfs) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = _Transparency;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
