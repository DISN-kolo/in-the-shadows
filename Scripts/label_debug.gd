extends Label

@onready var shadow_caster: CharacterBody3D = %ShadowCaster

func _process(delta: float) -> void:
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
