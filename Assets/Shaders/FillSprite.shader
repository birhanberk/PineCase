Shader "Custom/RadialFill"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0,1)) = 1
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _FillAmount;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord - 0.5;
                float angle = atan2(uv.y, -uv.x);

                angle = (angle + 3.14159265) / (2.0 * 3.14159265);
                angle = fmod(angle + 0.25, 1.0);

                fixed4 texColor = tex2D(_MainTex, i.texcoord) * _Color;

                if (angle > _FillAmount)
                {
                    float gray = dot(texColor.rgb, float3(0.3, 0.59, 0.11));
                    return fixed4(gray, gray, gray, texColor.a);
                }

                return texColor;
            }
            ENDCG
        }
    }
}