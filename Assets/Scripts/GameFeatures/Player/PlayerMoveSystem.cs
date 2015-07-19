using Entitas;
using UnityEngine;

public class PlayerMoveSystem : IReactiveSystem, ISetPool {
	Entity _player;

	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
	}

	public IMatcher GetTriggeringMatcher() {
		return Matcher.PlayerSpeed;
	}
	
	public GroupEventType GetEventType() {
		return GroupEventType.OnEntityAdded;
	}
	
	public void Execute(Entity[] entities) {
		var speed = entities.SingleEntity().playerSpeed;
		var pos = getNewClampedPlayerPos(_player.position.x, _player.position.y, speed.speedX, speed.speedY);
		_player.ReplacePosition(pos.x, pos.y);
	}

	Vector2 getNewClampedPlayerPos(float posX, float posY, float speedX, float speedY)	{
		posX = posX + speedX;
		posX = Mathf.Min(posX, 60f);
		posX= Mathf.Max(posX, -60f);
		posY = posY + speedY;
		posY = Mathf.Min(posY, 43f);
		posY = Mathf.Max(posY, -43f);

		return new Vector2(posX, posY);
	}
}