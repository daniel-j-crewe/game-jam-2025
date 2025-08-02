shader_type canvas_item;

void fragment() {
	float r = COLOR.rgb.r;
	float b = COLOR.rgb.b;
    COLOR.r = b;
    COLOR.b = r;
}