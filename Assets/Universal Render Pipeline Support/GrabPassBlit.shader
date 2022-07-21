Shader "Hidden/GrabPassBlit"
{
    Properties { }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Name "FinalBlit"

            ZWrite Off ZTest Always Blend Off Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD0;
            };

            TEXTURE2D(_GSF_RenderPass);
            SAMPLER(sampler_GSF_RenderPass);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.screen_pos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screen_pos.xy / i.screen_pos.w;
                float4 col = SAMPLE_TEXTURE2D(_GSF_RenderPass, sampler_GSF_RenderPass, uv);
                return float4(col.rgb, length(col.rgb) != 0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthBuffer"

            ZTest Always ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o = (v2f)0;

                o.vertex = TransformObjectToHClip(v.vertex);
                o.screen_pos = ComputeScreenPos(o.vertex);

                return o;
            }

            float4 frag(v2f i, out float depth : SV_DEPTH) : SV_TARGET0
            {
                depth = SampleSceneDepth(i.screen_pos.xy / i.screen_pos.w);
                return depth;
            }

            ENDHLSL
        }

        Pass
        {
            Name "AfterPP"

            ZWrite Off ZTest Always Blend Off Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD0;
            };

            TEXTURE2D(_AfterPostProcessTexture);
            SAMPLER(sampler_AfterPostProcessTexture);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.screen_pos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screen_pos.xy / i.screen_pos.w;
                return SAMPLE_TEXTURE2D(_AfterPostProcessTexture, sampler_AfterPostProcessTexture, uv);
            }
            ENDHLSL
        }
    }
}
