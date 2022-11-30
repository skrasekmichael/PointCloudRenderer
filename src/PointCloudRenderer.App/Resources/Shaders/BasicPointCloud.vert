#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;

uniform mat4 model = mat4(1.f);
uniform mat4 view = mat4(1.f);
uniform mat4 projection = mat4(1.f);
uniform float zoom = 1;

out vec4 vColor;

void main()
{
	gl_Position = vec4(aPosition, 1.f) * model * view * projection;
	vColor = aColor;
}
