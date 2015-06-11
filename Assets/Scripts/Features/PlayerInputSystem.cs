using Entitas;
using UnityEngine;

public class PlayerInputSystem : IExecuteSystem, ISetPool {
	Entity _player;

	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
	}
	
	public void Execute()
	{	
		Vector2 speed = Vector2.zero;
		if(Input.GetKey(KeyCode.UpArrow))
		{
			speed.y = 1;
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			speed.y = -1;
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			speed.x = -1;
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			speed.x = 1;
		}
		_player.ReplacePlayerMove(speed.x,speed.y);
	}
}