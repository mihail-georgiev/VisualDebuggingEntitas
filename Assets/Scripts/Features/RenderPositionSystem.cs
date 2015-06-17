using Entitas;
using UnityEngine;

public class RenderPositionSystem : IReactiveSystem {
	public IMatcher GetTriggeringMatcher() {
		return Matcher.AllOf(Matcher.GameObject, Matcher.Position);
	}
	
	public GroupEventType GetEventType() {
		return GroupEventType.OnEntityAdded;
	}
	
	public void Execute(Entity[] entities) {
		foreach (var e in entities) {
			var pos = e.position;
			if(e.hasGameObject) {
				e.gameObject.gameObject.transform.position = new Vector3(pos.x, 0, pos.y);
			}
		}
	}
}