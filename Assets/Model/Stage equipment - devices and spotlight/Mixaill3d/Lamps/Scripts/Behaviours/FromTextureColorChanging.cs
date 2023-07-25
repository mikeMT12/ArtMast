using Mixaill3d.Lamps.Scripts.Core;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Behaviours
{
    [CreateAssetMenu(fileName = "FromTextureColorChanging", menuName = "Mixaill3d/LampsBehaviour/FromTextureColorChanging")]
    public class FromTextureColorChanging : LampBasicBehaviour
    {
        [SerializeField] private int _frameWidth;
        [SerializeField] private int _frameHeight;
        [SerializeField] private Texture2D texture;
        
        protected override void ProcessSingleLampBehaviour(LampInfo lampInfo, float timeOffset, float speed)
        {
            float time = GetCurrentTime(timeOffset, speed);
            var frameAmountX = texture.width/_frameWidth;
            var frameAmountY = texture.height/_frameHeight;
            int frameAmount = frameAmountX * frameAmountY;
            int currentFrame = (int)(time * frameAmount);
            int frameX = currentFrame % frameAmountX;
            int frameY = currentFrame / frameAmountX;

            var transform = lampInfo.Renderer.transform;
            int id = transform.GetSiblingIndex();
            int idX = (id - 1) % _frameWidth;
            int idY = (id - 1) / _frameWidth;

            var color = texture.GetPixel(frameX * _frameWidth + idX, frameY * _frameHeight + (_frameHeight - idY - 1));
            SetColor(lampInfo, color);
        }
    }
}
