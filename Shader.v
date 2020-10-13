#version 330 core

in vec3 aPosition;
in vec3 aColor;

uniform float scaleFactor;
uniform mat4 rotation;
uniform mat4 perspective;

out vec4 vertexColor;

void main()
{
	gl_Position = perspective * rotation * vec4(aPosition * scaleFactor, 1.0) + vec4(0.0f, -0.2f, 1.0f, 1.0f);
	vertexColor = vec4(aColor, 1.0);
}
