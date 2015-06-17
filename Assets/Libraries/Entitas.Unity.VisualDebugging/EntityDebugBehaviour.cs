﻿using UnityEngine;
using Entitas;

namespace Entitas.Unity.VisualDebugging {
    public class EntityDebugBehaviour : MonoBehaviour {
        public Pool pool { get { return _pool; } }
        public Entity entity { get { return _entity; } }
        public bool[] unfoldedComponents { get { return _unfoldedComponents; } }

        Pool _pool;
        Entity _entity;
        int _debugIndex;
        bool[] _unfoldedComponents;

        public void Init(Pool pool, Entity entity, int debugIndex) {
            _pool = pool;
            _entity = entity;
            _debugIndex = debugIndex;
            _unfoldedComponents = new bool[_pool.totalComponents];
            _entity.OnComponentAdded += onEntityChanged;
            _entity.OnComponentRemoved += onEntityChanged;
			_entity.OnComponentAdded += logOnComponentAdded;
			_entity.OnComponentReplaced += logOnComponentReplaced;
			_entity.OnComponentRemoved += logOnComponentRemoved;
            updateName();

            UnfoldAllComponents();
        }

        public void UnfoldAllComponents() {
            for (int i = 0; i < _unfoldedComponents.Length; i++) {
                _unfoldedComponents[i] = true;
            }
        }

        public void FoldAllComponents() {
            for (int i = 0; i < _unfoldedComponents.Length; i++) {
                _unfoldedComponents[i] = false;
            }
        }

        public void DestroyBehaviour() {
            _entity.OnComponentAdded -= onEntityChanged;
            _entity.OnComponentRemoved -= onEntityChanged;
            if (this != null) {
                Destroy(gameObject);
            }
        }

        void onEntityChanged(Entity e, int index, IComponent component) {
            if (!e.HasComponent(_debugIndex)) {
                DestroyBehaviour();
            } else {
                updateName();
            }
        }

		void logOnComponentAdded(Entity e, int index, IComponent component) {
			AppUtils.writer.WriteToLog(component.DebugInfo(e.creationIndex, "added", System.DateTime.UtcNow));
		}

		void logOnComponentRemoved(Entity e, int index, IComponent component) {
			AppUtils.writer.WriteToLog(component.DebugInfo(e.creationIndex, "removed", System.DateTime.UtcNow));
		}

		void logOnComponentReplaced(Entity e, int index, IComponent component) {
			AppUtils.writer.WriteToLog(component.DebugInfo(e.creationIndex, "replaced", System.DateTime.UtcNow));
		}

		
		void updateName() {
			name = _entity.ToString();
		}
    }
}