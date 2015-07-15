using Entitas;
using UnityEngine;

public class PlayerInputSystem : IExecuteSystem, ISetPool {
	Entity _player;

	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
	}
	
	public void Execute() {	
		Vector2 previousSpeed = new Vector2(_player.playerMove.speedX, _player.playerMove.speedY);
		Vector2 newSpeed = Vector2.zero;
		if(Input.GetKey(KeyCode.UpArrow))
		{
			newSpeed.y = 1;
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			newSpeed.y = -1;
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			newSpeed.x = -1;
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			newSpeed.x = 1;
		}

		if(newSpeed != Vector2.zero || previousSpeed!= newSpeed)
			_player.ReplacePlayerMove(newSpeed.x,newSpeed.y);
	}
}