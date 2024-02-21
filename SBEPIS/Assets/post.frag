﻿#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorTexture;
uniform sampler2D depthTexture;
uniform float nearClipPlane;
uniform float farClipPlane;

const float edgeOffset = 1.0 / 300.0;
const vec2 edgeOffsets[9] = vec2[](
    vec2(-edgeOffset,  edgeOffset), // top-left
    vec2( 0.0f,        edgeOffset), // top-center
    vec2( edgeOffset,  edgeOffset), // top-right
    vec2(-edgeOffset,  0.0f),       // center-left
    vec2( 0.0f,        0.0f),       // center-center
    vec2( edgeOffset,  0.0f),       // center-right
    vec2(-edgeOffset, -edgeOffset), // bottom-left
    vec2( 0.0f,       -edgeOffset), // bottom-center
    vec2( edgeOffset, -edgeOffset)  // bottom-right    
);
float edgeKernel[9] = float[](
    -1, -1, -1,
    -1,  8, -1,
    -1, -1, -1
);

float depth(vec2 coord)
{
    float depth = texture(depthTexture, coord).r;
    float z = depth * 2.0 - 1.0; // Back to NDC 
    float linearDepth = (2.0 * nearClipPlane * farClipPlane) / (farClipPlane + nearClipPlane - z * (farClipPlane - nearClipPlane));
    return linearDepth >= farClipPlane - 1 ? farClipPlane : linearDepth;
}

void main()
{
    float edge = 0.0;
    for (int i = 0; i < 9; i++)
    {
        edge += depth(TexCoords + edgeOffsets[i]) * edgeKernel[i];
    }
    
    FragColor = mix(texture(colorTexture, TexCoords), vec4(0, 0, 0, 1), edge > 0.1 ? 1.0 : 0.0);
}