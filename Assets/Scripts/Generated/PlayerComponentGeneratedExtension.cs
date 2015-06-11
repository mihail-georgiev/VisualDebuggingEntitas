namespace Entitas {
    public partial class Entity {
        static readonly PlayerComponent playerComponent = new PlayerComponent();

        public bool isPlayer {
            get { return HasComponent(ComponentIds.Player); }
            set {
                if (value != isPlayer) {
                    if (value) {
                        AddComponent(ComponentIds.Player, playerComponent);
                    } else {
                        RemoveComponent(ComponentIds.Player);
                    }
                }
            }
        }
    }

    public partial class Pool {
        public Entity playerEntity { get { return GetGroup(Matcher.Player).GetSingleEntity(); } }

        public bool isPlayer {
            get { return playerEntity != null; }
            set {
                var entity = playerEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isPlayer = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherPlayer;

        public static AllOfMatcher Player {
            get {
                if (_matcherPlayer == null) {
                    _matcherPlayer = new Matcher(ComponentIds.Player);
                }

                return _matcherPlayer;
            }
        }
    }
}
