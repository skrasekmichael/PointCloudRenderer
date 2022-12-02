#version 330 core

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

uniform float size = 0.1;
uniform mat4 proj = mat4(1);

in vec4 vColor[];
out vec4 gColor;
out vec2 gCoords;

void main()
{
	vec4 position = gl_in[0].gl_Position;
	
	gColor = vColor[0];
	gCoords = vec2(0, 1);
	gl_Position = position + size * vec4(-0.2, -0.2, 0.0, 0.0) * proj; // 1:bottom-left
    EmitVertex();

	gColor = vColor[0];
	gCoords = vec2(1, 1);
    gl_Position = position + size * vec4(0.2, -0.2, 0.0, 0.0) * proj; // 2:bottom-right
    EmitVertex();

	gColor = vColor[0];
	gCoords = vec2(0, 0);
    gl_Position = position + size * vec4(-0.2, 0.2, 0.0, 0.0) * proj; // 3:top-left
    EmitVertex();

	gColor = vColor[0];
	gCoords = vec2(1, 0);
    gl_Position = position + size * vec4(0.2, 0.2, 0.0, 0.0) * proj; // 4:top-right
    EmitVertex();

    EndPrimitive();
}
