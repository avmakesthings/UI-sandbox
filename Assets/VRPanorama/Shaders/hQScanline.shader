// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "hQScanline"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Main("Main", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 screenPos;
		};

		uniform sampler2D _Main;
		uniform float hQStep;
		uniform float _Float0;
		uniform float hQSteps;
		uniform float sbs;
		uniform float scanlineSoft;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos4 = ase_screenPos;
			ase_screenPos4.xyzw /= ase_screenPos4.w;
			float2 appendResult24 = (float2(ase_screenPos4.x , ase_screenPos4.y));
			o.Emission = tex2D( _Main, appendResult24 ).rgb;
			float temp_output_25_0 = ( 1.0 / hQSteps );
			float temp_output_36_0 = ( sbs * 4.0 );
			o.Alpha = ( 1.0 - smoothstep( temp_output_25_0 , ( temp_output_25_0 + ( scanlineSoft / hQSteps ) ) , ( abs( ( frac( ( ( ase_screenPos4.x + ( ( ( ( ( ( hQStep + _Float0 ) * 10.0 ) * temp_output_25_0 ) - 5.0 ) * 0.1 ) / temp_output_36_0 ) ) * temp_output_36_0 ) ) - 0.5 ) ) * 2.0 ) ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=12001
218;387;1470;595;2024.382;672.299;1.9;True;True
Node;AmplifyShaderEditor.RangedFloatNode;33;-2215.091,-636.7968;Float;False;Property;_Float0;Float 0;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;15;-2203.403,-973.3986;Float;False;Global;hQStep;hQStep;0;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1910.591,-704.7969;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-1036.401,-629.1;Float;False;Global;hQSteps;hQSteps;1;0;9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1747.191,-621.0995;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;10.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;25;-636.5974,-424.8995;Float;False;2;0;FLOAT;1.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1517.99,-552.2992;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;-1381.901,-417.4001;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-1135.789,275.4024;Float;False;Global;sbs;sbs;3;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;35;-1100.388,389.2015;Float;False;Constant;_Float2;Float 2;3;0;4;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1304.001,-263.5001;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.1;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-840.3882,270.9015;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;4.0;False;1;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;4;-1545.2,95.89973;Float;False;0;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-1112.301,-240.7001;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;4.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1190.802,93.79994;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.1;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-840.0002,111.1994;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;4.0;False;1;FLOAT
Node;AmplifyShaderEditor.FractNode;6;-636.3995,76.69967;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;21;-777.5975,-227.7992;Float;False;Global;scanlineSoft;scanlineSoft;1;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;10;-493.2992,76.09973;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;30;-564.4885,-92.69714;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;11;-329.2004,79.89971;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-166.8006,79.89969;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;2.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-352.6009,-124.8002;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.03;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;24;-438.5969,-784.4994;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SmoothstepOpNode;8;15.89986,-99.50012;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;23;-21.9962,-604.0991;Float;True;Property;_Main;Main;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2012.687,-886.6981;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;26;-832.894,-741.6002;Float;False;2;0;FLOAT;1.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;19;257.3005,-170.4003;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;722.6997,-341.3001;Float;False;True;2;Float;ASEMaterialInspector;0;Unlit;hQScanline;False;False;False;False;True;True;True;True;True;True;False;False;False;False;True;True;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;15;0
WireConnection;32;1;33;0
WireConnection;28;0;32;0
WireConnection;25;1;9;0
WireConnection;27;0;28;0
WireConnection;27;1;25;0
WireConnection;18;0;27;0
WireConnection;17;0;18;0
WireConnection;36;0;34;0
WireConnection;36;1;35;0
WireConnection;16;0;17;0
WireConnection;16;1;36;0
WireConnection;14;0;4;1
WireConnection;14;1;16;0
WireConnection;5;0;14;0
WireConnection;5;1;36;0
WireConnection;6;0;5;0
WireConnection;10;0;6;0
WireConnection;30;0;21;0
WireConnection;30;1;9;0
WireConnection;11;0;10;0
WireConnection;12;0;11;0
WireConnection;13;0;25;0
WireConnection;13;1;30;0
WireConnection;24;0;4;1
WireConnection;24;1;4;2
WireConnection;8;0;12;0
WireConnection;8;1;25;0
WireConnection;8;2;13;0
WireConnection;23;1;24;0
WireConnection;37;0;15;0
WireConnection;37;1;34;0
WireConnection;26;1;9;0
WireConnection;19;0;8;0
WireConnection;0;2;23;0
WireConnection;0;9;19;0
ASEEND*/
//CHKSM=552728C0898706F00F30F2A9AFFFD14E428562EA