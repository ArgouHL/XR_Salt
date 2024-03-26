Shader "Custom/RayMarchingTest"
{
	Properties
	{
		_BaseMap("BaseMap", 2D) = "white" {}
		MAX_STEPS("MAX_STEPS",float) = 100//步进最大次数
		SURF_DIST("SURF_DIST",float) = 0.001//距离容差值
		MAX_DIST("MAX_DIST",float) = 100//步进的最远距离
		ParticalPos("ParticalPos",Vector) = (50,50,50,50)
			ParticalPos2("ParticalPos2",Vector) = (50,50,50,50)
			ParticalPos3("ParticalPos3",Vector) = (50,50,50,50)
	}
		SubShader
		{
			HLSLINCLUDE
			   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			   CBUFFER_START(UnityPerMaterial)
			   float MAX_STEPS;
			   float SURF_DIST;
			   float MAX_DIST;
			   float4 ParticalPos;
			   float4 ParticalPos2;
			   float4 ParticalPos3;

			   CBUFFER_END
			   TEXTURE2D(_BaseMap);                 SAMPLER(sampler_BaseMap);
			   struct Attributes {
					float4 positionOS : POSITION;
					float4 texcoord : TEXCOORD;
			   };
				struct Varyings {
				float4 positionCS : SV_POSITION;
				float3 positionWS : TEXCOORD0;
				float2 uv : TEXCOORD1;
				};
			ENDHLSL
			Pass
			{
				Tags{"LightMode" = "UniversalForward"}
				Cull Off
				HLSLPROGRAM
				#pragma target 3.0
				#pragma vertex Vertex
				#pragma fragment Frag
				Varyings Vertex(Attributes input)
				{
					Varyings output;
					VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
					output.positionCS = vertexInput.positionCS;
					output.uv = input.texcoord.xy;
					output.positionWS = vertexInput.positionWS;
					return output;
				}

				 float4 opUS(float4 d1, float4 d2, float k)
				 {
				 float h = clamp(0.5 + 0.5 * (d2.w - d1.w) / k, 0.0, 1.0);
				 float3 color = lerp(d2.rgb, d1.rgb, h);
				 float dist = lerp(d2.w, d1.w, h) - k * h * (1.0 - h);
				 return float4(color,dist);
				 }
				 
				 float smoothMin(float4 d1, float4 d2, float k)
				 {
					 float h = max(k - abs(d1 - d2),0) / k;
					 return min(d1, d2) - h * h * h * k * 1 / 6.0;
}



				float SdSphere(float3 pos,float radius)
				{
					float a = length(pos) - radius;
					return a;
				}

				float GetDist(float3 pos)
				{
					float sphere1 = SdSphere(pos - ParticalPos.xyz,ParticalPos.w);
					float sphere2 = SdSphere(pos - ParticalPos2.xyz, ParticalPos2.w);
					float sphere3 = SdSphere(pos - ParticalPos3.xyz, ParticalPos3.w);
					float result = smoothMin(sphere1, sphere2,2);
		
					result = smoothMin(result, sphere3,2);
					return result;
				}

				float3 GetNormal(float3 position)
				{
					const float2 offset = float2(0.001,0.0);
					float3 n = float3(
						GetDist(position + offset.xyy) - GetDist(position - offset.xyy),
						GetDist(position + offset.yxy) - GetDist(position - offset.yxy),
						GetDist(position + offset.yyx) - GetDist(position - offset.yyx));
					return normalize(n);
				}


				//光线步进
				float RayMarch(float3 rayOrigion,float3 rayDirection)
				{
					float disFromOrigion = 0;//终点距离射线起点的距离
					float disFromSphere;//距离物体的距离
					for (int i = 0;i < MAX_STEPS;i++)
					{
						float3 position = rayOrigion + rayDirection * disFromOrigion;//射线目前的终点坐标
						disFromSphere = GetDist(position);//该点离物体的距离
						disFromOrigion += disFromSphere;//用计算出来的离物体的距离更新终点
						if (disFromSphere<SURF_DIST || disFromOrigion>MAX_DIST) break;//如果该点离物体的距离非常小或者该点到起点的距离超过了最大值，就不继续前进了
					}
					return disFromOrigion;//返回击中物体的点到起点的距离或者返回一个超过最大距离的值-表明没击中物体
				}
				//计算法线


				float4 Frag(Varyings input) :SV_Target
				{
					float3 rayOrigion = _WorldSpaceCameraPos;
					float3 rayDirection = normalize(input.positionWS - rayOrigion);
					float dis = RayMarch(rayOrigion,rayDirection);
					float4 col = 0;
					if (dis < MAX_DIST)
					{
						float3 position = rayOrigion + rayDirection * dis;
						col.rgb = GetNormal(position);
						
					}
					else
						discard;
					return col;
				}
				ENDHLSL
			}
		}
			Fallback "Universal Render Pipeline/Lit"
}