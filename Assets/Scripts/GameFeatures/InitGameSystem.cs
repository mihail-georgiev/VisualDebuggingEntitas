using Entitas;
using UnityEngine;

public class InitGameSystem : IStartSystem, ISetPool {
	Pool _pool;
	
	public void SetPool(Pool pool) {
		_pool = pool;
	}
	
	public void Start() {
		var playerEntity = _pool.CreateEntity();
		playerEntity.isPlayer = true;
		GameObject player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
		playerEntity.AddGameObject(player);
		playerEntity.AddPlayerSpeed(0f,0f);
		playerEntity.AddPosition(-55f,0f);

		var scoreEntity = _pool.CreateEntity();
		scoreEntity.AddScore(0);
	}
}