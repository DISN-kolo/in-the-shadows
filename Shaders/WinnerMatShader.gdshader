shader_type canvas_item;

uniform float parameter: hint_range(-0.5, 1.5) = 0.0;
uniform float band: hint_range(0.0, 20.0) = 10.0;
uniform float max_x = 128.0;
uniform float max_y = 128.0;

vec4 calc_band(vec2 local_UV, vec4 local_COLOR, float local_band, float off)
{
	vec4 shine = vec4(1.0, 1.0, 1.0, 1.0);
	if ((local_UV.x*max_x + local_UV.y*max_y + local_band/2.0 - off > parameter*max_x + parameter*max_y)
		&& (local_UV.y*max_y + local_UV.x*max_x - local_band/2.0 - off < parameter*max_x + parameter*max_y))
	{
		shine = vec4(1.0, 1.0, 1.0, local_COLOR.a);
	}
	else
	{
		shine = vec4(0.0, 0.0, 0.0, local_COLOR.a);
	}
	return shine;
}

void fragment() {
	vec4 shine = calc_band(UV, COLOR, band, 0.0);
	shine += calc_band(UV, COLOR, band/8.0, -max_x/7.0);
	vec3 flatcolors = COLOR.rgb + shine.rgb;
	COLOR = vec4(flatcolors, COLOR.a);
}