using Entitas;
using UnityEngine;

public class PlayerMoveSystem : IReactiveSystem, ISetPool {
	public IMatcher GetTriggeringMatcher() {
		return Matcher.PlayerMove;
	}
	
	public GroupEventType GetEventType() {
		return GroupEventType.OnEntityAdded;
	}

	Entity _player;
	
	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
	}
	
	public void Execute(Entity[] entities) {
		var speed = entities.SingleEntity().playerMove;
		var pos = _player.position;
		_player.ReplacePosition(pos.x + speed.speedX, pos.y + speed.speedY);
	}
}