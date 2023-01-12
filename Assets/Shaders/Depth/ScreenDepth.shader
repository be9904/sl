Shader "Unlit/ScreenDepth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
		{
            "RenderPipeline"="UniversalPipeline"
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 cpos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            Texture2D _MainTex;
            float4 _MainTex_ST;
            SamplerState sampler_MainTex;

            v2f vert (Attributes v)
            {
                v2f o;
                o.cpos = TransformObjectToHClip(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.screenPos = ComputeScreenPos(o.cpos);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 screenSpaceUV = i.screenPos.xy / i.screenPos.w;
                
                half4 depthCol = _CameraDepthTexture.Sample(sampler_CameraDepthTexture, screenSpaceUV).r;
                float sceneEyeDepth = Linear01Depth(depthCol, _ZBufferParams);
                
                return half4(sceneEyeDepth, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}
