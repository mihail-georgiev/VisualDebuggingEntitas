using Entitas;
using UnityEngine;

public class HitDetectionSystem : IExecuteSystem, ISetPool {
	Entity _player, _score;
	Group _asteroids;
	Group _bullets;
	Pool _pool;
	
	public void SetPool(Pool pool) {
		_player = pool.playerEntity;
		_asteroids = pool.GetGroup(Matcher.AllOf(Matcher.Asteroid));
		_bullets = pool.GetGroup (Matcher.AllOf(Matcher.Bullet));
		_score = pool.scoreEntity;
		_pool = pool;
	}
	
	public void Execute() {
		checkForBulletHittingAsteroid ();
		checkForAsteroidHittingPlayer ();
	}

	void checkForBulletHittingAsteroid(){	
		foreach(var asteroid in _asteroids.GetEntities()){
			foreach (var bullet in _bullets.GetEntities()){
				if(checkForHitWithBullet(bullet.position, asteroid.position)){
					_score.ReplaceScore(_score.score.score + 10);
					bullet.isDestroyBullet = true;
					asteroid.isDestroyAsteroid = true;
				}
			}
		}
	}

	void checkForAsteroidHittingPlayer ()
	{
		foreach (var asteroid in _asteroids.GetEntities ()) {
			if (checkForHitWithPlayer (_player.position, asteroid.position)) {
				Entity stopGameEntity = _pool.CreateEntity();
				stopGameEntity.isStopGame = true;
			}
		}
	}

	bool checkForHitWithBullet (PositionComponent bulletPos, PositionComponent asteroidPos) {
		Rect bulletArea = new Rect (bulletPos.x - 3, bulletPos.y + 3, 6, 6);
		Rect asteroidArea = new Rect (asteroidPos.x - 4, asteroidPos.y + 4, 8, 8);
		
		if(bulletArea.Overlaps(asteroidArea))
			return true;
		else 
			return false;
	}

	bool checkForHitWithPlayer (PositionComponent playerPos, PositionComponent asteroidPos) {	
		Rect playerArea = new Rect (playerPos.x, playerPos.y + 6, 10, 12);
		Rect asteroidArea = new Rect (asteroidPos.x - 4, asteroidPos.y + 4, 8, 8);

		if(playerArea.Overlaps(asteroidArea))
			return true;
		else 
			return false;
	}
}