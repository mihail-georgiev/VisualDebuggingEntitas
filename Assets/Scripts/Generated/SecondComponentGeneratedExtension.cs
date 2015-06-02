using Entitas;

namespace Entitas {
    public partial class Entity {
        static readonly SecondComponent secondComponent = new SecondComponent();

        public bool isSecond {
            get { return HasComponent(CoreComponentIds.Second); }
            set {
                if (value != isSecond) {
                    if (value) {
                        AddComponent(CoreComponentIds.Second, secondComponent);
                    } else {
                        RemoveComponent(CoreComponentIds.Second);
                    }
                }
            }
        }
    }
}

    public partial class CoreMatcher {
        static AllOfMatcher _matcherSecond;

        public static AllOfMatcher Second {
            get {
                if (_matcherSecond == null) {
                    _matcherSecond = new CoreMatcher(CoreComponentIds.Second);
                }

                return _matcherSecond;
            }
        }
    }
