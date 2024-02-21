#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D colorTexture;
uniform sampler2D depthTexture;

void main()
{
    FragColor = texture(colorTexture, TexCoords) + texture(depthTexture, TexCoords);
}