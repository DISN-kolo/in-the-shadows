shader_type canvas_item;

uniform float intensity = 0.f;

void fragment() {
	COLOR = vec4(
		COLOR.r * (1. + intensity),
		COLOR.g * (1. + intensity),
		COLOR.b * (1. + intensity),
		COLOR.a
		);
}
