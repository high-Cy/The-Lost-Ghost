
Shader "Custom/Cel Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_specN("specN", Float) = 10.0
		_Threshold("Threshold of toon", range(0,1)) = 0.75
		_Step("Number of steps", Range(0,6)) = 4
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Outline("Thick of Outline",range(0,4))=0.002
		_OutlineColor ("Outline Color", Color) = (0.0, 0.0, 0.0, 1)
	}	
	SubShader
	{
		Pass 
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
             
            Fog {Mode Off}
            ZWrite On ZTest LEqual Cull Off
            Offset 1, 1
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
 
            struct v2f 
            { 
                V2F_SHADOW_CASTER;
            };
 
            v2f vert( appdata_base v )
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
 
            float4 frag( v2f i ) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
		Pass{
			
            
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "Lighting.cginc"

			float _Outline;
			uniform float4 _OutlineColor;

			struct vertIn
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;

            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
            };

			vertOut vert(vertIn v)
            {
                vertOut o;

				float3 worldNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.xyz += normalize(worldNormal).xyz * _Outline;
				

                return o;
            }
			float4 frag (vertOut v) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG

		}
		Pass
		{
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "Lighting.cginc"

			fixed4 _Color;
			uniform float _specN;
			uniform sampler2D _MainTex;
			uniform float _Step;
			uniform float _Threshold;
			uniform float4 _SpecularColor;
			

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};

			struct vertOut 
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};
			
			
			vertOut vert (vertIn v)
			{
				vertOut o;

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}


			float4 frag (vertOut v) : SV_Target
			{
				float4 unlitColor = tex2D(_MainTex, v.uv);
				float3 L = normalize(_WorldSpaceLightPos0.xyz- v.worldVertex.xyz);
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 H = normalize(L+V);
				float VdotN = saturate(dot(V, normalize(v.worldNormal)));
				float HdotN = saturate(dot(H, normalize(v.worldNormal)));
				float LdotN = saturate(dot(L, normalize(v.worldNormal)));

				float lightIntensity;
				
				lightIntensity = floor(max(0, LdotN)*_Step )/_Step;
				fixed3 albedo = unlitColor.rgb * _Color.rgb;

				float3 dif = _LightColor0.rgb * albedo * lightIntensity;
				
				float3 amb = albedo * UNITY_LIGHTMODEL_AMBIENT.rgb;
				

				//Spe part
				float specularIntensity = pow(HdotN * lightIntensity, _specN*_specN);

				float3 spe = _LightColor0.rgb * smoothstep(0.005, 0.01, specularIntensity) * _SpecularColor;


				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = dif.rgb + amb.rgb+ spe.rgb;

				returnColor.a = unlitColor.a;

				return returnColor;
			}
			ENDCG
		}
	}
}