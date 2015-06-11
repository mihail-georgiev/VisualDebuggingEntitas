namespace Entitas {
    public partial class Entity {
        public PositionComponent position { get { return (PositionComponent)GetComponent(ComponentIds.Position); } }

        public bool hasPosition { get { return HasComponent(ComponentIds.Position); } }

        public void AddPosition(PositionComponent component) {
            AddComponent(ComponentIds.Position, component);
        }

        public void AddPosition(float newX, float newY) {
            var component = new PositionComponent();
            component.x = newX;
            component.y = newY;
            AddPosition(component);
        }

        public void ReplacePosition(float newX, float newY) {
            PositionComponent component;
            if (hasPosition) {
                WillRemoveComponent(ComponentIds.Position);
                component = position;
            } else {
                component = new PositionComponent();
            }
            component.x = newX;
            component.y = newY;
            ReplaceComponent(ComponentIds.Position, component);
        }

        public void RemovePosition() {
            RemoveComponent(ComponentIds.Position);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherPosition;

        public static AllOfMatcher Position {
            get {
                if (_matcherPosition == null) {
                    _matcherPosition = new Matcher(ComponentIds.Position);
                }

                return _matcherPosition;
            }
        }
    }
}
