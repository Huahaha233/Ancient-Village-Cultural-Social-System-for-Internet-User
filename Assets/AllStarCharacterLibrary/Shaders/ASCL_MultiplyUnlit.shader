
Shader "ASCL/MultiplyUnlit" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Diffuse Map", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 400
		Lighting Off
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		
		struct Input 
		{
			float2 uv_MainTex;
		};

		float4 _Color;
		sampler2D _MainTex;

	    void surf (Input IN, inout SurfaceOutput o) 
	    {
			half4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Emission.rgb = tex.rgb* _Color;
		}
		ENDCG  
	}
	FallBack "VertexLit"
}



