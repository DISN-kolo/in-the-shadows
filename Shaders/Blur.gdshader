// adapted from source: https://godotshaders.com/shader/dual-kawase-up-fast-blur/
// which was, in turn, from https://blog.frost.kiwi/dual-kawase/#dual-kawase-blur

shader_type canvas_item;
uniform sampler2D uTexture;
uniform float opacity_mod: hint_range(0.0, 1.0) = 1.0;
uniform float offset: hint_range(0.0, 10.0);


void fragment() {
	vec2 uv = UV;
	//vec2 halfpixel = SCREEN_PIXEL_SIZE / 2.0;
	vec2 halfpixel = vec2(0.001, 0.001);
	
	vec4 sum = texture(uTexture, uv + vec2(-halfpixel.x * 2.0, 0.0) * offset);
	sum += texture(uTexture, uv + vec2(-halfpixel.x, halfpixel.y) * offset) * 2.0;
	sum += texture(uTexture, uv + vec2(0.0, halfpixel.y * 2.0) * offset);
	sum += texture(uTexture, uv + vec2(halfpixel.x, halfpixel.y) * offset) * 2.0;
	sum += texture(uTexture, uv + vec2(halfpixel.x * 2.0, 0.0) * offset);
	sum += texture(uTexture, uv + vec2(halfpixel.x, -halfpixel.y) * offset) * 2.0;
	sum += texture(uTexture, uv + vec2(0.0, -halfpixel.y * 2.0) * offset);
	sum += texture(uTexture, uv + vec2(-halfpixel.x, -halfpixel.y) * offset) * 2.0;

	COLOR = sum / 12.0;
	COLOR.a = COLOR.a * opacity_mod;
}