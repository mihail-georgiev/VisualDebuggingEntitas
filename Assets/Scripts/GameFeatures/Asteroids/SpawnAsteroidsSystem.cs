using Entitas;
using UnityEngine;

public class SpawnAsteroidsSystem : IExecuteSystem, ISetPool {
	Pool _pool;
	
	public void SetPool(Pool pool) {
		_pool = pool;
	}
	
	public void Execute() {
		int inGameAsteroids = _pool.GetGroup(Matcher.AllOf(Matcher.Asteroid)).Count;
		if(inGameAsteroids<8) {	
			Entity e = _pool.CreateEntity();
			float posX = 60f;
			float posY = Random.Range(-40f, 40f);
			e.isAsteroid = true;
			e.AddPosition(posX,posY);
			GameObject newAsteroid = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Asteroid"));
			e.AddGameObject(newAsteroid);
			e.AddAsteroidSpeed(Random.Range(0f,1f));
		}
	}
}