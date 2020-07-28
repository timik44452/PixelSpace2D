Shader "Skybox/SpaceSkybox"{
    Properties{
        _CamSpeed0("Camera speed", Float) = 0.001
        _CamSpeed1("Camera speed", Float) = 0.001

        [Gamma] _Exposure0("Exposure 0", Range(0, 8)) = 1.0
        [Gamma] _Exposure1("Exposure 1", Range(0, 8)) = 1.0
        
        _Offset0("Offset 0", Vector) = (0, 0, 1, 1)
        _Offset1("Offset 1", Vector) = (0, 0, 1, 1)
        
        [NoScaleOffset] _MainTex("Texture", 2D) = "grey" {}
    }

        SubShader{
            Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
            Cull Off ZWrite Off

            CGINCLUDE
            #include "UnityCG.cginc"

        half _Exposure0;
        half _Exposure1;

        float _CamSpeed0;
        float _CamSpeed1;

        float4 _Offset0;
        float4 _Offset1;

        struct appdata_t {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;

            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f {
            float4 vertex : SV_POSITION;
            float2 texcoord : TEXCOORD0;

            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert(appdata_t v)
        {
            v2f o;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            o.vertex = UnityObjectToClipPos(v.vertex);
            o.texcoord = v.texcoord;

            return o;
        }

        float4 skybox_frag(v2f i, sampler2D smp, half4 smpDecode)
        {
            float4 tex0 = tex2D(smp, _Offset0.xy + i.texcoord * _Offset0.zw + _WorldSpaceCameraPos.xy * _CamSpeed0);
            float4 tex1 = tex2D(smp, _Offset1.xy + i.texcoord * _Offset1.zw + _WorldSpaceCameraPos.xy * _CamSpeed1);

            float3 c0 = DecodeHDR(tex0, smpDecode) * _Exposure0 * unity_ColorSpaceDouble.rgb;
            float3 c1 = DecodeHDR(tex1, smpDecode) * _Exposure1 * unity_ColorSpaceDouble.rgb;

            return float4(max(c0.r, c1.r), max(c0.g, c1.g), max(c0.b, c1.b), 1);
        }
        ENDCG

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _MainTex;
            float4 _MainTex_HDR;
            float4 frag(v2f i) : SV_Target { return skybox_frag(i,_MainTex, _MainTex_HDR); }
            ENDCG
        }
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _MainTex;
            half4 _MainTex_HDR;
            half4 frag(v2f i) : SV_Target { return half4(0, 0, 0, 0); }
            ENDCG
        }
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _MainTex;
            half4 _MainTex_HDR;
            half4 frag(v2f i) : SV_Target{ return half4(0, 0, 0, 0); }
            ENDCG
        }
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _MainTex;
            half4 _MainTex_HDR;
            half4 frag(v2f i) : SV_Target { return half4(0, 0, 0, 0); }
            ENDCG
        }
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _MainTex;
            half4 _MainTex_HDR;
            half4 frag(v2f i) : SV_Target { return half4(0, 0, 0, 0); }
            ENDCG
        }
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            sampler2D _MainTex;
            half4 _MainTex_HDR;
            half4 frag(v2f i) : SV_Target { return half4(0, 0, 0, 0); }
            ENDCG
        }
    }
}
