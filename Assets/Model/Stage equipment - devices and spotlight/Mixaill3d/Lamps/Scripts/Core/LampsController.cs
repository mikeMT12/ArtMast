using System.Collections.Generic;

namespace Mixaill3d.Lamps.Scripts.Core
{
    public class LampsController : MonoBehaviourSingletone<LampsController>
    {
        private List<LampsGroup> _lampsGroups = new List<LampsGroup>();
        public void RegisterGroup(LampsGroup lampsGroup)
        {
            _lampsGroups.Add(lampsGroup);
        }

        public void UnregisterGroup(LampsGroup lampsGroup)
        {
            _lampsGroups.Remove(lampsGroup);
        }

        public void Update()
        {
            foreach (var group in _lampsGroups)
                group.UpdateLightning();
        }
    }
}
