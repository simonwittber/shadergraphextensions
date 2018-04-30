Shader "Skybox/SpaceBox"
{
    Properties
    {
        _AmbientSky ("Ambient Sky", Color) = (0,0,1,1)
        _TopSkyFalloff("Sky Falloff", Float) = 8.5
        [Space]
        _AmbientHorizon ("Ambient Horizon", Color) = (1,0,1,1)
        [Space]
        _AmbientGround ("Ambient Ground", Color) = (0,1,0,1)
        _BottomSkyFalloff("Ground Falloff", Float) = 3.0
		[Space]
		_AmbientBackground ("Ambient Background", Color) = (0,1,0,1)
		[Space]

        _SkyIntensity("Sky Intensity", Float) = 1.0

        _SunIntensity("Sun Intensity", float) = 2.0

        _SunFalloff("Sun Falloff", float) = 550
        _SunSize("Sun Size", float) = 1

		_Noise("Noise", 3D) = "" {}
        _Persistence("Persistence", Range(0,1)) = 0.4
        _Lacunarity("Lacunarity", Range(0,13)) = 3

    }

    CGINCLUDE

    #include "UnityCG.cginc"
	#include "Lighting.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    half4 _AmbientSky, _AmbientGround, _AmbientHorizon, _AmbientBackground;
    half _TopSkyFalloff;    
    half _BottomSkyFalloff;

    half _SkyIntensity;

    half _SunIntensity;

    half _SunFalloff;
    half _SunSize;

    float _Persistence, _Lacunarity;
	sampler3D _Noise;

	float fBm(float3 pos, float frequency, float persistence, float lacunarity)  {
		float gain = 1;
		int octaves = 5;
		float n = 0;
		for(int i=1; i<octaves; i++) {
			float s = tex3D(_Noise, pos*frequency);
			n += s * gain;
			frequency *= lacunarity;
			gain *= persistence;
		}
		return n;
	}

    v2f vert(appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos(v.position);
        o.texcoord = v.texcoord;
        return o;
    }
    
    half4 frag(v2f i) : COLOR
    {
        float3 v = normalize(i.texcoord);

        float p = v.y;
        float p1 = 1 - pow(min(1, 1 - p), _TopSkyFalloff);
        float p3 = 1 - pow(min(1, 1 + p), _BottomSkyFalloff);
        float p2 = 1 - p1 - p3;

        half3 c_sky = _AmbientSky * p1 + _AmbientHorizon * p2 + _AmbientGround * p3;
        half3 c_sun = _LightColor0 * min(pow(max(0, dot(v, _WorldSpaceLightPos0.xyz)), _SunFalloff) * _SunSize, 1);

		float n = fBm(v, 0.0125, _Persistence, _Lacunarity);
		
		c_sky = lerp(c_sky, _AmbientBackground, pow(1-n,2));
        return half4(c_sky * _SkyIntensity + c_sun * _SunIntensity, 0);

    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    } 
}