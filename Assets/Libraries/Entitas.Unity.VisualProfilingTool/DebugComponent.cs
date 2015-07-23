using Entitas.CodeGenerator;

namespace Entitas.Unity.VisualProfilingTool {
    [DontGenerate(false)]
    public class DebugComponent : IComponent {
        public EntityDebugBehaviour debugBehaviour;
    }
}