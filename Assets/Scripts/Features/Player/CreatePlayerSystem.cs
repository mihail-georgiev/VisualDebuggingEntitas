using Entitas;
using UnityEngine;

public class CreatePlayerSystem : IStartSystem, ISetPool {
	Pool _pool;
	
	public void SetPool(Pool pool) {
		_pool = pool;
	}
	
	public void Start() {
		var e = _pool.CreateEntity();
		e.isPlayer = true;
		GameObject player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
		e.AddGameObject(player);
		e.AddPlayerMove(0f,0f);
		e.AddPosition(-55f,0f);
	}
}