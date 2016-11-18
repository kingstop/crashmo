Shader "Bodhi/transparent" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_AutoLight("AutoLight", Range(0, 1)) = 0.5
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
			LOD 200

			CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 2.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _AutoLight;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutput o) {
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