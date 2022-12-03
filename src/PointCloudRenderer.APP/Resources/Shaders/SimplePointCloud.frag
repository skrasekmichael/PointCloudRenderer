#version 330 core

in vec4 gColor;
in vec2 gCoords;
out vec4 fragColor;

void main()
{
	float d = distance(gCoords, vec2(0.5, 0.5));
	if (d < 0.5)
	{
		fragColor = gColor;
	}
	else
	{
		discard;
	}
}
