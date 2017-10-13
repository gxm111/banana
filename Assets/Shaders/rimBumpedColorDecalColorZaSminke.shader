Shader "Rim Bumped Color Decal Color Za Sminke" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}      
	_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
    _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	_DecalTex ("Decal (RGBA)", 2D) = "black" {}
	_DecalColor ("Decal Color", Color) = (1,1,1,1)
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 300
	Cull Off
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
float4 _RimColor;
float4 _DecalColor;
float _RimPower;
sampler2D _DecalTex;
struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
    float3 viewDir;
    	float2 uv_DecalTex;
    	//float4 decalColor;
    };

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	half4 decal = tex2D(_DecalTex, IN.uv_DecalTex) * _DecalColor;
	//IN.decalColor = (1,0,0,1);//_DecalColor;
	c.rgb = lerp (c.rgb, decal.rgb, decal.a);
	//c *= _Color;
	o.Albedo = c.rgb;
	//o.Albedo*= IN.decalColor;
	o.Gloss = 0.8;
	o.Alpha = c.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));  
	half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
    o.Emission = _RimColor.rgb * pow (rim, _RimPower);
}
ENDCG  
}

FallBack "Diffuse"
}
