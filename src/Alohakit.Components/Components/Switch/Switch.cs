using Alohakit.Components.Core;
using Alohakit.Components.Core.Material;
using Alohakit.Components.Extensions;
using AlohaKit.UI;
using System.ComponentModel;
using System.Windows.Input;

namespace Alohakit.Components
{
    public class Switch : Component, ISwitch
    {
        const string ElementCanvasView = "Part_Canvas";
        const string ElementTrack = "Part_Track";
        const string ElementOutline = "Part_Track_Outline";
        const string ElementThumbEffect = "Part_Thumb_Effect";
        const string ElementThumb = "Part_Thumb";

        CanvasView _canvasView;
        RoundRectangle _track;
        RoundRectangle _outline;
        Shape _thumbEffect;
        float _thumbEffectX;
        Shape _thumb;
        float _thumbX;

        IAnimationManager _animationManager;


        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(Switch),
            false,
            propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                ((Switch)bindableObject).OnCheckedChanged();
            });

        public static readonly BindableProperty TrackColorProperty = SwitchComponent.TrackColorProperty;
        public static readonly BindableProperty TrackOpacityProperty = SwitchComponent.TrackOpacityProperty;

        public static readonly BindableProperty ThumbColorProperty = SwitchComponent.ThumbColorProperty;
        public static readonly BindableProperty ThumbOpacityProperty = SwitchComponent.ThumbOpacityProperty;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(Switch),
            null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(Switch),
            null);

        public static readonly BindableProperty OutlineColorProperty = OutlineComponent.OutlineColorProperty;
        public static readonly BindableProperty OutlineWidthProperty = OutlineComponent.OutlineWidthProperty;

        public static readonly BindableProperty RippleColorProperty = RippleComponent.RippleColorProperty;

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public Color TrackColor
        {
            get => (Color)GetValue(TrackColorProperty);
            set => SetValue(TrackColorProperty, value);
        }

        public double TrackOpacity
        {
            get => (double)GetValue(TrackOpacityProperty);
            set => SetValue(TrackOpacityProperty, value);
        }

        public Color ThumbColor
        {
            get => (Color)GetValue(ThumbColorProperty);
            set => SetValue(ThumbColorProperty, value);
        }

        public double ThumbOpacity
        {
            get => (double)GetValue(ThumbOpacityProperty);
            set => SetValue(ThumbOpacityProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public Color OutlineColor
        {
            get => (Color)GetValue(OutlineColorProperty);
            set => SetValue(OutlineColorProperty, value);
        }

        public int OutlineWidth
        {
            get => (int)GetValue(OutlineWidthProperty);
            set => SetValue(OutlineWidthProperty, value);
        }

        public Color RippleColor
        {
            get => (Color)GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Point TouchPoint { get; set; } = Point.Zero;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public float RippleProgress { get; set; } = 0f;

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal float ThumbPositionProgress { get; private set; } = 1f;

        public event EventHandler<CheckedChangedEventArgs> CheckedChanged;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvasView = GetTemplateChild(ElementCanvasView) as CanvasView;
            _track = GetTemplateChild(ElementTrack) as RoundRectangle;
            _outline = GetTemplateChild(ElementOutline) as RoundRectangle;
            _thumbEffect = GetTemplateChild(ElementThumbEffect) as Shape;

            if (_thumbEffect != null)
                _thumbEffectX = _thumbEffect.X;

            _thumb = GetTemplateChild(ElementThumb) as Shape;

            if (_thumb != null)
                _thumbX = _thumb.X;

            if (_canvasView != null)
            {
                _canvasView.Drawing += OnCanvasViewDrawing;

                _canvasView.StartInteraction += OnCanvasViewStartInteraction;
                _canvasView.EndInteraction += OnCanvasViewEndInteraction;
            }

            UpdateSwitch();
        }

        protected override void ChangeVisualState()
        {
            string state;

            switch (ComponentState)
            {
                case ComponentState.Normal:
                    state = IsChecked ? "normal:checked" : "normal";
                    break;
                case ComponentState.MouseOver:
                    state = IsChecked ? "mouseover:checked" : "mouseover";
                    break;
                case ComponentState.Pressed:
                    state = IsChecked ? "pressed:checked" : "pressed";
                    break;
                case ComponentState.Disabled:
                    state = IsChecked ? "disabled:checked" : "disabled";
                    break;
                default:
                    state = "normal";
                    break;
            }

            VisualStateManager.GoToState(this, state);
            _canvasView?.Invalidate();
        }

        public override void ChangeVisual()
        {
            switch (Visual)
            {
                case Visual.Cupertino:
                    Application.Current.Resources.TryGetValue("CupertinoSwitchStyle", out object cupertinoControlStyle);
                    Style = cupertinoControlStyle as Style;
                    break;
                case Visual.Fluent:
                    Application.Current.Resources.TryGetValue("FluentSwitchStyle", out object fluentControlStyle);
                    Style = fluentControlStyle as Style;
                    break;
                case Visual.Material:
                    Application.Current.Resources.TryGetValue("MaterialSwitchStyle", out object materialControlStyle);
                    Style = materialControlStyle as Style;
                    break;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StartRippleEffect()
        {
            if (Visual != Visual.Material)
                return;

            _animationManager ??= Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

            var start = -1f;
            var end = 1f;

            _animationManager?.Add(
                new Microsoft.Maui.Animations.Animation(
                    callback: (progress) =>
                    {
                        RippleProgress = start.Lerp(end, progress);
                        _canvasView?.Invalidate();
                    },
                    duration: 0.3f,
                    easing: Easing.SinInOut,
                    finished: () =>
                    {
                        RippleProgress = 0;
                        _canvasView?.Invalidate();
                    }
                ));
        }

        void OnCanvasViewDrawing(object sender, DrawingEventArgs e)
        {
            var canvas = e.Canvas;
            var bounds = e.Bounds;

            UpdateSwitch();

            DrawRippleEffect(canvas, bounds);
        }

        void OnCanvasViewStartInteraction(object sender, TouchEventArgs e)
        {
            if (!IsEnabled)
                return;

            TouchPoint = e.Touches[0];

            StartRippleEffect();

            IsChecked = !IsChecked;
        }

        void OnCanvasViewEndInteraction(object sender, TouchEventArgs e)
        {
            if (RippleProgress == 1f)
                RippleProgress = 0f;
        }

        void UpdateSwitch()
        {
            if (_track != null)
            {
                var cornerRadius = Math.Min(_track.WidthRequest, _track.HeightRequest) / 2;
                _track.CornerRadius = cornerRadius;
            }

            if (_outline != null)
            {
                var cornerRadius = Math.Min(_outline.WidthRequest, _outline.HeightRequest) / 2;
                _outline.CornerRadius = cornerRadius;
            }

            if (_thumbEffect != null)
            {
                var lX = _thumbEffectX;
                var rX = (float)(_canvasView.WidthRequest - (lX + _thumbEffect.WidthRequest));
                _thumbEffect.X = GetThumbPosition(lX, rX);

                var isAnimating = ThumbPositionProgress != 1.0;
                _thumbEffect.IsVisible = IsPointerOver && !isAnimating;
            }

            if (_thumb != null)
            {
                var lX = _thumbX;
                var rX = (float)(_canvasView.WidthRequest - (lX + _thumb.WidthRequest));
                _thumb.X = GetThumbPosition(lX, rX);
            }
        }

        float GetThumbPosition(float lX, float rX)
        {
            var isAnimating = ThumbPositionProgress != 1.0;

            float cX;

            if (IsChecked)
                cX = isAnimating ? lX : rX;
            else
                cX = isAnimating ? rX : lX;

            if (isAnimating)
            {
                if (IsChecked)
                    cX = ThumbPositionProgress < 0.25f ? lX : rX;
                else
                    cX = ThumbPositionProgress < 0.25f ? rX : lX;
            }

            return cX;
        }

        void DrawRippleEffect(ICanvas canvas, RectF bounds)
        {
            if (Visual != Visual.Material)
                return;

            if (RippleProgress < 0)
                return;

            var rippleBounds = _track != null ? new RectF(_track.X, _track.Y, _track.WidthRequest, _track.HeightRequest) : bounds;
            var rippleColor = RippleColor;
            var point = TouchPoint;

            var cornerRadius = Math.Min(bounds.Width, bounds.Height) / 2;

            canvas.DrawRippleEffect(
                rippleBounds,
                cornerRadius,
                point,
                rippleColor,
                RippleProgress
            );
        }

        void UpdateThumbPosition()
        {
            if (Handler is null)
                return;

            _animationManager ??=
                Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();

            ThumbPositionProgress = 0f;
            var start = 0f;
            var end = 1f;

            _animationManager?.Add(
                new Microsoft.Maui.Animations.Animation(
                    callback: (progress) =>
                    {
                        ThumbPositionProgress = start.Lerp(end, progress);
                        _canvasView?.Invalidate();
                    },
                    duration: 0.5f,
                    easing: Easing.CubicInOut
                )
            );
        }

        void OnCheckedChanged()
        {
            ChangeVisualState();
            UpdateThumbPosition();

            CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(IsChecked));
            Command?.Execute(CommandParameter ?? IsChecked);
        }
    }
}