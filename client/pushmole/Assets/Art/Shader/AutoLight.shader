Shader "AutoLight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//        _EvementLight ("EvementLight", Range(0, 1)) = 0
        _AutoLight ("AutoLight", Range(0, 1)) = 0.5197519
        //_Glossiness ("Glossiness", Range(0, 1)) = 0.5197519
        //_Metallic("Metallic", Range(0, 1)) = 0.5197519
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert 
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 2.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		//half _Glossiness;
		//half _Metallic;
		half _AutoLight;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			float3 emissionNew =(c*_AutoLight);
			o.Emission = emissionNew;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
