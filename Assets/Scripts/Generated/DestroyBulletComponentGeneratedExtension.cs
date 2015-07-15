namespace Entitas {
    public partial class Entity {
        static readonly DestroyBulletComponent destroyBulletComponent = new DestroyBulletComponent();

        public bool isDestroyBullet {
            get { return HasComponent(ComponentIds.DestroyBullet); }
            set {
                if (value != isDestroyBullet) {
                    if (value) {
                        AddComponent(ComponentIds.DestroyBullet, destroyBulletComponent);
                    } else {
                        RemoveComponent(ComponentIds.DestroyBullet);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherDestroyBullet;

        public static AllOfMatcher DestroyBullet {
            get {
                if (_matcherDestroyBullet == null) {
                    _matcherDestroyBullet = new Matcher(ComponentIds.DestroyBullet);
                }

                return _matcherDestroyBullet;
            }
        }
    }
}
