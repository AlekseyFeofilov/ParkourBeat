Shader "Sky"
{
	Properties
	{
		_SkyColor("Цвет Неба", Color) = (0.37, 0.52, 0.73, 0)
		_SkyCurvature("Кривизна Неба", Float) = 0.5
		_SkyIntensity("Интенсивность Неба", Float) = 1.75
		
		_HorizonColor("Цвет Горизонта", Color) = (0.89, 0.96, 1, 0)
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 position : POSITION;
		float3 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 position : SV_POSITION;
		float3 texcoord : TEXCOORD0;
	};

	half3 _SkyColor;
	half3 _HorizonColor;
	
	half _SkyIntensity;
	half _SkyCurvature;

	v2f vert(appdata v)
	{
		v2f o;
		o.position = UnityObjectToClipPos(v.position);
		o.texcoord = v.texcoord;
		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		float3 v = normalize(i.texcoord);

		float p = v.y;
		float p1 = 1 - pow(min(1, 1 - p), pow(_SkyCurvature, v.x*v.z));
		float p2 = 1 - p1;

		half3 c_sky = _SkyColor * p1 + _HorizonColor * p2;

		return half4(c_sky * _SkyIntensity, 0);
	}
	ENDCG

	SubShader
	{
		Tags{ "RenderType" = "Skybox" "Queue" = "Background" }
			Pass
		{
			ZWrite Off
			Cull Off
			Fog { Mode Off }
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}