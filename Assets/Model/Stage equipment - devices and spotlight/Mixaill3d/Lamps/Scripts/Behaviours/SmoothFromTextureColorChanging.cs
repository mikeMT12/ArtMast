using Mixaill3d.Lamps.Scripts.Core;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Behaviours
{
    [CreateAssetMenu(fileName = "SmoothFromTextureColorChanging", menuName = "Mixaill3d/LampsBehaviour/SmoothFromTextureColorChanging")]
    public class SmoothFromTextureColorChanging : LampBasicBehaviour
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
            var renderer = lampInfo.Renderer;
            var prevColor = GetColor(renderer, currentFrame - 1, frameAmountX);
            var currentColor = GetColor(renderer, currentFrame, frameAmountX);
            var color = Color.Lerp(prevColor, currentColor, time * frameAmount - currentFrame);
            SetColor(lampInfo, color);
        }

        private Color GetColor(Renderer renderer, int currentFrame, int frameAmountX)
        {
            int frameX = currentFrame % frameAmountX;
            int frameY = currentFrame / frameAmountX;

            int id = renderer.transform.GetSiblingIndex();
            int idX = (id - 1) % _frameWidth;
            int idY = (id - 1) / _frameWidth;

            var color = texture.GetPixel(frameX * _frameWidth + idX, frameY * _frameHeight + (_frameHeight - idY - 1));
            return color;
        }
    }
}
