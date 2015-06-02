using Entitas;

using System.Collections.Generic;

public static class CoreComponentIds {
    public const int First = 0;
    public const int Second = 1;
    public const int Third = 2;

    public const int TotalComponents = 3;

    static readonly Dictionary<int, string> components = new Dictionary<int, string> {
        { 0, "First" },
        { 1, "Second" },
        { 2, "Third" }
    };

    public static string IdToString(int componentId) {
        return components[componentId];
    }
}

public partial class CoreMatcher : AllOfMatcher {
    public CoreMatcher(int index) : base(new [] { index }) {
    }

    public override string ToString() {
        return CoreComponentIds.IdToString(indices[0]);
    }
}