using Entitas;

public class AsteroidMoveSystem : IExecuteSystem, ISetPool {
	Group _group;
	
	public void SetPool(Pool pool) {
		_group = pool.GetGroup(Matcher.AllOf(Matcher.Asteroid));
	}
	
	public void Execute() {
		foreach(Entity e in _group.GetEntities()) {	
			var speed = e.asteroidSpeed;
			var pos = e.position;
			if(pos.x< -60f)
				e.isDestroyAsteroid = true;
			else 
				e.ReplacePosition(pos.x - speed.speed, pos.y); 
		}
	}
}