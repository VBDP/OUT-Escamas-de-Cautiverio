Shader "Custom/GLKitty_LavaFullRed"
{
    Properties
    {
        _MainTex ("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            float3 rotateY(float3 v, float t)
            {
                float cost = cos(t);
                float sint = sin(t);
                return float3(v.x * cost + v.z * sint, v.y, -v.x * sint + v.z * cost);
            }

            float smin(float a, float b, float k)
            {
                float h = saturate(0.5 + 0.5*(b-a)/k);
                return lerp(b, a, h) - k*h*(1.0-h);
            }

            float noise(float3 p)
            {
                float t = _Time.y;
                float3 np = normalize(p);

                float a = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(t/20.0 + np.x, t/20.0 + np.y)).r;
                float b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(t/20.0 + 0.77 + np.y, t/20.0 + np.z)).r;

                a = lerp(a, 0.5, abs(np.x));
                b = lerp(b, 0.5, abs(np.z));

                float n = a + b - 0.4;
                n = lerp(n, 0.5, abs(np.y)*0.5);
                return n;
            }

            float map(float3 p)
            {
                float d = (-1.0 * length(p) + 3.0) + 1.5 * noise(p);
                d = min(d, (length(p) - 1.5) + 1.5 * noise(p));

                float m = 1.5;
                float s = 0.03;

                d = smin(d, max(abs(p.x)-s, abs(p.y+p.z*0.2)-0.07), m);
                d = smin(d, max(abs(p.z)-s, abs(p.x+p.y*0.5)-0.07), m);
                d = smin(d, max(abs(p.z-p.y*0.4)-s, abs(p.x-p.y*0.2)-0.07), m);
                d = smin(d, max(abs(p.z*0.2-p.y)-s, abs(p.x+p.z)-0.07), m);
                d = smin(d, max(abs(p.z*-0.2+p.y)-s, abs(-p.x+p.z)-0.07), m);

                return d;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float2 fragCoord = i.uv * _ScreenParams.xy;

                float2 uv = fragCoord * 2.0 / _ScreenParams.xy - 1.0;
                uv.x *= _ScreenParams.x / _ScreenParams.y;

                float3 ray = normalize(float3(uv.x, uv.y, 1.0));

                // RED BASE COLOR: dark red in empty spaces
                float3 col = float3(0.1,0.0,0.0);

                const int rayCount = 48;
                float t = 0;

                for (int r = 0; r < rayCount; r++)
                {
                    float3 p = float3(0,0,-3) + ray * t;
                    p = rotateY(p, _Time.y/3.0);

                    float mask = max(0.0,(1.0-length(p/3.0)));
                    p = rotateY(p, mask*sin(_Time.y*0.5)*1.2);
                    p.y += sin(_Time.y+p.x)*mask*0.5;
                    p *= 1.1+(sin(_Time.y*0.5)*mask*0.3);

                    float d = map(p);

                    if(d < 0.01)
                    {
                        float iter = (float)r / rayCount;
                        float ao = 1.0 - pow(1.0-iter,2.0);

                        float m2 = max(0.0,(1.0-length(p/2.0)));
                        m2 *= abs(sin(_Time.y*-1.5+length(p)+p.x)-0.2);

                        // LAVA COLORS: bright and glowing red/orange
                        col += float3(0.9,0.2,0.0)*max(0.0,(noise(p)*2.0-1.3))*m2; // glowing lava
                        col += float3(1.0,0.4,0.0)*ao*2.0; // ambient glow
                        col += float3(0.5,0.1,0.0)*(t/8.0); // depth gradient

                        break;
                    }

                    t += d*0.5;
                }

                // Vignette, subtle
                float2 uv2 = i.uv;
                uv2 *= 1.0 - uv2.yx;
                float vig = pow(uv2.x * uv2.y * 20.0, 0.25);
                col *= vig*0.5 + 0.5;

                // Clamp final color
                col = saturate(col);

                return float4(col,1.0);
            }

            ENDHLSL
        }
    }
}