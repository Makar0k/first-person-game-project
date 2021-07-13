Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(1.0,5.0)) = 1.01
        _Emission ("Emmisive Color", Color) = (0,0,0,0)
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
        float3 normal : NORMAL;
    };

    float4 _OutlineColor;
    float _OutlineWidth;

    v2f vert(appdata v)
    {
        v.vertex.xyz *= _OutlineWidth;

        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        return o;
    }

    ENDCG

    SubShader
    {
        Pass // Outline
        {
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }
            ENDCG
        }
        Pass // Normal Render
        {
            ZWrite On

            Material
            {
                Diffuse[_Color]
                Ambient[_Color]
                Emission [_Emission]
            }
            Lighting On

            SetTexture[_MainTex]
            {
                ConstantColor[_Color]
                combine previous * texture, previous + texture
            }
            
        }
    }
}
