using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : IReactiveSystem, ISetPool{
	Pool _pool;
	Text _scoreLabel;

	public IMatcher GetTriggeringMatcher() {
		return Matcher.AllOf(Matcher.Score);
	}
	
	public GroupEventType GetEventType() {
		return GroupEventType.OnEntityAdded;
	}

	public void SetPool(Pool pool) {
		_pool = pool;
		_scoreLabel = GameObject.Find("ScoreLabel").GetComponent<Text>();
	}

	public void Execute(Entity[] entities) {
		_scoreLabel.text = "Score: " + _pool.score.score;
	}
}