#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;

uniform mat4 mvp = mat4(1.f);

out vec4 vColor;

void main()
{
	gl_Position = vec4(aPosition, 1.f) * mvp;
	vColor = aColor;
}
