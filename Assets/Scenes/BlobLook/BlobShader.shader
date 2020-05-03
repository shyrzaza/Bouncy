Shader "Bouncy/BlobShader"
{
    Properties
    {
        _ShellNormal ("Shell Normal", 2D) = "bump" {}
		_ShellAlpha("Shell Alpha", 2D) = "white" {}
		_HardTint ("Hard Tint", Color) = (0.0, 0.7, 0.0, 1.0)
		_MediumTint("Medium Tint", Color) = (0.0, 0.7, 0.0, 1.0)
		_Hardness("Hardness", Range(-1, 1)) = 0
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
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 uv_ext : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv_ext.zw = UnityObjectToWorldDir(float3(v.uv * 2 - float2(1, 1), 0)); // 5Head
				o.uv_ext.xy = o.uv_ext.zw * 0.5 + (0.5).rr;

                return o;
            }

			float4 _MediumTint;
			float4 _HardTint;

			sampler2D _ShellNormal;
			sampler2D _ShellAlpha;

			float3 medium_hard(float2 uv_ext) {
				// Transform UV to half-sphere normal
				float nangle = min(1, length(uv_ext));
				float3 nor = float3(uv_ext, -1 + nangle * nangle);

				// NL lighting for directional light source
				float nl = max(0, dot(nor, _WorldSpaceLightPos0.xyz));

				return _MediumTint * (nl.rrr + ShadeSH9(half4(nor, 1)));
			}

			float3 hard_hard(float2 uv, float fadein) {
				float3 nor = tex2D(_ShellNormal, uv);
				nor.x = nor.x * 2 - 1;
				nor.y = nor.y * 2 - 1;
				float nxyl = length(nor.xy);
				nor.z = -1 + nxyl * nxyl;
				// Tight

				// NL lighting for directional light source
				float nl = max(0, dot(nor, _WorldSpaceLightPos0.xyz));

				return _HardTint * (nl.rrr + ShadeSH9(half4(nor, 1)));
			}

			float _Hardness;

            fixed4 frag (v2f i) : SV_Target
            {
				float sat_hardness = smoothstep(0, 1, saturate(_Hardness));

				// Shrink UV 10% to account for clean edges
				float2 shell_uv = (i.uv_ext.xy - (.5).rr) * 1.05 + (.5).rr;

				// Alpha uv affected by fade-in
				float2 scaled_uv = (shell_uv - (.5).rr) * lerp(0.8, 1, sat_hardness) + (.5).rr;

				float a = tex2D(_ShellAlpha, scaled_uv).r;

				float3 soft = float3(1.0, 0.0, 1.0);
				float3 medium = medium_hard(i.uv_ext.zw);
				float3 hard = hard_hard(scaled_uv, sat_hardness);

				return fixed4(lerp(soft, lerp(medium, hard, sat_hardness), min(1, _Hardness + 1)), a);
            }
            ENDCG
        }
    }
}
