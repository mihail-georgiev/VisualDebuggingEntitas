﻿using System.Collections.Generic;
using UnityEngine;

namespace Entitas.Unity.VisualProfilingTool {
    public class DebugPool : Pool {
        public string name { get { return _name; } }
        public GameObject entitiesContainer { get { return _entitiesContainer.gameObject; } }
        public Dictionary<IMatcher, Group> groups { get { return _groups; }}

        int _debugIndex;
        string _name;
        Transform _entitiesContainer;

        public DebugPool(int totalComponents, string name = "Debug Pool") : base(totalComponents + 1) {
            init(totalComponents, name);
        }

        public DebugPool(int totalComponents, int startCreationIndex, string name = "Debug Pool") : base(totalComponents + 1, startCreationIndex) {
            init(totalComponents, name);
        }

        void init(int totalComponents, string name) {
            _debugIndex = totalComponents;
            _name = name;
            _entitiesContainer = new GameObject().transform;
            _entitiesContainer.gameObject.AddComponent<PoolDebugBehaviour>().Init(this);
            updateName();
        }

        public override Entity CreateEntity() {
            var entity = base.CreateEntity();
			LogWriter.Instance.WriteToLog("Entity_" + entity._creationIndex + ":  -> created, ");
            addDebugComponent(entity);
            updateName();

            return entity;
        }

        public override void DestroyEntity(Entity entity) {
		LogWriter.Instance.WriteToLog("Entity_" + entity._creationIndex + ":  -> destroyed, ");
            	var debugComponent = (DebugComponent)entity.GetComponent(_debugIndex);
            	debugComponent.debugBehaviour.DestroyBehaviour();
	    	base.DestroyEntity(entity);
            	updateName();
	}

		public override void DestroyAllEntities() {
            base.DestroyAllEntities();
            if (_entitiesContainer != null) {
                Object.Destroy(_entitiesContainer.gameObject);
            }
        }
	
        void addDebugComponent(Entity entity) {
            var debugBehaviour = new GameObject().AddComponent<EntityDebugBehaviour>();
            debugBehaviour.Init(this, entity);
            debugBehaviour.transform.SetParent(_entitiesContainer, false);
            var debugComponent = new DebugComponent();
            debugComponent.debugBehaviour = debugBehaviour;
            entity.AddComponent(_debugIndex, debugComponent);
        }

        void updateName() {
            if (_entitiesContainer != null) {
                _entitiesContainer.name = string.Format(_name + " ({0} entities, {1} groups)", Count, groups.Count);
            }
        }
    }
}