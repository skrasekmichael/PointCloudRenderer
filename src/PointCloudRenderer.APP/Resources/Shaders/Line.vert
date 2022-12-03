#version 330 core

layout (location = 0) in vec3 aPosition;

uniform mat4 mvp;

void main()
{
	gl_Position = vec4(aPosition, 1.0) * mvp;
}
