Shader "Custom/RetroCRT"
{
    Properties
    {
        _NoiseAmount ("Noise Amount", Range(0,1)) = 0.25
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.4
        _ScanlineDensity ("Scanline Density", Range(200,2000)) = 800
        _ScanlineSpeed ("Scanline Scroll Speed", Range(0,10)) = 2
        _VignetteIntensity ("Vignette Intensity", Range(0,1)) = 0.3
        _ChromaticAberration ("Chromatic Aberration", Range(0,2)) = 0.4
        _BarrelDistortion ("Barrel Distortion", Range(-0.5,0.5)) = 0.07
        _StaticFlicker ("Flicker", Range(0,1)) = 0.15
        _Enabled ("Enabled", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        ZWrite Off Cull Off ZTest Always

        Pass
        {
            Name "RetroCRT"
            HLSLPROGRAM
            #pragma vertex Vert              // provided by Blit.hlsl
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // DO NOT redeclare _BlitTexture or its sampler; Blit.hlsl already does

            float _NoiseAmount;
            float _ScanlineIntensity;
            float _ScanlineDensity;
            float _ScanlineSpeed;
            float _VignetteIntensity;
            float _ChromaticAberration;
            float _BarrelDistortion;
            float _StaticFlicker;
            float _Enabled;

            // ----- helper funcs (unchanged) -----
            float Hash21(float2 p)
            {
                p = frac(p * float2(123.34, 345.45));
                p += dot(p, p + 34.345);
                return frac(p.x * p.y);
            }

            float2 BarrelDistort(float2 uv, float k)
            {
                float2 c = uv * 2.0 - 1.0;
                float r2 = dot(c,c);
                c *= (1.0 + k * r2);
                return c * 0.5 + 0.5;
            }

            float3 ChromaticSample(float2 uv, float2 dir, float amt)
            {
                // _BlitTexture & sampler_LinearClamp come from Blit.hlsl
                float3 col;
                col.r = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + dir * amt).r;
                col.g = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).g;
                col.b = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv - dir * amt).b;
                return col;
            }

            float3 ApplyScanlines(float3 col, float2 uv, float t, float density, float intensity, float speed)
            {
                float s = 0.5 + 0.5 * sin((uv.y * density) + t * speed * 6.28318);
                float scan = lerp(1.0, s, intensity);
                return col * scan;
            }

            float3 ApplyVignette(float3 col, float2 uv, float intensity)
            {
                float2 d = uv - 0.5;
                float r = dot(d,d);
                float vig = smoothstep(0.75, 0.0, r);
                return lerp(col * vig, col, 1.0 - intensity);
            }

            float3 AddStatic(float3 col, float2 uv, float t, float amount, float flicker)
            {
                float n = Hash21(uv * float2(1024, 768) + t * 60.0);
                float f = lerp(1.0, 1.0 + (n - 0.5) * 0.5, amount);
                float lf = 1.0 + (sin(t * 120.0) * 0.5 + 0.5) * flicker * 0.1;
                return col * f * lf;
            }

            // Use Varyings defined in Blit.hlsl
            half4 Frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.texcoord;      // note: in Blit.hlsl, the field is texcoord
                if (_Enabled < 0.5)
                    return SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                float t = _Time.y;

                if (abs(_BarrelDistortion) > 0.0001)
                    uv = BarrelDistort(uv, _BarrelDistortion);

                float2 dir = normalize(uv - 0.5);
                float3 col = ChromaticSample(uv, dir, _ChromaticAberration * 0.0025);
                col = ApplyScanlines(col, uv, t, _ScanlineDensity, _ScanlineIntensity, _ScanlineSpeed);
                col = AddStatic(col, uv, t, _NoiseAmount, _StaticFlicker);
                col = ApplyVignette(col, uv, _VignetteIntensity);

                return half4(col, 1);
            }
            ENDHLSL
        }
    }
    Fallback Off
}
