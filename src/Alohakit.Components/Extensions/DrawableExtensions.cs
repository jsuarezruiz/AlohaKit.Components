namespace Alohakit.Components.Extensions
{
    public static class DrawableExtensions
    {
        public static void DrawRippleEffect(this ICanvas canvas, RectF bounds, float cornerRadius, Point touchPoint, Color color, float animationPercent)
        {
            if (!bounds.Contains((float)touchPoint.X, (float)touchPoint.Y))
                return;

            canvas.SaveState();

            // Clip
            var path = new PathF();
            path.AppendRoundedRectangle(bounds, cornerRadius);
            canvas.ClipPath(path);

            canvas.FillColor = color;

            canvas.Alpha = 0.25f;

            float minimumRippleEffectSize = 0.0f;
            var rippleEffectSize = minimumRippleEffectSize.Lerp(bounds.Width, animationPercent);

            canvas.FillCircle((float)touchPoint.X, (float)touchPoint.Y, rippleEffectSize);

            canvas.RestoreState();
        }
    }
}