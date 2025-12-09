extends Label

var shadow_caster : CharacterBody3D;
var sc_loaded : bool = false;

func _process(delta: float) -> void:
	if sc_loaded:
		self.text = "
rotx: %8.2f | %8.2f
roty: %8.2f | %8.2f
rotz: %8.2f | %8.2f" % [
			shadow_caster.rotation.x,
			shadow_caster.rotation.x / PI,
			shadow_caster.rotation.y,
			shadow_caster.rotation.y / PI,
			shadow_caster.rotation.z,
			shadow_caster.rotation.z / PI
		];

func _on_loaded_thing(arg: CharacterBody3D) -> void:
	shadow_caster = arg;
	sc_loaded = true;
