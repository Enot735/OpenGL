#version 330 core

in vec3 aPosition;
in vec3 aColor;

uniform float scaleFactor;
uniform mat4 rotation;

out vec4 vertexColor;

void main()
{
	gl_Position = rotation * vec4(aPosition * scaleFactor, 1.0);
	vertexColor = vec4(aColor, 0.0);
}
