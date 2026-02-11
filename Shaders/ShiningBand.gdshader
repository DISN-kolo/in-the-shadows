shader_type canvas_item;

uniform float parameter: hint_range(-0.5, 1.5) = 0.0;
uniform float band: hint_range(0.0, 70.0) = 10.0;
uniform float mini_band_width: hint_range(1.0, 10.0) = 8.0;
uniform float max_x = 128.0;
uniform float part_of_max_x: hint_range(0.5, 10.0) = 7.0;
uniform float max_y = 128.0;
uniform vec4 colorization : source_color;

bool condition_calculator(vec2 local_UV, float local_band, float off)
{
	if ((local_UV.x*max_x + local_UV.y*max_y + local_band/2.0 - off > parameter*max_x + parameter*max_y)
		&& (local_UV.y*max_y + local_UV.x*max_x - local_band/2.0 - off < parameter*max_x + parameter*max_y))
	{
		return true;
	}
	else
	{
		return false;
	}
}

vec4 calc_band(vec2 local_UV, vec4 local_COLOR, float local_band, float off)
{
	vec4 shine = colorization;
	if (condition_calculator(local_UV, local_band, off))
	{
		shine = vec4(colorization.rgb, local_COLOR.a);
	}
	else
	{
		shine = vec4(0.0, 0.0, 0.0, local_COLOR.a);
	}
	return shine;
}

void fragment() {
	float mini_band = band/mini_band_width;
	float off_small = -max_x/part_of_max_x;
	vec4 shine = calc_band(UV, COLOR, band, 0.0);
	shine += calc_band(UV, COLOR, mini_band, off_small);
	vec3 flatcolors = COLOR.rgb + shine.rgb;
	if (condition_calculator(UV, band, 0.0) || condition_calculator(UV, mini_band, off_small))
	{
		COLOR = vec4(flatcolors, COLOR.a)*0.5 + vec4(shine.rgb, COLOR.a)*0.5;
	}
	else
	{
		COLOR = vec4(flatcolors, COLOR.a);
	}
}