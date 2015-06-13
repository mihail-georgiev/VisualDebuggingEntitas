﻿using Entitas;
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
		var pos = newClampedPlayerPos(_player.position.x, _player.position.y, speed.speedX, speed.speedY);
		_player.ReplacePosition(pos.x, pos.y);
	}

	Vector2 newClampedPlayerPos(float posX, float posY, float speedX, float speedY)
	{
		posX = posX + speedX;
		posX = Mathf.Min(posX, 60f);
		posX= Mathf.Max(posX, -60f);
		posY = posY + speedY;
		posY = Mathf.Min(posY, 43f);
		posY = Mathf.Max(posY, -43f);

		return new Vector2(posX, posY);
	}
}