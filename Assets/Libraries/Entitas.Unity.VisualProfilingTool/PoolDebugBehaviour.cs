using UnityEngine;

namespace Entitas.Unity.VisualProfilingTool {
    public class PoolDebugBehaviour : MonoBehaviour {
        public DebugPool pool { get { return _pool; } }

        DebugPool _pool;

        public void Init(DebugPool pool) {
            _pool = pool;
        }
    }
}