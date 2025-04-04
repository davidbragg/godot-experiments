extends ColorRect

enum ConstraintAxis {X, Y}
enum Trigger { NONE, LEFT, RIGHT, TOP, BOTTOM }
enum Action { IDLE, DRAG, RELEASED }

# the maximum range the node can move
@export var max_offset: Vector2 = Vector2(100, 100)
# the threshold of movement off center before a trigger is set
@export var trigger_threshold: int = 90
@export var constrain_axis_direction: ConstraintAxis = ConstraintAxis.X
@export var release_tween_length: float = 0.3

# set to (inf, inf) to act as a null/unset state
var node_start_position: Vector2
var trigger_state: Trigger = Trigger.NONE
var action_state: Action = Action.IDLE
var tween = Tween.new()
var last_emit: String = "last emit: NONE"

# assert valid export variables on ready
func _ready() -> void:
	assert(trigger_threshold > 0, "Trigger Threshold must be greater than 0.")

	if (constrain_axis_direction == ConstraintAxis.X):
		assert(max_offset.x >= trigger_threshold, "Trigger Threshold > Max Offset x. Trigger is unreachable.")
	else:
		assert(max_offset.y >= trigger_threshold, "Trigger Threshold > Max Offset y. Trigger is unreachable.")

	node_start_position = position
	
	if OS.is_debug_build():
		debug_info()
		$DebugInfo.visible = true

func _on_gui_input(event: InputEvent) -> void:
	# on left click release when Action.DRAG
	if (event is InputEventMouseButton && event.pressed == false && action_state == Action.DRAG):
		emit_trigger()
		set_action(Action.RELEASED)
		reset_position()

	# on left drag while !Action.RELEASED
	if (event is InputEventMouseMotion && event.button_mask == 1 && action_state != Action.RELEASED):
		if action_state != Action.DRAG:
			set_action(Action.DRAG)
		drag_node(event)

# follow the drag event to the max_offset values and track against position triggers
func drag_node(event: InputEvent) -> void:
	var new_position = position + event.relative
	new_position = new_position.clamp(node_start_position - max_offset, node_start_position + max_offset)

	if constrain_axis_direction == ConstraintAxis.X:
		new_position.y = node_start_position.y
	if constrain_axis_direction == ConstraintAxis.Y:
		new_position.x = node_start_position.x

	var position_offset = node_start_position - new_position

	match trigger_state:
		Trigger.NONE:
			if (position_offset.x > trigger_threshold):
				set_trigger(Trigger.LEFT)
			elif (position_offset.x < -trigger_threshold):
				set_trigger(Trigger.RIGHT)
			if (position_offset.y > trigger_threshold):
				set_trigger(Trigger.TOP)
			elif (position_offset.y < -trigger_threshold):
				set_trigger(Trigger.BOTTOM)
		Trigger.LEFT, Trigger.RIGHT:
			if (abs(position_offset.x) < trigger_threshold):
				set_trigger(Trigger.NONE)
		Trigger.TOP, Trigger.BOTTOM:
			if (abs(position_offset.y) < trigger_threshold):
				set_trigger(Trigger.NONE)

	position = new_position

func set_trigger(state: Trigger) -> void:
	trigger_state = state
	debug_info()

func set_action(state: Action) -> void:
	action_state = state
	debug_info()

func emit_trigger() -> void:
	last_emit ="last emit: " + Trigger.keys()[trigger_state]
	debug_info()
	# emit signal to be handled by another script
	# draggable_node.emit(Trigger.STATE)

func reset_position() -> void:
	# it is normal to put an if tween: tween.kill() here to prevent repeatedly creating tweens
	# we should never get here during an active tween due to tracking on action_state
	tween = create_tween()
	# tweens run sequentially
	# return the node back to the node_start_position
	tween.set_trans(Tween.TRANS_CUBIC).set_ease(Tween.EASE_OUT)
	tween.tween_property(self, "position", node_start_position, release_tween_length)
	# on tween complete, call set_action(Action.IDLE)
	tween.tween_callback(set_action.bind(Action.IDLE))

func debug_info() -> void:
	if !OS.is_debug_build():
		return
	var trigger_zone = "trigger zone: " + Trigger.keys()[trigger_state]
	var action_state ="action state: " + Action.keys()[action_state]
	$DebugInfo.text = trigger_zone + "\n" + action_state + "\n" + last_emit
