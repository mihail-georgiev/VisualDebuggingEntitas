using System.Collections.Generic;

public static class ComponentIds {
    public const int Asteroid = 0;
    public const int AsteroidSpeed = 1;
    public const int Bullet = 2;
    public const int DestroyAsteroid = 3;
    public const int DestroyBullet = 4;
    public const int GameObject = 5;
    public const int Player = 6;
    public const int PlayerSpeed = 7;
    public const int Position = 8;
    public const int Score = 9;
    public const int StopGame = 10;

    public const int TotalComponents = 11;

    static readonly Dictionary<int, string> components = new Dictionary<int, string> {
        { 0, "Asteroid" },
        { 1, "AsteroidSpeed" },
        { 2, "Bullet" },
        { 3, "DestroyAsteroid" },
        { 4, "DestroyBullet" },
        { 5, "GameObject" },
        { 6, "Player" },
        { 7, "PlayerSpeed" },
        { 8, "Position" },
        { 9, "Score" },
        { 10, "StopGame" }
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