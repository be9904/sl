Shader "DepthHack"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			ZTest Always
			Cull Off
			ColorMask 0
	
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			
			struct Attributes
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert (Attributes v)
			{
				v2f o;
				o.vertex = TransformObjectToHClip(v.vertex);
				o.vertex.z = 0;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				return 0;
			}
			ENDHLSL
		}
	}
}
