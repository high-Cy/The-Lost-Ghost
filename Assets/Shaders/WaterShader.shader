Shader "Unlit/WaterShader"
{
    Properties
    {
		//_Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Texture", 2D) = "blue" {}
        _Amount ("Amount", Range(0,1)) = 0.188
        _Frequency ("Frequency", Range(0,200)) = 20
		_Speed ("Speed", Range(0,500)) = 50	
        
    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _Amount;
			uniform float _Frequency;
			uniform float _Speed;

            struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 uv : TEXCOORD0;
			};

			struct vertOut 
			{
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 worldVertex : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};
			
			
			vertOut vert (vertIn v)
			{
				vertOut o;

				float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
				// Transform vertex in world coordinates to camera coordinates, and pass UV
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				// Pass out the world vertex position and world normal to be interpolated
				// in the fragment shader (and utilised)
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

				return o;
			}

			float4 frag (vertOut v) : SV_Target
			{
				float2 center_uv = {0.5,0.3};
				float2 uv = v.uv;
				float2 dt = center_uv - uv;
				float len = sqrt(dot(dt,dt));
				float amount = max(_Amount, _Amount/ (0.01+len*_Speed));

				if(amount < 0.1){
					amount = 0;
				}
				uv.y += amount * cos(len*_Frequency*UNITY_PI);

				float4 returnColor = tex2D(_MainTex, uv.y);

				return returnColor;
			}
			ENDCG
		}
	}
}
  