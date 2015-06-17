using Entitas;
using UnityEngine;

public class HitDetectionSystem : IExecuteSystem, ISetPool {
	Entity _player;
	Group _asteroids;
	
	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
		_asteroids = pool.GetGroup(Matcher.Asteroid);
	}
	
	public void Execute() {
		Vector2 playerPos = new Vector2 (_player.position.x, _player.position.y);
		foreach (var asteroid in _asteroids.GetEntities()) {
			Vector2 asteroidPos = new Vector2(asteroid.position.x, asteroid.position.y);
			if(checkForHit(playerPos, asteroidPos))	{
				Debug.Log (playerPos + " " + asteroidPos);
				Debug.Log("You have been hit!");
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#endif
			}
		}
	}

	bool checkForHit (Vector2 playerPos, Vector2 asteroidPos) {	
		float pX1 = playerPos.x - 8f;
		float pX2 = playerPos.x + 8f;
		float pY1 = playerPos.y - 8f;
		float pY2 = playerPos.y + 8f;
		float ax1 = asteroidPos.x + 4f;
		float ax2 = asteroidPos.x - 4f;
		float ay1 = asteroidPos.y + 4f;
		float ay2 = asteroidPos.y - 4f;
		if (((pX1 < ax1 && ax1 < pX2) || (pX1 < ax2 && ax2 < pX2)) 
			&& ((pY1 < ay1 && ay1 < pY2) || (pY1 < ay2 && ay2 < pY2)))
			return true;
		else 
			return false;
	}
}