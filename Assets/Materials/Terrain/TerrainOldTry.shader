﻿Shader "Custom/TerrainOldTry" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_HighlightColor ("Highlight Color", Color) = (1, 1, 1, 1)
		_HighlightArray ("Highlight Color", Color) = (1, 1, 1, 1)
		_HighlightStrength("Highlight Stength", Range(0,1)) = 0.2
		_MainTex ("Terrain Texture Array", 2DArray) = "white" {}
		_GridTex ("Grid Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.5

		#pragma multi_compile _ GRID_ON

		#include "../HexCellData.cginc"

		UNITY_DECLARE_TEX2DARRAY(_MainTex);

		sampler2D _GridTex;

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _HighlightColor;
		half _HighlightStrength;

		struct Input {
			float4 color : COLOR;
			float3 worldPos;
			float3 terrain;
			float3 visibility;
		};

		void vert (inout appdata_full v, out Input data) {
			UNITY_INITIALIZE_OUTPUT(Input, data);

			float4 cell0 = GetCellData(v, 0);
			float4 cell1 = GetCellData(v, 1);
			float4 cell2 = GetCellData(v, 2);

			data.terrain.x = cell0.w;
			data.terrain.y = cell1.w;
			data.terrain.z = cell2.w;

			data.visibility.x = cell0.x;
			data.visibility.y = cell1.x;
			data.visibility.z = cell2.x;
			data.visibility = lerp(0, _HighlightStrength, data.visibility);
		}

		float4 GetTerrainColor (Input IN, int index) {
			float3 uvw = float3(IN.worldPos.xz * 0.02, IN.terrain[index]);
			float4 c = UNITY_SAMPLE_TEX2DARRAY(_MainTex, uvw);
			float4 cc = IN.color[index] * c;
			half lerpFactor = distance(cc, _HighlightColor) / 2.5;
			float4 hc = lerp(cc, _HighlightColor, IN.visibility[index] * lerpFactor);

			//return c * (IN.color[index] + (IN.visibility[index] * _HighlightColor));
			return hc;

		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c =
				GetTerrainColor(IN, 0) +
				GetTerrainColor(IN, 1) +
				GetTerrainColor(IN, 2);

			fixed4 grid = 1;
			#if defined(GRID_ON)
				float2 gridUV = IN.worldPos.xz;
				gridUV.x *= 1 / (4 * 8.66025404);
				gridUV.y *= 1 / (2 * 15.0);
				grid = tex2D(_GridTex, gridUV);
			#endif
			
			o.Albedo = c.rgb * grid * _Color; 
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}