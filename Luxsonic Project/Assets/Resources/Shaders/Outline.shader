Shader "Custom/Image Effects" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SaturationAmount ("Saturation Amount", Range(0.0, 2.0)) = 1.0
		_BrightnessAmount ("Brightness Amount", Range(0.0, 2.0)) = 1.0
		_ContrastAmount ("Contrast Amount", Range(0.0,2.0)) = 1.0

		//outline
		_Color ("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0.0, 0.03)) = .005
        _MainTex ("Base (RGB)", 2D) = "white" { }
     }
	}
	SubShader 
	{
		Tags { "Queue" = "Transparent" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float _SaturationAmount;
			uniform float _BrightnessAmount;
			uniform float _ContrastAmount;

			//outline
			struct appdata {
     		float4 vertex : POSITION;
     		float3 normal : NORMAL;
 			};
  
		 	struct v2f {
     		float4 pos : POSITION;
     		float4 color : COLOR;
 			};
  
			uniform float _Outline;
 			uniform float4 _OutlineColor;


 			v2f vert(appdata v) {
    		 // just make a copy of incoming vertex data but scaled according to normal direction
     		v2f o;
    		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
  
     		float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
     		float2 offset = TransformViewToProjection(norm.xy);
  
     		o.pos.xy += offset * o.pos.z * _Outline;
     		o.color = _OutlineColor;
     		return o;
 			}

			float3 ContrastSaturationBrightness( float3 color, float brt, float sat, float con)
			{
				//RGB Color Channels
				float AvgLumR = 0.5;
				float AvgLumG = 0.5;
				float AvgLumB = 0.5;
				
				//Luminace Coefficients for brightness of image
				float3 LuminaceCoeff = float3(0.2125,0.7154,0.0721);
				
				//Brigntess calculations
				float3 AvgLumin = float3(AvgLumR,AvgLumG,AvgLumB);
				float3 brtColor = color * brt;
				float intensityf = dot(brtColor, LuminaceCoeff);
				float3 intensity = float3(intensityf, intensityf, intensityf);
				
				//Saturation calculation
				float3 satColor = lerp(intensity, brtColor, sat);
				
				//Contrast calculations
				float3 conColor = lerp(AvgLumin, satColor, con);
				
				return conColor;
				
			}
			
			
			float4 frag (v2f_img i) : COLOR
			{
				float4 renderTex = tex2D(_MainTex, i.uv);
				
				
				renderTex.rgb = ContrastSaturationBrightness(renderTex.rgb, _BrightnessAmount, _SaturationAmount, _ContrastAmount); 
				
				return renderTex;
			
			}
			
			ENDCG
		}

		
	}
}


 ENDCG
  
     SubShader {
         Tags { "Queue" = "Transparent" }
  
         // note that a vertex shader is specified here but its using the one above
         Pass {
             Name "OUTLINE"
             Tags { "LightMode" = "Always" }
             Cull Off
             ZWrite Off
             ZTest Always
             ColorMask RGB // alpha not used
  
             // you can choose what kind of blending mode you want for the outline
             Blend SrcAlpha OneMinusSrcAlpha // Normal
             //Blend One One // Additive
             //Blend One OneMinusDstColor // Soft Additive
             //Blend DstColor Zero // Multiplicative
             //Blend DstColor SrcColor // 2x Multiplicative
  
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             
             half4 frag(v2f i) :COLOR {
                 return i.color;
             }
             ENDCG
         }
  
         CGPROGRAM
           #pragma surface surf Lambert
   
           sampler2D _MainTex;
           fixed4 _Color;
   
           struct Input {
               float2 uv_MainTex;
           };
   
           void surf (Input IN, inout SurfaceOutput o) {
               fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
               o.Albedo = c.rgb;
               o.Alpha = c.a;
           }
         ENDCG
     }
  
     SubShader {
         Tags { "Queue" = "Transparent" }
  
         Pass {
             Name "OUTLINE"
             Tags { "LightMode" = "Always" }
             Cull Front
             ZWrite Off
             ZTest Always
             ColorMask RGB
  
             // you can choose what kind of blending mode you want for the outline
             Blend SrcAlpha OneMinusSrcAlpha // Normal
             //Blend One One // Additive
             //Blend One OneMinusDstColor // Soft Additive
             //Blend DstColor Zero // Multiplicative
             //Blend DstColor SrcColor // 2x Multiplicative
  
             CGPROGRAM
             #pragma vertex vert
             #pragma exclude_renderers gles xbox360 ps3
             ENDCG
             SetTexture [_MainTex] { combine primary }
         }
  
         CGPROGRAM
           #pragma surface surf Lambert
   
           sampler2D _MainTex;
           fixed4 _Color;
   
           struct Input {
               float2 uv_MainTex;
           };
   
           void surf (Input IN, inout SurfaceOutput o) {
               fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
               o.Albedo = c.rgb;
               o.Alpha = c.a;
           }
           ENDCG
     }
  
     Fallback "Diffuse"
 }