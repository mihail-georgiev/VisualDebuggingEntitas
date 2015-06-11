namespace Entitas {
    public partial class Entity {
        public PlayerMoveComponent playerMove { get { return (PlayerMoveComponent)GetComponent(ComponentIds.PlayerMove); } }

        public bool hasPlayerMove { get { return HasComponent(ComponentIds.PlayerMove); } }

        public void AddPlayerMove(PlayerMoveComponent component) {
            AddComponent(ComponentIds.PlayerMove, component);
        }

        public void AddPlayerMove(float newSpeedX, float newSpeedY) {
            var component = new PlayerMoveComponent();
            component.speedX = newSpeedX;
            component.speedY = newSpeedY;
            AddPlayerMove(component);
        }

        public void ReplacePlayerMove(float newSpeedX, float newSpeedY) {
            PlayerMoveComponent component;
            if (hasPlayerMove) {
                WillRemoveComponent(ComponentIds.PlayerMove);
                component = playerMove;
            } else {
                component = new PlayerMoveComponent();
            }
            component.speedX = newSpeedX;
            component.speedY = newSpeedY;
            ReplaceComponent(ComponentIds.PlayerMove, component);
        }

        public void RemovePlayerMove() {
            RemoveComponent(ComponentIds.PlayerMove);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherPlayerMove;

        public static AllOfMatcher PlayerMove {
            get {
                if (_matcherPlayerMove == null) {
                    _matcherPlayerMove = new Matcher(ComponentIds.PlayerMove);
                }

                return _matcherPlayerMove;
            }
        }
    }
}
