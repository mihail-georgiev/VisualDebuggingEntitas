using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class StopGameSystem : IReactiveSystem, ISetPool{
	Pool _pool;
	GameObject _gameOverPanel;
	Text _infoLabel;

	public IMatcher GetTriggeringMatcher() {
		return Matcher.StopGame;
	}
	
	public GroupEventType GetEventType() {
		return GroupEventType.OnEntityAdded;
	}

	public void SetPool(Pool pool) {
		_pool = pool;
		_gameOverPanel = GameObject.Find("GameOverPanel");
		_infoLabel = GameObject.Find("InfoLabel").GetComponent<Text>();
		_gameOverPanel.SetActive(false);
	}

	public void Execute(Entity[] entities) {
		GameObject.FindObjectOfType<GameController>().runSystems = false;
		_gameOverPanel.SetActive(true);
		_infoLabel.text = "Game Over\nYou got: " + _pool.score.score + " points!";
		_pool.DestroyAllEntities();
	}
}