Shader "Custom/LeafShaderLow" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MainTexUnhealthy("Unhealthy Albedo (RGB)", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0,1)) = 0.5
		_AddMul("AddMul", Range(0,1)) = 0.5
		_HealthFactor("Healthyness", Range(0,1)) = 0.5
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf StandardSpecular fullforwardshadows
			// Use Shader model 3.0 target
			#pragma target 3.0
			sampler2D _MainTex;
			sampler2D _MainTexUnhealthy;
			struct Input {
				float2 uv_MainTex;
				float3 worldNormal;
				INTERNAL_DATA
			};
			half _Smoothness;
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
				UNITY_DEFINE_INSTANCED_PROP(half, _HealthFactor)
				UNITY_DEFINE_INSTANCED_PROP(half, _AddMul)
			UNITY_INSTANCING_BUFFER_END(Props)
			void surf(Input IN, inout SurfaceOutputStandardSpecular o) {
				fixed4 healthy = tex2D(_MainTex, IN.uv_MainTex);
				clip(healthy.a - 0.5);
				fixed4 unhealthy = tex2D(_MainTexUnhealthy, IN.uv_MainTex);
				fixed4 mul = healthy * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
				fixed4 add = healthy;
				add.rgb = lerp(add.rgb, UNITY_ACCESS_INSTANCED_PROP(Props, _Color), 0.5);

				healthy = lerp(mul, add, UNITY_ACCESS_INSTANCED_PROP(Props, _AddMul));
				healthy = lerp(unhealthy, healthy, UNITY_ACCESS_INSTANCED_PROP(Props, _HealthFactor));

				o.Albedo = healthy.rgb;
				// o.Metallic = _Metallic;
				// o.Normal = IN.worldNormal;// UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				// o.Specular = 0.5;// tex2D(_Specular, IN.uv_MainTex);
				// o.Smoothness = _Smoothness;
				o.Alpha = healthy.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}