// Toony Colors Pro+Mobile 2
// (c) 2014-2023 Jean Moreno

Shader "Minimal"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[TCP2Separator]

		[TCP2HeaderHelp(Rim Lighting)]
		[Toggle(TCP2_RIM_LIGHTING)] _UseRim ("Enable Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1
		[TCP2Separator]
		
		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"DisableBatching" = "True"
			"RenderType"="Opaque"
		}

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Texture/Sampler abstraction
		#define TCP2_TEX2D_WITH_SAMPLER(tex)						UNITY_DECLARE_TEX2D(tex)
		#define TCP2_TEX2D_NO_SAMPLER(tex)							UNITY_DECLARE_TEX2D_NOSAMPLER(tex)
		#define TCP2_TEX2D_SAMPLE(tex, samplertex, coord)			UNITY_SAMPLE_TEX2D_SAMPLER(tex, samplertex, coord)
		#define TCP2_TEX2D_SAMPLE_LOD(tex, samplertex, coord, lod)	UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex, samplertex, coord, lod)

		// Shader Properties
		fixed4 _Color;
		fixed4 _HColor;
		fixed4 _SColor;
		float _RimMin;
		float _RimMax;
		fixed4 _RimColor;

		ENDCG

		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha noambient noforwardadd novertexlights interpolateview halfasview nolppv
		#pragma instancing_options nolodfade nolightprobe
		#pragma target 2.0

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature_local_fragment TCP2_RIM_LIGHTING

		//================================================================
		// STRUCTS

		// Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			half4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			half3 worldNormal; INTERNAL_DATA
		};

		//================================================================

		// Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 worldNormal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;
			half ndv;
			half ndvRaw;

			Input input;

			// Shader Properties
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
			float __rimMin;
			float __rimMax;
			float3 __rimColor;
			float __rimStrength;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			// Shader Properties Sampling
			float4 __albedo = ( float4(1,1,1,1) );
			float4 __mainColor = ( _Color.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );
			output.__rimMin = ( _RimMin );
			output.__rimMax = ( _RimMax );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );

			output.input = input;

			half3 worldNormal = WorldNormalVector(input, output.Normal);
			output.worldNormal = worldNormal;

			half ndv = abs(dot(input.viewDir, normalize(output.Normal.xyz)));
			half ndvRaw = ndv;
			output.ndv = ndv;
			output.ndvRaw = ndvRaw;

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;

			output.Albedo *= __mainColor.rgb;

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{

			half ndv = surface.ndv;
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				// extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			half3 ramp;
			
			ndl = saturate(ndl);
			ramp = ndl.xxx;

			// Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;

			// Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			// Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			// Rim Lighting
			#if defined(TCP2_RIM_LIGHTING)
			#if !defined(UNITY_PASS_FORWARDADD)
			half rim = 1 - surface.ndvRaw;
			rim = ( rim );
			half rimMin = surface.__rimMin;
			half rimMax = surface.__rimMax;
			rim = smoothstep(rimMin, rimMax, rim);
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			color.rgb += rim * rimColor * rimStrength;
			#endif
			#endif

			return color;
		}

		// Same as UnityGI_Base but with attenuation extraction that works with lightmaps
		inline UnityGI UnityGI_Base_TCP2(UnityGIInput data, half occlusion, half3 normalWorld, out half tcp2_atten)
		{
			UnityGI o_gi;
			ResetUnityGI(o_gi);

			// Base pass with Lightmap support is responsible for handling ShadowMask / blending here for performance reason
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
				half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
				float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
				float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
				data.atten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif

			o_gi.light = data.light;

			// TCP2: don't apply attenuation to light color
			// o_gi.light.color *= data.atten;

			// TCP2: extract attenuation
			tcp2_atten = data.atten;

			#if UNITY_SHOULD_SAMPLE_SH
				o_gi.indirect.diffuse = ShadeSHPerPixel(normalWorld, data.ambient, data.worldPos);
			#endif

			#if defined(LIGHTMAP_ON)
				// Baked lightmaps
				half4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, data.lightmapUV.xy);
				half3 bakedColor = DecodeLightmap(bakedColorTex);

				#ifdef DIRLIGHTMAP_COMBINED
					fixed4 bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER (unity_LightmapInd, unity_Lightmap, data.lightmapUV.xy);
					o_gi.indirect.diffuse += DecodeDirectionalLightmap (bakedColor, bakedDirTex, normalWorld);

					#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
						ResetUnityLight(o_gi.light);
						o_gi.indirect.diffuse = SubtractMainLightWithRealtimeAttenuationFromLightmap (o_gi.indirect.diffuse, data.atten, bakedColorTex, normalWorld);
					#endif

				#else // not directional lightmap
					o_gi.indirect.diffuse += bakedColor;

					#if defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN)
						ResetUnityLight(o_gi.light);
						o_gi.indirect.diffuse = SubtractMainLightWithRealtimeAttenuationFromLightmap(o_gi.indirect.diffuse, data.atten, bakedColorTex, normalWorld);
					#endif

				#endif
			#endif

			#ifdef DYNAMICLIGHTMAP_ON
				// Dynamic lightmaps
				fixed4 realtimeColorTex = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, data.lightmapUV.zw);
				half3 realtimeColor = DecodeRealtimeLightmap (realtimeColorTex);

				#ifdef DIRLIGHTMAP_COMBINED
					half4 realtimeDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, data.lightmapUV.zw);
					o_gi.indirect.diffuse += DecodeDirectionalLightmap (realtimeColor, realtimeDirTex, normalWorld);
				#else
					o_gi.indirect.diffuse += realtimeColor;
				#endif
			#endif

			o_gi.indirect.diffuse *= occlusion;
			return o_gi;
		}

		inline UnityGI UnityGlobalIllumination_TCP2 (UnityGIInput data, half occlusion, half3 normalWorld, out half tcp2_atten)
		{
			return UnityGI_Base_TCP2(data, occlusion, normalWorld, tcp2_atten);
		}

		inline UnityGI UnityGlobalIllumination_TCP2 (UnityGIInput data, half occlusion, half3 normalWorld, Unity_GlossyEnvironmentData glossIn, out half tcp2_atten)
		{
			UnityGI o_gi = UnityGI_Base_TCP2(data, occlusion, normalWorld, tcp2_atten);
			o_gi.indirect.specular = UnityGI_IndirectSpecular(data, occlusion, glossIn);
			return o_gi;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			// GI without reflection probes
			half tcp2_atten;
			gi = UnityGlobalIllumination_TCP2(data, 1.0, normal, tcp2_atten); // occlusion is applied in the lighting function, if necessary

			surface.atten = tcp2_atten; // transfer attenuation to lighting function

		}

		ENDCG

	}

	Fallback "Diffuse"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(ver:"2.9.6";unity:"2022.3.16f1";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2019_4","UNITY_2020_1","UNITY_2021_1","UNITY_2021_2","UNITY_2022_2","NO_RAMP","SPECULAR_SHADER_FEATURE","RIM","RIM_SHADER_FEATURE","ENABLE_FOG","DISABLE_BATCHING","ENABLE_LIGHTMAPS"];flags:list["noambient","noforwardadd","novertexlights","interpolateview","halfasview"];flags_extra:dict[pragma_gpu_instancing=list["nolodfade","nolightprobe"]];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="2.0",RIM_LABEL="Rim Lighting"];shaderProperties:list[sp(name:"Albedo";imps:list[imp_constant(type:color_rgba;fprc:float;fv:1;f2v:(1, 1);f3v:(1, 1, 1);f4v:(1, 1, 1, 1);cv:RGBA(1, 1, 1, 1);guid:"dab463b6-50ff-4b3b-b42a-f9cf4f651f96";op:Multiply;lbl:"Albedo";gpu_inst:False;locked:False;impl_index:-1)];layers:list[];unlocked:list[];clones:dict[];isClone:False)];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH 24a1a9216a5bd0ea34221e8b12b0f9a7 */
