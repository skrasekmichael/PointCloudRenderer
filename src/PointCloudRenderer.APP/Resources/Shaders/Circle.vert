#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aCoords;

uniform mat4 mvp = mat4(1.f);

out vec2 vCoords;
flat out vec3 center;

void main()
{
	gl_Position = vec4(aPosition, 1.f) * mvp;
	vCoords = aCoords;
}
