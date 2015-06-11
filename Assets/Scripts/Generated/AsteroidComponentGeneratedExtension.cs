namespace Entitas {
    public partial class Entity {
        static readonly AsteroidComponent asteroidComponent = new AsteroidComponent();

        public bool isAsteroid {
            get { return HasComponent(ComponentIds.Asteroid); }
            set {
                if (value != isAsteroid) {
                    if (value) {
                        AddComponent(ComponentIds.Asteroid, asteroidComponent);
                    } else {
                        RemoveComponent(ComponentIds.Asteroid);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAsteroid;

        public static AllOfMatcher Asteroid {
            get {
                if (_matcherAsteroid == null) {
                    _matcherAsteroid = new Matcher(ComponentIds.Asteroid);
                }

                return _matcherAsteroid;
            }
        }
    }
}
