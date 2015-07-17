using Entitas;
using UnityEngine;

public class DestroyBulletSystem : IReactiveSystem, ISetPool{
	Pool _pool;

	public IMatcher GetTriggeringMatcher() {
		return Matcher.AllOf(Matcher.DestroyBullet);
	}
	
	public GroupEventType GetEventType() {
		return GroupEventType.OnEntityAdded;
	}

	public void SetPool(Pool pool) {
		_pool = pool;
	}
	public void Execute(Entity[] entities) {
		foreach (var e in entities) {
			GameObject.Destroy(e.gameObject.gameObject);
			_pool.DestroyEntity(e);
		}
	}
}