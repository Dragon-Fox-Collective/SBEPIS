#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorTexture;
uniform sampler2D depthTexture;
uniform float nearClipPlane;
uniform float farClipPlane;

const float offset = 1.0 / 300.0;
const vec2 offsets3x3[9] = vec2[](
    vec2(-offset,  offset), // top-left
    vec2( 0.0f,        offset), // top-center
    vec2( offset,  offset), // top-right
    vec2(-offset,  0.0f),       // center-left
    vec2( 0.0f,        0.0f),       // center-center
    vec2( offset,  0.0f),       // center-right
    vec2(-offset, -offset), // bottom-left
    vec2( 0.0f,       -offset), // bottom-center
    vec2( offset, -offset)  // bottom-right    
);
const vec2 offsets5x5[25] = vec2[](
    vec2(-offset*2,  offset*2), // top-left
    vec2(-offset,  offset*2), // top-left
    vec2( 0.0f,        offset*2), // top-center
    vec2( offset,  offset*2), // top-right
    vec2( offset*2,  offset*2), // top-right
    vec2(-offset*2,  offset), // top-left
    vec2(-offset,  offset), // top-left
    vec2( 0.0f,        offset), // top-center
    vec2( offset,  offset), // top-right
    vec2( offset*2,  offset), // top-right
    vec2(-offset*2,  0.0f),       // center-left
    vec2(-offset,  0.0f),       // center-left
    vec2( 0.0f,        0.0f),       // center-center
    vec2( offset,  0.0f),       // center-right
    vec2( offset*2,  0.0f),       // center-right
    vec2(-offset*2, -offset), // bottom-left
    vec2(-offset, -offset), // bottom-left
    vec2( 0.0f,       -offset), // bottom-center
    vec2( offset, -offset), // bottom-right
    vec2( offset*2, -offset), // bottom-right
    vec2(-offset*2, -offset*2), // bottom-left
    vec2(-offset, -offset*2), // bottom-left
    vec2( 0.0f,       -offset*2), // bottom-center
    vec2( offset, -offset*2), // bottom-right
    vec2( offset*2, -offset*2)  // bottom-right
);

const float PI = 3.14159265358979323846;

float depth(vec2 coord)
{
    float depth = texture(depthTexture, coord).r;
    float z = depth * 2.0 - 1.0; // Back to NDC 
    float linearDepth = (2.0 * nearClipPlane * farClipPlane) / (farClipPlane + nearClipPlane - z * (farClipPlane - nearClipPlane));
    return linearDepth >= farClipPlane - 1 ? farClipPlane : linearDepth;
}

float gaussian(float sigma, float pos)
{
    return exp(-(pos * pos) / (2.0 * sigma * sigma)) / (sigma * sqrt(2.0 * PI));
}

float luminance(vec3 color)
{
    return dot(color, vec3(0.299, 0.587, 0.114));
}

vec3 gaussianBlurColor(float sigma)
{
    vec3 result = vec3(0.0);
    float total = 0.0;
    for (int i = 0; i < 9; i++)
    {
        vec2 offset = offsets3x3[i];
        float weight = gaussian(sigma, length(offset));
        vec3 samp = texture(colorTexture, TexCoords + offset).rgb;
        result += samp * weight;
        total += weight;
    }
    return result / total;
}

float gaussianBlurDepth(float sigma)
{
    float result = 0.0;
    float total = 0.0;
    for (int i = 0; i < 9; i++)
    {
        vec2 offset = offsets3x3[i];
        float weight = gaussian(sigma, length(offset * 300.0));
        float samp = depth(TexCoords + offset);
        result += samp * weight;
        total += weight;
    }
    return result / total;
}

void main()
{
    vec3 color = texture(colorTexture, TexCoords).rgb;
    
    float gamma = 0.5;
    int numColors = 16;
    vec3 posterColor = color;
    posterColor = pow(posterColor, vec3(gamma, gamma, gamma));
    posterColor = posterColor * numColors;
    posterColor = floor(posterColor);
    posterColor = posterColor / numColors;
    posterColor = pow(posterColor, vec3(1.0/gamma));
    
    float edge = gaussianBlurDepth(0.7) - gaussianBlurDepth(0.3) > 0.1 ? 1.0 : 0.0;
    //color = mix(color, vec3(0.0), edge);
    
    FragColor = vec4(color, 1.0);
}