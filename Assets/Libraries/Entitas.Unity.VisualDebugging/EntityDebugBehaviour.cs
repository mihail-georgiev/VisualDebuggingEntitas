using UnityEngine;
using Entitas;

namespace Entitas.Unity.VisualDebugging {
    public class EntityDebugBehaviour : MonoBehaviour {
        public Pool pool { get { return _pool; } }
        public Entity entity { get { return _entity; } }

        Pool _pool;
        Entity _entity;

        public void Init(Pool pool, Entity entity) {
            _pool = pool;
            _entity = entity;
            _entity.OnComponentAdded += onEntityChanged;
            _entity.OnComponentRemoved += onEntityChanged;
			_entity.OnComponentAdded += logOnComponentAdded;
			_entity.OnComponentReplaced += logOnComponentReplaced;
			_entity.OnComponentRemoved += logOnComponentRemoved;
            updateName();
        }

        public void DestroyBehaviour() {
            _entity.OnComponentAdded -= onEntityChanged;
            _entity.OnComponentRemoved -= onEntityChanged;
			_entity.OnComponentAdded -= logOnComponentAdded;
			_entity.OnComponentReplaced -= logOnComponentReplaced;
			_entity.OnComponentRemoved -= logOnComponentRemoved;
            if (this != null) {
                Destroy(gameObject);
            }
        }

        void onEntityChanged(Entity e, int index, IComponent component) {
                updateName();
        }

	void logOnComponentAdded(Entity e, int index, IComponent component) {
		LogWriter.Instance.WriteToLog(component.DebugInfo(e.creationIndex, "added"), System.DateTime.UtcNow);
	}

	void logOnComponentRemoved(Entity e, int index, IComponent component) {
		LogWriter.Instance.WriteToLog(component.DebugInfo(e.creationIndex, "removed"),System.DateTime.UtcNow);
	}

	void logOnComponentReplaced(Entity e, int index, IComponent component) {
		LogWriter.Instance.WriteToLog(component.DebugInfo(e.creationIndex, "replaced"), System.DateTime.UtcNow);
	}

	void updateName() {
		name = _entity.ToString();
	}
    }
}