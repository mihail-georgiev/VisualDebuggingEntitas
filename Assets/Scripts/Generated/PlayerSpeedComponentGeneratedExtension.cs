namespace Entitas {
    public partial class Entity {
        public PlayerSpeedComponent playerSpeed { get { return (PlayerSpeedComponent)GetComponent(ComponentIds.PlayerSpeed); } }

        public bool hasPlayerSpeed { get { return HasComponent(ComponentIds.PlayerSpeed); } }

        public void AddPlayerSpeed(PlayerSpeedComponent component) {
            AddComponent(ComponentIds.PlayerSpeed, component);
        }

        public void AddPlayerSpeed(float newSpeedX, float newSpeedY) {
            var component = new PlayerSpeedComponent();
            component.speedX = newSpeedX;
            component.speedY = newSpeedY;
            AddPlayerSpeed(component);
        }

        public void ReplacePlayerSpeed(float newSpeedX, float newSpeedY) {
            PlayerSpeedComponent component;
            if (hasPlayerSpeed) {
                WillRemoveComponent(ComponentIds.PlayerSpeed);
                component = playerSpeed;
            } else {
                component = new PlayerSpeedComponent();
            }
            component.speedX = newSpeedX;
            component.speedY = newSpeedY;
            ReplaceComponent(ComponentIds.PlayerSpeed, component);
        }

        public void RemovePlayerSpeed() {
            RemoveComponent(ComponentIds.PlayerSpeed);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherPlayerSpeed;

        public static AllOfMatcher PlayerSpeed {
            get {
                if (_matcherPlayerSpeed == null) {
                    _matcherPlayerSpeed = new Matcher(ComponentIds.PlayerSpeed);
                }

                return _matcherPlayerSpeed;
            }
        }
    }
}
