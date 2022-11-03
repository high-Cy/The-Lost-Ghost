
Shader "CustomRenderTexture/DepthFogShader"
{
    Properties
    {
        //_FogColor ("FogColor", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
		//_FogDensity ("FogDensity", Range(0,1)) = 0.5
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always            
        
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            uniform sampler2D _MainTex;
			uniform float4 _FogColor;
			uniform float _FogDensity;

			sampler2D _CameraDepthTexture;

            struct vertIn
            {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;

            };

            struct vertOut
            {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
            };

			vertOut vert(vertIn v)
            {
				 vertOut o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.uv = v.uv;
			     return o;
            };

            float4 frag(vertOut v) : SV_Target
            {
                float4 unlitColor = tex2D(_MainTex, v.uv);
                
				float dis = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,v.uv));
				dis = LinearEyeDepth(dis);

                //dis = Linear01Depth(dis);

                float fogDensity = lerp(0.7, _FogDensity, step(dis, 500));
                float fogFactor = saturate(exp(-pow(fogDensity*0.1*dis,2)));
                float3 afterFog = lerp(_FogColor.rgb, unlitColor.rgb, fogFactor);
				return float4(afterFog, unlitColor.a);
            };
            ENDCG
        }
    }
}
