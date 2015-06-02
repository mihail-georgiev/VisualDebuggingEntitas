using Entitas;

namespace Entitas {
    public partial class Entity {
        public ThirdComponent third { get { return (ThirdComponent)GetComponent(CoreComponentIds.Third); } }

        public bool hasThird { get { return HasComponent(CoreComponentIds.Third); } }

        public void AddThird(ThirdComponent component) {
            AddComponent(CoreComponentIds.Third, component);
        }

        public void AddThird(System.Collections.Generic.Dictionary<string, int> newDict) {
            var component = new ThirdComponent();
            component.dict = newDict;
            AddThird(component);
        }

        public void ReplaceThird(System.Collections.Generic.Dictionary<string, int> newDict) {
            ThirdComponent component;
            if (hasThird) {
                WillRemoveComponent(CoreComponentIds.Third);
                component = third;
            } else {
                component = new ThirdComponent();
            }
            component.dict = newDict;
            ReplaceComponent(CoreComponentIds.Third, component);
        }

        public void RemoveThird() {
            RemoveComponent(CoreComponentIds.Third);
        }
    }
}

    public partial class CoreMatcher {
        static AllOfMatcher _matcherThird;

        public static AllOfMatcher Third {
            get {
                if (_matcherThird == null) {
                    _matcherThird = new CoreMatcher(CoreComponentIds.Third);
                }

                return _matcherThird;
            }
        }
    }
