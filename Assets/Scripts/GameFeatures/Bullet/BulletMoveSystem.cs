using Entitas;

public class BulletMoveSystem : IExecuteSystem, ISetPool {
	Group _bullets;

	public void SetPool(Pool pool) {
		_bullets = pool.GetGroup(Matcher.AllOf(Matcher.Bullet));
	}
	
	public void Execute() {	
		foreach (Entity bullet in _bullets.GetEntities ()) {
			bullet.ReplacePosition(bullet.position.x +0.5f, bullet.position.y);
		}
	}
}