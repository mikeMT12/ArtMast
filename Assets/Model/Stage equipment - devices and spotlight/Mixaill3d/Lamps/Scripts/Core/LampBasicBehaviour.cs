using System;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Core
{
    public class LampBasicBehaviour : ScriptableObject
    {
        public virtual void ProcessBehaviour(LampInfo[] lampInfos, float timeOffset, float speed)
        {
            foreach (var lamp in lampInfos)
            {
                ProcessSingleLampBehaviour(lamp, timeOffset, speed);
            }
        }

        protected void SetColor(LampInfo lampInfo, Color color)
        {
            var renderer = lampInfo.Renderer;
            var additionalRenderer = lampInfo.LightCone;
            SetMaterialColor(color, renderer, "_EmissionColor", true);
            SetMaterialColor(color, additionalRenderer, "_Color", false);
            var light = lampInfo.Light;
            if (light != null)
            {
                light.color = color;
            }
        }

        private static void SetMaterialColor(Color color, Renderer renderer, string colorName, bool changeAlpha)
        {
            if (renderer == null)
                return;
            var material = renderer.material;
            var colorToApply = color;
            if (!changeAlpha)
            {
                var materialColor = material.GetColor(colorName);
                colorToApply.a = materialColor.a;
            }
            ////
            if (colorName == "_EmissionColor")
            {
                colorToApply *= Mathf.Pow(2.0F, 1.2f- 0.4169F);
            }
            ////
            material.SetColor(colorName, colorToApply);
        }

        protected virtual void ProcessSingleLampBehaviour(LampInfo lampInfo, float timeOffset, float speed)
        {
            throw new Exception("It is basic behaviour. There is no realization.");
        }
        
        protected float GetCurrentTime(float timeOffset, float speed)
        {
            return Mathf.Repeat(Time.time * speed + timeOffset, 1f);
        }
    }
}
