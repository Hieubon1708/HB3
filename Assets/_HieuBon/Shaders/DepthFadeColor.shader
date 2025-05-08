Shader "Custom/DistanceFade"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _FarColor ("Far Color", Color) = (0,0,1,1) // Màu xanh rực rỡ
        _FadeDistance ("Fade Distance", Float) = 20 // Khoảng cách bắt đầu mờ dần
        _MaxDistance ("Max Distance", Float) = 100 // Khoảng cách tối đa để đạt màu Far Color
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _MainColor;
        fixed4 _FarColor;
        float _FadeDistance;
        float _MaxDistance;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Tính khoảng cách từ camera đến world position của fragment
            float distanceToCamera = length(_WorldSpaceCameraPos.xyz - IN.worldPos);

            // Tính toán hệ số lerp dựa trên khoảng cách
            float fadeFactor = saturate((distanceToCamera - _FadeDistance) / (_MaxDistance - _FadeDistance));

            // Nội suy màu giữa Main Color và Far Color dựa trên hệ số fade
            fixed4 finalColor = lerp(_MainColor, _FarColor, fadeFactor);

            o.Albedo = finalColor.rgb;
            o.Metallic = 0;
            o.Smoothness = 0.5;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}