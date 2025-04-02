extends ColorRect

enum ConstraintAxis {X, Y}
enum Trigger { NONE, LEFT, RIGHT, TOP, BOTTOM }

# the maximum range the node can move
@export var max_offset: Vector2 = Vector2(100, 100)
# the threshold of movement of center before a trigger is set
@export var trigger_threshold: int = 90
@export var constrain_axis_direction: ConstraintAxis = ConstraintAxis.X

# set to (inf, inf) to act as a null/unset state
var node_start_position: Vector2 = Vector2.INF
var trigger_state: Trigger = Trigger.NONE

# assert valid export variable on ready
func _ready() -> void:
	assert(trigger_threshold > 0, "Trigger Threshold must be greater than 0.")
	
	if (constrain_axis_direction == ConstraintAxis.X):
		assert(max_offset.x >= trigger_threshold, "Trigger Threshold > Max Offset x. Trigger is unreachable.")
	else:
		assert(max_offset.y >= trigger_threshold, "Trigger Threshold > Max Offset y. Trigger is unreachable.")

func _on_gui_input(event: InputEvent) -> void:
	# check if set to (inf, inf) before setting the node_start_positon
	# ensures we set the node_start_position only the first time we click it
	if (node_start_position == Vector2.INF && event is InputEventMouseButton && event.pressed == true):
		node_start_position = position
	if (event is InputEventMouseButton && event.pressed == false):
		if (trigger_state != Trigger.NONE):
			emit_trigger()
		# TODO: tween back to node_start_position
		position = node_start_position
		set_state(Trigger.NONE)
	if (event is InputEventMouseMotion && event.button_mask == 1):
		drag_node(event)
		
func drag_node(event: InputEvent) -> void:
	var new_position = position + event.relative
	new_position = new_position.clamp(node_start_position - max_offset, node_start_position + max_offset)
#		
	if constrain_axis_direction == ConstraintAxis.X:
		new_position.y = node_start_position.y
	if constrain_axis_direction == ConstraintAxis.Y:
		new_position.x = node_start_position.x
		
	var position_offset = node_start_position - new_position
	
	match trigger_state:
		Trigger.NONE:
			if (position_offset.x > trigger_threshold):
				set_state(Trigger.LEFT)
			elif (position_offset.x < -trigger_threshold):
				set_state(Trigger.RIGHT)
			if (position_offset.y > trigger_threshold):
				set_state(Trigger.TOP)
			elif (position_offset.y < -trigger_threshold):
				set_state(Trigger.BOTTOM)
		Trigger.LEFT, Trigger.RIGHT:
			if (abs(position_offset.x) < trigger_threshold):
				set_state(Trigger.NONE)
		Trigger.TOP, Trigger.BOTTOM:
			if (abs(position_offset.y) < trigger_threshold):
				set_state(Trigger.NONE)
	
	# TODO: elastic stretch movement to drag - log function?
	position = new_position

func set_state(state: Trigger) -> void:
	trigger_state = state
	$Label.text = Trigger.keys()[trigger_state]
	
func emit_trigger() -> void:
	print(str("emit behaviour for: ", Trigger.keys()[trigger_state]))
	# emit signal to be handled by another script
	# draggable_node.emit(Trigger.STATE)
