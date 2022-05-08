// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Distant Lands/Vista/Vista/Stylized Background"
{
	Properties
	{
		_Depth("Depth", Float) = 1
		_DepthOffset("Depth Offset", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		uniform float _DepthOffset;
		uniform float _Depth;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 transform3 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float clampResult11 = clamp( ( ( abs( transform3.z ) - _DepthOffset ) / _Depth ) , 0.0 , 1.0 );
			float4 lerpResult6 = lerp( unity_AmbientGround , unity_AmbientSky , clampResult11);
			o.Albedo = lerpResult6.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18900
225;156.5;1036;568;1300.605;54.38074;1.586471;True;False
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;3;-1490.493,370.8946;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;7;-1288.616,421.8957;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1284.392,514.9566;Inherit;False;Property;_DepthOffset;Depth Offset;1;0;Create;True;0;0;0;False;0;False;0;387.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1007.292,570.6848;Inherit;False;Property;_Depth;Depth;0;0;Create;True;0;0;0;False;0;False;1;525;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;10;-1077.926,420.8071;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;4;-828.6183,392.7394;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;11;-695.4555,333.6144;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;15;-755.9081,145.5949;Inherit;False;unity_AmbientSky;0;1;COLOR;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;16;-754.1293,18.34894;Inherit;False;unity_AmbientGround;0;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;6;-367.7422,123.6507;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;148.4404,154.4209;Float;False;True;-1;2;;0;0;Standard;Distant Lands/Vista/Vista/Stylized Background;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;3;3
WireConnection;10;0;7;0
WireConnection;10;1;9;0
WireConnection;4;0;10;0
WireConnection;4;1;5;0
WireConnection;11;0;4;0
WireConnection;6;0;16;0
WireConnection;6;1;15;0
WireConnection;6;2;11;0
WireConnection;0;0;6;0
ASEEND*/
//CHKSM=6630E55E8B6B39436E08206B4467FB0573C18883