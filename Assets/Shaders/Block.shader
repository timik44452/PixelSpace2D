Shader "Custom/Block"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_Mask("Mask", 2D) = "white" {}
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque"  }
		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv2 : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv  : TEXCOORD0;
				float2 uv2  : TEXCOORD1;
				//float3 ambient : TEXCOORD1;
				float4 vertex   : SV_POSITION;
				float4 color : COLOR;
				
			};

			float4 _Color;
			sampler2D _Mask;
			sampler2D _MainTex;

			v2f vert(appdata_t i)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(i.vertex);
				OUT.uv = i.uv;
				OUT.uv2 = i.uv2;
				OUT.color = i.color * _Color;
				//OUT.ambient = ShadeSH9(mul(unity_ObjectToWorld, float4(0, 0, -1, 0)));

				return OUT;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv) * tex2D(_Mask, i.uv2) * 0.5 * _Color * i.color;
				
				//col.rgb *= i.ambient;

				clip(col.a - 0.01);

				return col;

			}
			ENDCG
		}
	}
}