using Alohakit.Components.Core;
using AlohaKit.UI;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alohakit.Components
{
    public abstract class Component : TemplatedView, Core.IComponent, IDisposable
    {
        const string ElementCanvasView = "Part_Canvas";

        static Resources.Resources _resources;

        CanvasView _canvasView;
        ComponentState _componentState;

        public Component()
        {
            InitializeComponent();
        }

        public static new readonly BindableProperty VisualProperty = VisualComponent.VisualProperty;
       
        public new Visual Visual
        {
            get => (Visual)GetValue(VisualProperty);
            set => SetValue(VisualProperty, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ComponentState ComponentState
        {
            get => _componentState;
            set
            {
                _componentState = value;
                ChangeVisualState();
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvasView = GetTemplateChild(ElementCanvasView) as CanvasView;

            if (_canvasView != null)
            {
                _canvasView.StartInteraction += OnCanvasViewStartInteraction;
                _canvasView.StartHoverInteraction += OnCanvasViewHoverInteraction;
                _canvasView.MoveHoverInteraction += OnCanvasViewHoverInteraction;
                _canvasView.EndHoverInteraction += OnCanvasViewEndHoverInteraction;
                _canvasView.EndInteraction += OnCanvasViewEndInteraction;
                _canvasView.CancelInteraction += OnCanvasViewCancelInteraction;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
            {
                ComponentState = IsEnabled ? ComponentState.Normal : ComponentState.Disabled;
            }
        }

        public void Dispose()
        {
            if (_canvasView != null)
            {
                _canvasView.StartInteraction -= OnCanvasViewStartInteraction;
                _canvasView.StartHoverInteraction -= OnCanvasViewHoverInteraction;
                _canvasView.MoveHoverInteraction -= OnCanvasViewHoverInteraction;
                _canvasView.EndHoverInteraction -= OnCanvasViewEndHoverInteraction;
                _canvasView.EndInteraction -= OnCanvasViewEndInteraction;
                _canvasView.CancelInteraction -= OnCanvasViewCancelInteraction;
            }
        }

        public virtual void ChangeVisual()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        static Resources.Resources ComponentResources => _resources ??= new Resources.Resources();

        void InitializeComponent()
        {
            var application = Application.Current;

            if (application is not null && !application.Resources.MergedDictionaries.Contains(ComponentResources))
                application.Resources.Add(ComponentResources);

            _componentState = ComponentState.Normal;

            ChangeVisual();
        }

        void OnCanvasViewStartInteraction(object sender, TouchEventArgs e) => UpdateComponentState(ComponentState.Pressed);

        void OnCanvasViewHoverInteraction(object sender, TouchEventArgs e) => UpdateComponentState(ComponentState.Hovered);

        void OnCanvasViewEndHoverInteraction(object sender, EventArgs e) => UpdateComponentState(ComponentState.Normal);

        void OnCanvasViewEndInteraction(object sender, TouchEventArgs e) => UpdateComponentState(ComponentState.Normal);

        void OnCanvasViewCancelInteraction(object sender, EventArgs e) => UpdateComponentState(ComponentState.Normal);

        void UpdateComponentState(ComponentState componentState)
        {
            if (!IsEnabled)
                return;

            ComponentState = componentState;
        }
    }
}