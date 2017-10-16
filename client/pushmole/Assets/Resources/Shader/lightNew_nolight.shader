Shader "Bodhi/lightNew_nolight" {
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
		#pragma surface surf CustomLambert
		#pragma target 2.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _AutoLight;
		fixed4 _Color;

		fixed4 LightingCustomLambert (SurfaceOutput s, fixed3 lightDir, half3 viewDir,fixed atten)
	    {
            float4 col;  
            col.rgb = s.Albedo; 
            col.a = s.Alpha;  
            return col;  
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			float3 emissionNew =(c*_AutoLight);
			o.Emission = emissionNew;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
