using Entitas;
using UnityEngine;

public class AsteroidMoveSystem : IExecuteSystem, ISetPool {
	Group _group;
	
	public void SetPool(Pool pool) {
		_group = pool.GetGroup(Matcher.Asteroid);
	}
	
	public void Execute() {
		foreach(Entity e in _group.GetEntities()) {	
			var move = e.asteroidMove;
			var pos = e.position;
			if(pos.x< -60f)
				e.isDestroyAsteroid = true;
			else 
				e.ReplacePosition(pos.x - move.speed, pos.y); 
		}
	}
}