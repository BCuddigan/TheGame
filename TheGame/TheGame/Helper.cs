using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public static class Helper
    {
        public static Matrix GlobalTransformation(Vector2 backBufferSize, Vector2 baseScreenSize)
        {
            float horScaling = backBufferSize.X / baseScreenSize.X;
            float verScaling = backBufferSize.Y / baseScreenSize.Y;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);

            return Matrix.CreateScale(screenScalingFactor);
        }

    }
}
