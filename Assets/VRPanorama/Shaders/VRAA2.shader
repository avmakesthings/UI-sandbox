Shader "VRPanorama/VRAA2" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_U ("U", Float ) = 215
    _V ("V", Float ) = 512
   
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
            uniform float _U;
            uniform float _V;
            half4 _MainTex_TexelSize;

fixed4 frag (v2f_img i) : SV_Target
{
	float _TXWidth = (1.0f / _U);
	float _TXHeight = (1.0f / _V);
	
	
	fixed4 original = tex2D(_MainTex, i.uv);
	
	            float4 c1 = tex2D(_MainTex, i.uv + float2(0.0f, 0.0f));
				float4 c2 = tex2D(_MainTex, i.uv + float2(_TXWidth, 0.0f));
				float4 c3 = tex2D(_MainTex, i.uv + float2(-_TXWidth, _TXHeight));
				float4 c4 = tex2D(_MainTex, i.uv + float2(-_TXWidth, 0.0f));
				float4 c5 = tex2D(_MainTex, i.uv + float2(-_TXWidth, -_TXHeight));
				
				
				float4 d2 = tex2D(_MainTex, i.uv + float2(0.0f, -_TXHeight * 2.0f));
				float4 d3 = tex2D(_MainTex, i.uv + float2(0.0f, _TXHeight * 2.0f));
				float4 d4 = tex2D(_MainTex, i.uv + float2(-_TXWidth * 2.0f, 0.0f));
				float4 d5 = tex2D(_MainTex, i.uv + float2(_TXWidth * 2.0f, 0.0f));
                
                float4 _Col = (c1 + c2 + c3 + c4 + c5 + d2 + d3 + d4 + d5) / 9.0f;
	



	//half3 magic = float3(0.06711056, 0.00583715, 52.9829189);
 //   half gradient = frac(magic.z * frac(dot(i.uv / _MainTex_TexelSize, magic.xy))) / 255.0;
 //   _Col.rgb -= gradient.xxx;




	float4 output = _Col;
	output.a = original.a;
	return output;
}
ENDCG

	}
}

Fallback off

}
