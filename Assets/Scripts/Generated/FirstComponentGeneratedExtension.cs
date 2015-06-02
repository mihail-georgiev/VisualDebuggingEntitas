using Entitas;

namespace Entitas {
    public partial class Entity {
        public FirstComponent first { get { return (FirstComponent)GetComponent(CoreComponentIds.First); } }

        public bool hasFirst { get { return HasComponent(CoreComponentIds.First); } }

        public void AddFirst(FirstComponent component) {
            AddComponent(CoreComponentIds.First, component);
        }

        public void AddFirst(float newX, int newY) {
            var component = new FirstComponent();
            component.x = newX;
            component.y = newY;
            AddFirst(component);
        }

        public void ReplaceFirst(float newX, int newY) {
            FirstComponent component;
            if (hasFirst) {
                WillRemoveComponent(CoreComponentIds.First);
                component = first;
            } else {
                component = new FirstComponent();
            }
            component.x = newX;
            component.y = newY;
            ReplaceComponent(CoreComponentIds.First, component);
        }

        public void RemoveFirst() {
            RemoveComponent(CoreComponentIds.First);
        }
    }
}

    public partial class CoreMatcher {
        static AllOfMatcher _matcherFirst;

        public static AllOfMatcher First {
            get {
                if (_matcherFirst == null) {
                    _matcherFirst = new CoreMatcher(CoreComponentIds.First);
                }

                return _matcherFirst;
            }
        }
    }
