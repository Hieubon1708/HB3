Shader "Custom/FadeBackOut"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _Size ("Size", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Sử dụng blending cho hiệu ứng trong suốt

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = _Color.w;

                // x left
                if (i.uv.x < _Size)
                {
                    alpha = smoothstep(1, 0, distance(i.uv.x, _Size) / _Size) * _Color.w;
                }
                
                // x right
                if (i.uv.x > 1 - _Size)
                {
                    alpha = smoothstep(1, 0, distance(i.uv.x, 1 - _Size) / _Size) * _Color.w;
                }

                // y bottom
                if (i.uv.y < _Size)
                {
                    alpha = smoothstep(1, 0, distance(i.uv.y, _Size) / _Size) * _Color.w;
                }

                // y top
                if (i.uv.y > 1 - _Size)
                {
                    alpha = smoothstep(1, 0, distance(i.uv.y, 1 - _Size) / _Size) * _Color.w;
                }

                // x left y bottom
                if(i.uv.x < _Size && i.uv.y < _Size) 
                {
                    float dist = distance(i.uv, float2(_Size, _Size));
                    if(dist > _Size) alpha = 0;
                    else alpha = smoothstep(1, 0, dist / _Size) * _Color.w;
                }
                
                // x left y top
                if(i.uv.x < _Size && i.uv.y > 1 - _Size) 
                {
                    float dist = distance(i.uv, float2(_Size, 1 - _Size));
                    if(dist > _Size) alpha = 0;
                    else alpha = smoothstep(1, 0, dist / _Size) * _Color.w;
                }

                // x right y bottom
                if(i.uv.x > 1 - _Size && i.uv.y < _Size) 
                {
                    float dist = distance(i.uv, float2(1 - _Size, _Size));
                    if(dist > _Size) alpha = 0;
                    else alpha = smoothstep(1, 0, dist / _Size) * _Color.w;
                }
                
                // x right y top
                if(i.uv.x > 1 - _Size && i.uv.y > 1 - _Size) 
                {
                    float dist = distance(i.uv, float2(1 - _Size, 1 - _Size));
                    if(dist > _Size) alpha = 0;
                    else alpha = smoothstep(1, 0, dist / _Size) * _Color.w;
                }

                fixed4 finalColor = _Color;
                finalColor.a = alpha;

                return finalColor;
            }
            ENDCG
        }
    }
}