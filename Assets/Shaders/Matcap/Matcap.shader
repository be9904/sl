Shader "MatCap"
{
    Properties
    {
        [NoScaleOffset] _MatCap ("MatCap", 2D) = "white" {}
        [KeywordEnum(ViewSpaceNormal, ViewDirectionCross, ViewDirectionAligned)] _MatCapType ("Matcap UV Type", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _ _MATCAPTYPE_VIEWSPACENORMAL _MATCAPTYPE_VIEWDIRECTIONCROSS

            // #include "UnityCG.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 clipPos : SV_POSITION;
                float3 worldSpaceNormal : TEXCOORD0;
                float3 worldSpacePos : TEXCOORD1;
            };

            Texture2D _MatCap;
            SamplerState sampler_MatCap;

            v2f vert (Attributes v)
            {
                v2f o;
                o.clipPos = TransformObjectToHClip(v.vertex);
                o.worldSpaceNormal = TransformObjectToWorldNormal(v.normal);
                o.worldSpacePos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float3 worldSpaceNormal = normalize(i.worldSpaceNormal);

            #if defined(_MATCAPTYPE_VIEWSPACENORMAL)
                // basic matcap using the viewspace normal
                // 10 math instructions

                float3 viewSpaceNormal = mul((float3x3)UNITY_MATRIX_V, worldSpaceNormal);
                float2 matcapUV = normalize(viewSpaceNormal).xy;

            #elif defined(_MATCAPTYPE_VIEWDIRECTION)
                // usual fix that uses a cross product of the view direction and view normal
                // 17 math instructions

                float3 viewSpaceNormal = mul((float3x3)UNITY_MATRIX_V, worldSpaceNormal);
                float3 viewSpaceViewDir = normalize(mul(UNITY_MATRIX_V, float4(i.worldSpacePos, 1.0)));
                float2 matcapUV = cross(viewSpaceViewDir, viewSpaceNormal).yx;
                matcapUV.x = -matcapUV.x;

            #else // _MATCAPTYPE_VIEWDIRECTIONALIGNED
                // improved technique that uses a view direction aligned normal
                // 17 math instructions

                float3 worldSpaceViewDir = normalize(i.worldSpacePos - _WorldSpaceCameraPos.xyz);

                float3 up = mul((float3x3)UNITY_MATRIX_I_V, float3(0,1,0));
                float3 right = normalize(cross(up, worldSpaceViewDir));
                up = cross(worldSpaceViewDir, right);
                float2 matcapUV = mul(float3x3(right, up, worldSpaceViewDir), worldSpaceNormal).xy;

            #endif

                // remap from -1 .. 1 to 0 .. 1
                matcapUV = matcapUV * 0.5 + 0.5;

                return _MatCap.Sample(sampler_MatCap, matcapUV);
            }
            ENDHLSL
        }
    }
}