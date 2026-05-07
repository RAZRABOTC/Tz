Shader "Unify/UI/Tinted Blur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        
        _BlurSize ("Blur Size", Range(1, 5)) = 3
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        _TintIntensity ("Tint Intensity", Range(0, 1)) = 0.5

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "TintedBlur"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _GlobalUniversalBlurTexture;
            float4 _MainTex_ST;
            float4 _ClipRect;
            fixed4 _TextureSampleAdd;
            
            float _BlurSize;
            float4 _TintColor;
            float _TintIntensity;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color;
                OUT.screenPos = ComputeScreenPos(OUT.vertex);
                
                return OUT;
            }

            float4 blurRadius1(float2 uv, float2 texel)
            {
                float4 result = 0;
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 1) * texel);
                return result / 9.0;
            }
            
            float4 blurRadius2(float2 uv, float2 texel)
            {
                float4 result = 0;
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 2) * texel);
                return result / 25.0;
            }
            
            float4 blurRadius3(float2 uv, float2 texel)
            {
                float4 result = 0;
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3,-3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3,-2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3,-1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3, 0) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3, 1) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3, 2) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-3, 3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-2, 3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2(-1, 3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 0, 3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 1, 3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 2, 3) * texel);
                result += tex2D(_GlobalUniversalBlurTexture, uv + float2( 3, 3) * texel);
                return result / 49.0;
            }
            
            float4 blurRadius4(float2 uv, float2 texel)
            {
                float4 result = 0;
                for (int u = -4; u <= 4; u++)
                for (int v = -4; v <= 4; v++)
                {
                    result += tex2D(_GlobalUniversalBlurTexture, uv + float2(u, v) * texel);
                }
                return result / 81.0;
            }
            
            float4 blurRadius5(float2 uv, float2 texel)
            {
                float4 result = 0;
                for (int u = -5; u <= 5; u++)
                for (int v = -5; v <= 5; v++)
                {
                    result += tex2D(_GlobalUniversalBlurTexture, uv + float2(u, v) * texel);
                }
                return result / 121.0;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 spriteColor = tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd;
                float alpha = spriteColor.a * IN.color.a;
                
                #ifdef UNITY_UI_CLIP_RECT
                alpha *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
                
                #ifdef UNITY_UI_ALPHACLIP
                clip(alpha - 0.001);
                #endif
                
                if (alpha < 0.05)
                    return fixed4(0, 0, 0, 0);
                
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                float2 texelSize = 1.0 / _ScreenParams.xy;
                
                int radius = (int)_BlurSize;
                radius = clamp(radius, 1, 5);
                
                float4 blurredColor;
                
                if (radius == 1)
                    blurredColor = blurRadius1(screenUV, texelSize);
                else if (radius == 2)
                    blurredColor = blurRadius2(screenUV, texelSize);
                else if (radius == 3)
                    blurredColor = blurRadius3(screenUV, texelSize);
                else if (radius == 4)
                    blurredColor = blurRadius4(screenUV, texelSize);
                else
                    blurredColor = blurRadius5(screenUV, texelSize);
                
                half4 tintedBlur = blurredColor * _TintColor;
                half4 finalColor = lerp(blurredColor, tintedBlur, _TintIntensity);
                
                finalColor *= spriteColor * IN.color;
                finalColor.a = alpha;
                
                return finalColor;
            }
            ENDCG
        }
    }
}