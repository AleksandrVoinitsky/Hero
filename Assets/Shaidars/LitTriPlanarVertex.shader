Shader "Toon/Lit TriPlanar Vertex" {
    Properties{
        [Header(Main)]
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
        _Normal("Noise", 2D) = "gray" {}
        _NoiseScale("Noise Scale", Range(-2,2)) = 1

        [Space]
        [Header(Extra Textures)]
        _MainTexBase("Base Texture", 2D) = "white" {}
        _MainTex("Primary Texture", 2D) = "white" {}
        _MainTex2("Secondary Texture", 2D) = "white" {}
        _Scale("Base Scale", Range(-2,2)) = 1
        _PrimaryScale("Primary Scale", Range(-2,2)) = 1
        _SecondaryScale("Secondary Scale", Range(-2,2)) = 1
        _EdgeColor("Primary Edge Color", Color) = (0.5,0.5,0.5,1)
        _EdgeColor2("Secondary Edge Color", Color) = (0.5,0.5,0.5,1)
        _Edgewidth("Edge Width", Range(0,0.2)) = 0.1

        [Space]
        [Header(Rim)]
        _RimPower("Rim Power", Range(-2,20)) = 1
        _RimColor("Rim Color Top", Color) = (0.5,0.5,0.5,1)
        _RimColor2("Rim Color Side/Bottom", Color) = (0.5,0.5,0.5,1)
    }

        SubShader{
        Tags{ "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
#pragma surface surf ToonRamp

        sampler2D _Ramp;
        uniform float4 _ShadowColor;
        uniform float _ShadowStrength;

        // custom lighting function that uses a texture ramp based
        // on angle between light direction and normal
    #pragma lighting ToonRamp exclude_path:prepass
        inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
        {
    #ifndef USING_DIRECTIONAL_LIGHT
            lightDir = normalize(lightDir);
    #endif

            half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
            half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
            c.rgb = lerp(c.rgb, (_ShadowColor - _ShadowStrength), 1 - (atten));
            c.a = 0;
            return c;
        }


        sampler2D _MainTex, _MainTexBase, _Normal, _MainTex2;
        float4 _Color, _RimColor, _RimColor2;
        float _RimPower;
        float _Scale, _PrimaryScale, _SecondaryScale, _NoiseScale;
        float4 _EdgeColor, _EdgeColor2;
        float _Edgewidth;

        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float3 worldPos; // world position built-in value
            float3 worldNormal; // world normal built-in value
            float3 viewDir;// view direction built-in value we're using for rimlight
            float4 vertexColor : COLOR;
        };

        void surf(Input IN, inout SurfaceOutput o) {

            // clamp (saturate) and increase(pow) the worldnormal value to use as a blend between the projected textures
            float3 blendNormal = saturate(pow(IN.worldNormal * 1.4,4));

            // normal noise triplanar for x, y, z sides
            float3 xn = tex2D(_Normal, IN.worldPos.zy * _NoiseScale);
            float3 yn = tex2D(_Normal, IN.worldPos.zx * _NoiseScale);
            float3 zn = tex2D(_Normal, IN.worldPos.xy * _NoiseScale);

            // lerped together all sides for noise texture
            float3 noisetexture = zn;
            noisetexture = lerp(noisetexture, xn, blendNormal.x);
            noisetexture = lerp(noisetexture, yn, blendNormal.y);

            // triplanar for primary texture for x, y, z sides
            float3 xm = tex2D(_MainTex, IN.worldPos.zy * _PrimaryScale);
            float3 zm = tex2D(_MainTex, IN.worldPos.xy * _PrimaryScale);
            float3 ym = tex2D(_MainTex, IN.worldPos.zx * _PrimaryScale);

            // lerped together all sides for primary texture
            float3 toptexture = zm;
            toptexture = lerp(toptexture, xm, blendNormal.x);
            toptexture = lerp(toptexture, ym, blendNormal.y);

            // triplanar for secondary texture for x, y, z sides
           float3 xm2 = tex2D(_MainTex2, IN.worldPos.zy * _SecondaryScale);
           float3 zm2 = tex2D(_MainTex2, IN.worldPos.xy * _SecondaryScale);
           float3 ym2 = tex2D(_MainTex2, IN.worldPos.zx * _SecondaryScale);

           // lerped together all sides for secondary texture
           float3 toptexture2 = zm2;
           toptexture2 = lerp(toptexture2, xm2, blendNormal.x);
           toptexture2 = lerp(toptexture2, ym2, blendNormal.y);

           // triplanar for base texture, x,y,z sides
           float3 x = tex2D(_MainTexBase, IN.worldPos.zy * _Scale);
           float3 y = tex2D(_MainTexBase, IN.worldPos.zx * _Scale);
           float3 z = tex2D(_MainTexBase, IN.worldPos.xy * _Scale);

           // lerped together all sides for base texture
           float3 baseTexture = z;
           baseTexture = lerp(baseTexture, x, blendNormal.x);
           baseTexture = lerp(baseTexture, y, blendNormal.y);

           // rim light for fuzzy top texture
           half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal * noisetexture));

           // rim light for side/bottom texture
           half rim2 = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal * noisetexture));

           // primary texture only on red vertex color with the noise texture
           float vertexColoredPrimary = step(0.6 * noisetexture,IN.vertexColor.r);
           float3 primaryTextureResult = vertexColoredPrimary * toptexture;
           // secondary texture only on blue vertex color with the noise texture
           float vertexColoredSecondary = step(0.6 * noisetexture + saturate(IN.vertexColor.r),IN.vertexColor.b);
           float3 secondaryTextureResult = vertexColoredSecondary * toptexture2;
           // edge for primary texture
           float vertexColorEdge = (step((0.6 - _Edgewidth) * noisetexture,IN.vertexColor.r)) * (1 - vertexColoredPrimary);
           // edge for secondary texture
           float vertexColorEdge2 = (step((0.6 - _Edgewidth) * noisetexture + saturate(IN.vertexColor.r),IN.vertexColor.b)) * (1 - vertexColoredSecondary);

           // basetexture only where there is no red or blue vertex paint
           float3 sideTextureResult = baseTexture * (1 - (vertexColoredPrimary + vertexColorEdge + vertexColoredSecondary + vertexColorEdge2));

           // final albedo color by adding everything together
           o.Albedo = sideTextureResult + primaryTextureResult + (vertexColorEdge * _EdgeColor) + secondaryTextureResult + (vertexColorEdge2 * _EdgeColor2); //+ topTextureEdgeResult;
           o.Albedo *= _Color;

           // adding the fuzzy rimlight(rim) on the top texture, and the harder rimlight (rim2) on the side/bottom texture
           o.Emission = (vertexColoredSecondary * pow(rim2, _RimPower) * _RimColor2) + (vertexColoredPrimary * _RimColor * pow(rim, _RimPower));


       }
       ENDCG

        }

            Fallback "Diffuse"
}
