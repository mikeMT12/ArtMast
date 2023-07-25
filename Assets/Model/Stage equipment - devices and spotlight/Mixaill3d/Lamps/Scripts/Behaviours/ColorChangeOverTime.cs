using Mixaill3d.Lamps.Scripts.Core;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Behaviours
{
    [CreateAssetMenu(fileName = "ColorChangeOverTime", menuName = "Mixaill3d/LampsBehaviour/ColorChangeOverTime")]
    public class ColorChangeOverTime : LampBasicBehaviour
    {
        [SerializeField] protected Gradient _gradient;
        protected override void ProcessSingleLampBehaviour(LampInfo lampInfo, float timeOffset, float speed)
        {
            var currentTime = GetCurrentTime(timeOffset, speed);
            var color = _gradient.Evaluate(currentTime);
            SetColor(lampInfo, color);
        }
    }
}
