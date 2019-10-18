Shader "Unlit/ARCoreBackground"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    // For GLES3
    SubShader
    {
        Pass
        {
            ZWrite Off
            Cull Off

            GLSLPROGRAM

#pragma only_renderers gles3

#ifdef SHADER_API_GLES3
#extension GL_OES_EGL_image_external_essl3 : require
#endif // SHADER_API_GLES3

            uniform mat4 _UnityDisplayTransform;

#ifdef VERTEX
            varying vec2 textureCoord;

            void main()
            {
#ifdef SHADER_API_GLES3
                float flippedV = 1.0 - gl_MultiTexCoord0.y;
                textureCoord.x = _UnityDisplayTransform[0].x * gl_MultiTexCoord0.x + _UnityDisplayTransform[1].x * flippedV + _UnityDisplayTransform[2].x;
                textureCoord.y = _UnityDisplayTransform[0].y * gl_MultiTexCoord0.x + _UnityDisplayTransform[1].y * flippedV + _UnityDisplayTransform[2].y;
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
#endif // SHADER_API_GLES3
            }
#endif // VERTEX

#ifdef FRAGMENT
            varying vec2 textureCoord;
            uniform samplerExternalOES _MainTex;

#if defined(SHADER_API_GLES3) && !defined(UNITY_COLORSPACE_GAMMA)
            float GammaToLinearSpaceExact (float value)
            {
                if (value <= 0.04045F)
                    return value / 12.92F;
                else if (value < 1.0F)
                    return pow((value + 0.055F)/1.055F, 2.4F);
                else
                    return pow(value, 2.2F);
            }

            vec3 GammaToLinearSpace (vec3 sRGB)
            {
                // Approximate version from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
                return sRGB * (sRGB * (sRGB * 0.305306011F + 0.682171111F) + 0.012522878F);

                // Precise version, useful for debugging, but the pow() function is too slow.
                // return vec3(GammaToLinearSpaceExact(sRGB.r), GammaToLinearSpaceExact(sRGB.g), GammaToLinearSpaceExact(sRGB.b));
            }
#endif // SHADER_API_GLES3 && !UNITY_COLORSPACE_GAMMA

            void main()
            {
#ifdef SHADER_API_GLES3
                vec3 result = texture(_MainTex, textureCoord).xyz;

#ifndef UNITY_COLORSPACE_GAMMA
                result = GammaToLinearSpace(result);
#endif // !UNITY_COLORSPACE_GAMMA

                gl_FragColor = vec4(result, 1);
#endif // SHADER_API_GLES3
            }

#endif // FRAGMENT
            ENDGLSL
        }
    }

    FallBack Off
}
