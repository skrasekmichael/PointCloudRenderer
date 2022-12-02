#version 330 core

uniform vec4 color;

in vec2 vCoords;
flat in vec3 center;

out vec4 fragColor;

void main()
{
	fragColor = color;
	float d = distance(vCoords, vec2(0.5, 0.5));
	if (d < 0.49 || d > 0.5)
	{
		discard;
	}
}
