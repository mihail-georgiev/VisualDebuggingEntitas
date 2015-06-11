namespace Entitas {
    public partial class Entity {
        static readonly DestroyAsteroidComponent destroyAsteroidComponent = new DestroyAsteroidComponent();

        public bool isDestroyAsteroid {
            get { return HasComponent(ComponentIds.DestroyAsteroid); }
            set {
                if (value != isDestroyAsteroid) {
                    if (value) {
                        AddComponent(ComponentIds.DestroyAsteroid, destroyAsteroidComponent);
                    } else {
                        RemoveComponent(ComponentIds.DestroyAsteroid);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherDestroyAsteroid;

        public static AllOfMatcher DestroyAsteroid {
            get {
                if (_matcherDestroyAsteroid == null) {
                    _matcherDestroyAsteroid = new Matcher(ComponentIds.DestroyAsteroid);
                }

                return _matcherDestroyAsteroid;
            }
        }
    }
}
