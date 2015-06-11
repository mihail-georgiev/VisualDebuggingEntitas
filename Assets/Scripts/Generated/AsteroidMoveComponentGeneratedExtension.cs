namespace Entitas {
    public partial class Entity {
        public AsteroidMoveComponent asteroidMove { get { return (AsteroidMoveComponent)GetComponent(ComponentIds.AsteroidMove); } }

        public bool hasAsteroidMove { get { return HasComponent(ComponentIds.AsteroidMove); } }

        public void AddAsteroidMove(AsteroidMoveComponent component) {
            AddComponent(ComponentIds.AsteroidMove, component);
        }

        public void AddAsteroidMove(float newSpeed) {
            var component = new AsteroidMoveComponent();
            component.speed = newSpeed;
            AddAsteroidMove(component);
        }

        public void ReplaceAsteroidMove(float newSpeed) {
            AsteroidMoveComponent component;
            if (hasAsteroidMove) {
                WillRemoveComponent(ComponentIds.AsteroidMove);
                component = asteroidMove;
            } else {
                component = new AsteroidMoveComponent();
            }
            component.speed = newSpeed;
            ReplaceComponent(ComponentIds.AsteroidMove, component);
        }

        public void RemoveAsteroidMove() {
            RemoveComponent(ComponentIds.AsteroidMove);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherAsteroidMove;

        public static AllOfMatcher AsteroidMove {
            get {
                if (_matcherAsteroidMove == null) {
                    _matcherAsteroidMove = new Matcher(ComponentIds.AsteroidMove);
                }

                return _matcherAsteroidMove;
            }
        }
    }
}
