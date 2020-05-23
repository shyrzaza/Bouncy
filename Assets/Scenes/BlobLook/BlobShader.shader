Shader "Bouncy/BlobShader"
{
    Properties
    {
		_HardTex ("Hard Texture", 2D) = "white" {}
		_HardTint ("Hard Tint", Color) = (0.0, 0.6, 0.0, 1.0)
		_MediumTint("Medium Tint", Color) = (0.0, 0.7, 0.0, 1.0)
		_SoftCol ("Soft Tint", Color) = (0.0, 0.8, 0.0, 1.0)
		_Hardness("Hardness", Range(-1, 1)) = 0
		_Ambient("Ambient", Range(0, 1)) = 0.2
		_BowlingGloss ("Bowling Gloss", Range(0, 1)) = 0.35
		_MediumGloss ("Medium Gloss", Range(0, 0.2)) = 0.05
    }
    SubShader
    {
        Tags { 
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjector"="True" // What does this do?
			"CanUseSpriteAtlas" = "False" // Fuck that
			"LightMode" = "ForwardBase"
		}

		Lighting On // What does this do?
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 col: COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 col: COLOR;
				float4 uv_ext : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.col = v.col;
				o.uv_ext.zw = UnityObjectToWorldDir(float3(v.uv * 2 - float2(1, 1), 0)); // 5Head
				o.uv_ext.xy = v.uv;

                return o;
            }

			float4 _HardTint;
			float4 _MediumTint;
			float _Ambient;
			sampler2D _HardTex;
			float _BowlingGloss;
			float _MediumGloss;
			float4 _SoftCol;

			float4 soft_hard(float nl_raw) {
				
				float gloss = 0;

				if (nl_raw > 0.7) {
					gloss = 0.2;
				} else if (nl_raw > 0.4) {
					gloss = 0.1;
				}
				else if (nl_raw < -0.6) {
					gloss = -0.2;
				}
				else if (nl_raw < -0.2) {
					gloss = -0.1;
				}

				return float4(_SoftCol.rgb + gloss, _SoftCol.a);
			}

			float4 medium_hard(float nl) {
				float gloss = step(0.5, nl) * _MediumGloss;
				return float4(_MediumTint.rgb * (nl + _Ambient) + gloss, 1);
			}

			float4 hard_hard(float2 uv, float nl) {
				float4 col = tex2D(_HardTex, uv) * _HardTint;
				float gloss = step(0.6, nl) * _BowlingGloss;
				return float4(col.rgb * (nl + _Ambient) + gloss, col.a);
			}

			float _Hardness;

            fixed4 frag (v2f i) : SV_Target
            {
				// Transform UV to half-sphere normal
				float nangle = min(1, length(i.uv_ext.zw));
				float3 nor = float3(i.uv_ext.zw, -1 + nangle * nangle);

				// NL lighting for directional light source
				float nl_raw = dot(nor, _WorldSpaceLightPos0.xyz);
				float nl = max(0, nl_raw);

				float4 soft = soft_hard(nl_raw);
				float4 medium = medium_hard(nl);
				float4 hard = hard_hard(i.uv_ext.xy, nl);

				float4 col = lerp(soft, lerp(medium, hard, smoothstep(0, 1, saturate(_Hardness)) * hard.a), min(1, _Hardness + 1)); 

				return float4(col.rgb * _LightColor0.rgb * i.col.rgb, col.a * i.col.a);
            }
            ENDCG
        }
    }
}
