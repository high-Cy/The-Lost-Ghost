Shader "Custom/TerrainShader"
{
    Properties
    {
        _TopColor ("TopColor", Color) = (0.5,0.5,0.5,1)
		_MiddleColor ("MiddleColor", Color) = (0,1,0,0)
		_BottomColor ("BottomColor", Color) = (0,1,0,0)
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _TopThershold("Top Thershold height", range(0,20)) = 17
		_BottomThershold("Bottom Thershold height", range(0,20)) = 7
		_TopGradient("Scale of Top Gradient", range(0.001,10)) = 3
		_BottomGradient("Scale of Bottom Gradient", range(0.001,10)) = 3
		_Specular("Specular",Color) = (1,1,1,1)
		_SpecN("SpecN", range(0,50)) = 20
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
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "unitycg.cginc"
			#include "Lighting.cginc"

			fixed4 _TopColor;
			float _TopThershold;
			fixed4 _MiddleColor;
			float _BottomThershold;
			fixed4 _BottomColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _TopGradient;
			float _BottomGradient;
			fixed4 _Specular;
			float _SpecN;

			struct vertIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertOut {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldVertex : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float y :TEXCOORD3;
			};
			
			
			vertOut vert (vertIn v)
			{
				vertOut o;

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				// Transform vertex in world coordinates to camera coordinates, and pass UV
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.y = v.vertex.y;

				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}
			fixed4 frag(vertOut v) : SV_Target
			{
				float4 unlitColor = float4(0.0f, 0.0f, 0.0f, 1.0f);
				if(v.y > _TopThershold){
					unlitColor = _TopColor;
					
				}else if(v.y <= _TopThershold && v.y > _BottomThershold){
					
					float gradient_percent = (_TopThershold - v.y)/_TopGradient;
					if(gradient_percent >= 1){
						unlitColor.rgb = _MiddleColor.rgb;
					}else{
						unlitColor.rgb = lerp(_TopColor, _MiddleColor, gradient_percent);
					}
				}else{
					float gradient_percent = (_BottomThershold - v.y)/_BottomGradient;
					if(gradient_percent >= 1){
						unlitColor.rgb = _BottomColor.rgb;
					}else{
						unlitColor.rgb = lerp(_MiddleColor, _BottomColor, gradient_percent);
					}
				}
				// Our interpolated normal might not be of length 1
				float3 N = normalize(v.worldNormal);

				float3 amb = unlitColor.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb;
	
				//float3 L = normalize(_WorldSpaceLightPos0.xyz- v.worldVertex.xyz);
				//float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				fixed3 L = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 V = normalize(_WorldSpaceCameraPos.xyz - v.worldVertex.xyz);
				fixed3 reflectDir = normalize(reflect(-L, N));
				float LdotN = dot(L, N);
				float3 dif;
				
				dif = _LightColor0.rgb* unlitColor.rgb * saturate(LdotN);
				


				float3 H = normalize(V + L);
				float3 spe = _LightColor0.rgb  * _Specular.rgb * pow(saturate(dot(reflectDir, H)), _SpecN);


				float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
				returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
				returnColor.a = unlitColor.a;

				return returnColor;
			}
			ENDCG
	    }
    }
}
