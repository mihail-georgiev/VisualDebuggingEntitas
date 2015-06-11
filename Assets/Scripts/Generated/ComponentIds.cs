using System.Collections.Generic;

public static class ComponentIds {
    public const int Asteroid = 0;
    public const int AsteroidMove = 1;
    public const int DestroyAsteroid = 2;
    public const int GameObject = 3;
    public const int Player = 4;
    public const int PlayerMove = 5;
    public const int Position = 6;

    public const int TotalComponents = 7;

    static readonly Dictionary<int, string> components = new Dictionary<int, string> {
        { 0, "Asteroid" },
        { 1, "AsteroidMove" },
        { 2, "DestroyAsteroid" },
        { 3, "GameObject" },
        { 4, "Player" },
        { 5, "PlayerMove" },
        { 6, "Position" }
    };

    public static string IdToString(int componentId) {
        return components[componentId];
    }
}

namespace Entitas {
    public partial class Matcher : AllOfMatcher {
        public Matcher(int index) : base(new [] { index }) {
        }

        public override string ToString() {
            return ComponentIds.IdToString(indices[0]);
        }
    }
}