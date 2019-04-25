Shader "Unlit/Sketch"
{
	Properties
	{
		_Color("Color",Color) = (1,1,1,1)
		//get textures for the mat
		_TileFactor("TileFactor", Range(0, 10)) = 1
		_Hatch0("Hatch0",2D) = "white"{}
		_Hatch1("Hatch1",2D) = "white"{}
		_Hatch2("Hatch2",2D) = "white"{}
		_Hatch3("Hatch3",2D) = "white"{}
		_Hatch4("Hatch4",2D) = "white"{}
		_Hatch5("Hatch5",2D) = "white"{}
		//outline drawing constant
		_OutlineFactor("OutlineFactor",Range(0.0,0.1)) = 0.01
	}
		SubShader
		{
			Tags{ "Queue" = "Transparent" }

			//DRAW OUTLINE
			Pass
			{
			//only render the back side
			Cull Front
			//no depth
			ZWrite Off
			//set outline drawing pass a little far from the camera
			Offset 1,1

			//include the vertex and fragment shader
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			float _OutlineFactor;

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				//change normal
				float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(vnormal.xy);

				//add offset
				o.pos.xy += offset * _OutlineFactor;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(0,0,0,1);
			}
			ENDCG
		}


		//DRAW THE OBJECT
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase


			//to use sample textures
			float4 _Color;
			float _TileFactor;
			sampler2D _Hatch0;
			sampler2D _Hatch1;
			sampler2D _Hatch2;
			sampler2D _Hatch3;
			sampler2D _Hatch4;
			sampler2D _Hatch5;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 hatchWeights0:TEXCOORD1;
				float3 hatchWeights1:TEXCOORD2;
				SHADOW_COORDS(4)
				float3 worldPos:TEXCOORD3;
			};

			//vertext shader
			v2f vert(appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				//the txture grows when the uv parameter get bigger
				o.uv = v.texcoord* _TileFactor;
				float3 worldLightDir = normalize(WorldSpaceLightDir(v.vertex));
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);

				//diffuse
				float diffuse = max(0, dot(worldLightDir, worldNormal));
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				//the weight of the textures
				o.hatchWeights0 = float3(0, 0, 0);
				o.hatchWeights1 = float3(0, 0, 0);

				//render the texture according to the diffuse
				float hatchFactor = diffuse * 7.0;
				if (hatchFactor > 6.0) {
				}
				else if (hatchFactor > 5.0) {
					o.hatchWeights0.x = hatchFactor - 5.0;
				}
				else if (hatchFactor > 4.0) {
					o.hatchWeights0.x = hatchFactor - 4.0;
					o.hatchWeights0.y = 1.0 - o.hatchWeights0.x;
				}
				else if (hatchFactor > 3.0) {
					o.hatchWeights0.y = hatchFactor - 3.0;
					o.hatchWeights0.z = 1.0 - o.hatchWeights0.y;
				}
				else if (hatchFactor > 2.0) {
					o.hatchWeights0.z = hatchFactor - 2.0;
					o.hatchWeights1.x = 1.0 - o.hatchWeights0.z;
				}
				else if (hatchFactor > 1.0) {
					o.hatchWeights1.x = hatchFactor - 1.0;
					o.hatchWeights1.y = 1.0 - o.hatchWeights1.x;
				}
				else {
					o.hatchWeights1.y = hatchFactor;
					o.hatchWeights1.z = 1.0 - o.hatchWeights1.y;
				}
				
				//transfer the shader to fragment
				TRANSFER_SHADOW(o);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 hatchTex0 = tex2D(_Hatch0, i.uv) * i.hatchWeights0.x;
				float4 hatchTex1 = tex2D(_Hatch1, i.uv) * i.hatchWeights0.y;
				float4 hatchTex2 = tex2D(_Hatch2, i.uv) * i.hatchWeights0.z;
				float4 hatchTex3 = tex2D(_Hatch3, i.uv) * i.hatchWeights1.x;
				float4 hatchTex4 = tex2D(_Hatch4, i.uv) * i.hatchWeights1.y;
				float4 hatchTex5 = tex2D(_Hatch5, i.uv) * i.hatchWeights1.z;

				//calculate the white part according to diffuse
				float4 whiteColor = float4(1, 1, 1, 1)*(1 - i.hatchWeights0.x - i.hatchWeights0.y - i.hatchWeights0.z - i.hatchWeights1.x - i.hatchWeights1.y - i.hatchWeights1.z);
				float4 hatchColor = hatchTex0 + hatchTex1 + hatchTex2 + hatchTex3 + hatchTex4 + hatchTex5 + whiteColor;
				
				//shadow
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
				return float4(hatchColor.rgb*_Color.rgb*atten, 1.0);
			}
			ENDCG
		}
		}
}