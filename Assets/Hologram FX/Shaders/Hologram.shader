Shader "Bytesized/Hologram"
{
	Properties
	{
		[Header(Color)]
		_MainColor ("MainColor", Color) = (1, 0, 0, 1)
		_MainTex ("MainTexture", 2D) = "white" {}

		[Header(General)]
		_Brightness("Brightness", Range(0.1, 6.0)) = 3.0
		_Alpha ("Alpha", Range (0.0, 1.0)) = 1.0
		_Direction ("Direction", Vector) = (0,1,0,0)

		[Header(Scanlines)]
		[Toggle] _ScanEnabled ("Scanlines Enabled", Float) = 1.0
		_ScanTiling ("Scan Tiling", Range(0.01, 1000.0)) = 180
		_ScanSpeed ("Scan Speed", Range(-2.0, 2.0)) = 2.0

		[Space(10)]
		[Header(Glow)]
		[Toggle] _GlowEnabled ("Glow Enabled", Float) = 1.0
		_GlowTiling ("Glow Tiling", Range(0.01, 1.0)) = 0.32
		_GlowSpeed ("Glow Speed", Range(-10.0, 10.0)) = -2.8

		[Space(10)]
		[Header(Distortion)]
		[Toggle]_DistortionEnabled ("Distortion Enabled", Float) = 1.0
		_DistortionSpeed ("Distort Speed", Range(0, 10)) = 2.0
		_DistortionIntensity ("Distort Intensity", Range(0, 1)) = 0.25

		[Space(10)]
		[Header(Flicker)]
		_FlickerTex ("Flicker Control Texture", 2D) = "white" {}
		_FlickerSpeed ("Flicker Speed", Range(0.01, 10)) = 1.0

		[Space(10)]
		[Header(Fresnel)]
		_FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
		_FresnelPower ("Fresnel Power", Range(0.1, 10)) = 5.0
	}
	SubShader
	{
		Tags {
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100
		ColorMask RGB
        Cull Back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			/* Color */
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainColor;

			/* General */
			float _Brightness;
			float _Alpha;
			float4 _Direction;
		
			/* Glow */
			float _GlowEnabled;
			float _GlowTiling;
			float _GlowSpeed;

			/* Distortion */
			float _DistortionEnabled;
			float _DistortionSpeed;
			float _DistortionIntensity;

			/* Scan */
			float _ScanEnabled;
			float _ScanTiling;
			float _ScanSpeed;

			/* Flicker */
			float _FlickerSpeed;
			sampler2D _FlickerTex;

			/* Fresnel */
			float _FresnelPower;

			/* Struct that holds vertex information, its used as input for the vertex shader */
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			/* Struct that contains the data that is generated from the vertex shader and goes into the fragment shader */
			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldVertex : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float3 worldNormal : NORMAL;
			};

			/* Generates an offset for the vertices in order to create a distortion effect */
			float calculateDistortionMovement(float4 vertex)
			{
				float z = step(0.99, sin(_Time.y * _DistortionSpeed * 0.5));
				float w = step(0.5, sin(_Time.y * 2.0 + vertex.y));
				return _DistortionIntensity * w * z;
			}

			/* Generate a float4 that represents the fresnel effect that */
			float4 calculateFresnelEffect(float3 viewDir, float3 worldNormal)
			{
				half fresnel = 1.0 - saturate(dot(viewDir, worldNormal));
				float4 fresnelColor = _MainColor * pow (fresnel, _FresnelPower);
				return float4(fresnelColor.rgb, fresnel);
			}

			/* Function to calculate the scalines */
			float calculateScanEffect(half directionToVertex)
			{
				float x = frac(directionToVertex * _ScanTiling + _Time.w * _ScanSpeed);
				return step(x, 0.5) * 0.65 * _ScanEnabled;
			}

			/* Calculates the multiplier for the glow effect */
			float calculateGlowEffect(half directionToVertex)
			{
				return frac(directionToVertex * _GlowTiling - _Time.x * _GlowSpeed) * _GlowEnabled;
			}

			/**/
			float calculateFlickerEffect()
			{
				return tex2D(_FlickerTex, _Time * _FlickerSpeed);
			}

			vertexOutput vert (vertexInput v)
			{
				vertexOutput o;

				/* We transform the vertex by an offset in order to create the distortion effect. This only happens if _DistortionEnabled is 1*/
				v.vertex.x += calculateDistortionMovement(v.vertex) * _DistortionEnabled;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldVertex.xyz));

				return o;
			}
			
			fixed4 frag (vertexOutput i) : SV_Target
			{
				fixed4 texColor = tex2D(_MainTex, i.uv);

				half directionToVertex = (dot(i.worldVertex, normalize(float4(_Direction.xyz, 1.0))) + 1) / 2;
				/* Glow */
				float glowEffect = calculateGlowEffect(directionToVertex);
				/* Scanlines */
				float scanEffect = calculateScanEffect(directionToVertex);
				/* Fresnel */
				fixed4 fresnelEffect = calculateFresnelEffect(i.viewDir, i.worldNormal);
				/* Flicker */
				fixed4 flicker = calculateFlickerEffect();

				fixed4 color = (texColor * _MainColor + (glowEffect * 0.35 * _MainColor) + fresnelEffect) * _Brightness;
				color.a = texColor.a * _Alpha * (scanEffect + glowEffect + fresnelEffect.a) * flicker;
				return color;
			}

			ENDCG
		}
	}
}
