using Entitas;
using UnityEngine;

public class PlayerInputSystem : IExecuteSystem, ISetPool {
	Entity _player;
	Pool _pool;

	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
		_pool = pool;
	}
	
	public void Execute() {	
		Vector2 previousSpeed = new Vector2(_player.playerSpeed.speedX, _player.playerSpeed.speedY);
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
			_player.ReplacePlayerSpeed(newSpeed.x,newSpeed.y);

		if (Input.GetKeyDown (KeyCode.Space))
			shoot();
}

	void shoot(){
		var playerPos = _player.position;

		GameObject newBullet = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));
		Entity e = _pool.CreateEntity();
		e.isBullet = true;
		e.AddGameObject (newBullet);
		e.AddPosition (playerPos.x + 15f, playerPos.y);
	}
}