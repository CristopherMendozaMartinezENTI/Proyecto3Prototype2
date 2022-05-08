// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Distant Lands/Stylized Sky Shader"
{
	Properties
	{
		[HDR]_ZenithColor("Zenith Color", Color) = (0.3063813,0.6443705,0.764151,1)
		[HDR]_HorizonColor("Horizon Color", Color) = (0.8111427,1.574054,1.849057,1)
		_GradientMultiplier("Gradient Multiplier", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows nofog 
		struct Input
		{
			float4 vertexColor : COLOR;
		};

		uniform float4 _HorizonColor;
		uniform float4 _ZenithColor;
		uniform float _GradientMultiplier;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 clampResult7 = clamp( ( _GradientMultiplier * i.vertexColor ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult4 = lerp( _HorizonColor , _ZenithColor , clampResult7);
			o.Emission = lerpResult4.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
225;156.5;1036;568;1448.302;161.4752;1.6;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-796.2379,326.1084;Inherit;False;Property;_GradientMultiplier;Gradient Multiplier;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1;-789.9054,435.794;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-580.3506,379.062;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;3;-580.3499,-71.72144;Inherit;False;Property;_HorizonColor;Horizon Color;1;1;[HDR];Create;True;0;0;0;False;0;False;0.8111427,1.574054,1.849057,1;0.8111427,1.574054,1.849057,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-547.7633,127.8724;Inherit;False;Property;_ZenithColor;Zenith Color;0;1;[HDR];Create;True;0;0;0;False;0;False;0.3063813,0.6443705,0.764151,1;0.3063813,0.6443705,0.764151,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;7;-443.2148,378.3831;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;4;-268.0599,122.4413;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;9;-31.22898,111.3382;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Distant Lands/Stylized Sky Shader;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;5;1;1;0
WireConnection;7;0;5;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;4;2;7;0
WireConnection;9;2;4;0
ASEEND*/
//CHKSM=08CA2C3EB2CF5F043EE2549FCC182A7446562070