namespace Entitas {
    public partial class Entity {
        public AsteroidSpeedComponent asteroidSpeed { get { return (AsteroidSpeedComponent)GetComponent(ComponentIds.AsteroidSpeed); } }

        public bool hasAsteroidSpeed { get { return HasComponent(ComponentIds.AsteroidSpeed); } }

        public void AddAsteroidSpeed(AsteroidSpeedComponent component) {
            AddComponent(ComponentIds.AsteroidSpeed, component);
        }

        public void AddAsteroidSpeed(float newSpeed) {
            var component = new AsteroidSpeedComponent();
            component.speed = newSpeed;
            AddAsteroidSpeed(component);
        }

        public void ReplaceAsteroidSpeed(float newSpeed) {
            AsteroidSpeedComponent component;
            if (hasAsteroidSpeed) {
                WillRemoveComponent(ComponentIds.AsteroidSpeed);
                component = asteroidSpeed;
            } else {
                component = new AsteroidSpeedComponent();
            }
            component.speed = newSpeed;
            ReplaceComponent(ComponentIds.AsteroidSpeed, component);
        }

        public void RemoveAsteroidSpeed() {
            RemoveComponent(ComponentIds.AsteroidSpeed);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAsteroidSpeed;

        public static AllOfMatcher AsteroidSpeed {
            get {
                if (_matcherAsteroidSpeed == null) {
                    _matcherAsteroidSpeed = new Matcher(ComponentIds.AsteroidSpeed);
                }

                return _matcherAsteroidSpeed;
            }
        }
    }
}
