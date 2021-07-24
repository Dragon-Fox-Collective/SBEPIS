#ifndef PARALLAX_INCLUDED
#define PARALLAX_INCLUDED

float2 GradientNoise_Dir_float(float2 p)
{
    // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
    p = p % 289;
    // need full precision, otherwise half overflows when p > 1
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

void GradientNoise_float(float2 UV, float Scale, out float Out)
{
    float2 p = UV * Scale;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(GradientNoise_Dir_float(ip), fp);
    float d01 = dot(GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
}

void Parallax_float(float Amplitude, float Steps, float2 UVs, float NoiseScale, float3 ViewDir, out float2 Out) {
	Amplitude *= -0.01f;
	float height = 1.0;
	float step = 1.0 / Steps;
	float2 offset = UVs.xy;
	float heightSample;
	GradientNoise_float(offset, NoiseScale, heightSample);
	float2 delta = ViewDir.xy * Amplitude / (ViewDir.z * Steps);

	for (float i = 0.0f; i < Steps; i++) {
		if (heightSample < height) {
			height -= step;
			offset += delta;
			GradientNoise_float(offset, NoiseScale, heightSample);
		}
		else
		{
			break;
		}
	}

	Out = offset;
}

#endif