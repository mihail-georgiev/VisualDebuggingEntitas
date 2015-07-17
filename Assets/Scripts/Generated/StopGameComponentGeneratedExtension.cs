namespace Entitas {
    public partial class Entity {
        static readonly StopGameComponent stopGameComponent = new StopGameComponent();

        public bool isStopGame {
            get { return HasComponent(ComponentIds.StopGame); }
            set {
                if (value != isStopGame) {
                    if (value) {
                        AddComponent(ComponentIds.StopGame, stopGameComponent);
                    } else {
                        RemoveComponent(ComponentIds.StopGame);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity stopGameEntity { get { return GetGroup(Matcher.StopGame).GetSingleEntity(); } }

        public bool isStopGame {
            get { return stopGameEntity != null; }
            set {
                var entity = stopGameEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isStopGame = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherStopGame;

        public static AllOfMatcher StopGame {
            get {
                if (_matcherStopGame == null) {
                    _matcherStopGame = new Matcher(ComponentIds.StopGame);
                }

                return _matcherStopGame;
            }
        }
    }
}
