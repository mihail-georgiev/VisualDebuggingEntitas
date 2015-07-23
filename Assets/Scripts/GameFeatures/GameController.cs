using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System;
using Entitas.Unity.VisualProfilingTool;

public class GameController : MonoBehaviour {
	Pool _pool;
	IStartSystem[] _startSystems;
	IExecuteSystem[] _executeSystems;
	
	[HideInInspector]
	public bool runSystems = true;
	
	void Start () {
		#if (UNITY_EDITOR)
		_pool = new DebugPool(ComponentIds.TotalComponents);
		#else
		_pool = new Pool(ComponentIds.TotalComponents);
		#endif
		
		createStartSystems();
		startSystems();
		createExecuteSystems();
	}
	
	void createStartSystems () {
		_startSystems = new [] {
			_pool.CreateStartSystem<InitGameSystem>(),
		};
	}
	
	void createExecuteSystems () {
		_executeSystems = new []{
			_pool.CreateExecuteSystem<PlayerInputSystem>(),
			_pool.CreateExecuteSystem<PlayerMoveSystem>(),
			_pool.CreateExecuteSystem<SpawnAsteroidsSystem>(),
			_pool.CreateExecuteSystem<HitDetectionSystem>(),
			_pool.CreateExecuteSystem<ScoreSystem>(),
			_pool.CreateExecuteSystem<AsteroidMoveSystem>(),
			_pool.CreateExecuteSystem<BulletMoveSystem>(),
			_pool.CreateExecuteSystem<RenderPositionSystem>(),
			_pool.CreateExecuteSystem<DestroyAsteroidsSystem>(),
			_pool.CreateExecuteSystem<DestroyBulletSystem>(),
			_pool.CreateExecuteSystem<StopGameSystem>()
		};
	}
	
	void startSystems () {
		foreach (var system in _startSystems) {
			system.Start();
		}
	}
	
	void Update () {
		if(runSystems){
			foreach (var system in _executeSystems) {
				system.Execute();
			}
		}
	}
	
	void OnApplicationQuit() {
		LogWriter.Instance.CloseLog();
	}
}