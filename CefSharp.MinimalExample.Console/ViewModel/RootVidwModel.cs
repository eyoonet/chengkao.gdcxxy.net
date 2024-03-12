using Stylet;
using StyletIoC;

namespace console
{
    public class RootViewModel : Conductor<Screen>.Collection.OneActive
    {

        private readonly IContainer _container;
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootViewModel"/> class.
        /// </summary>
        /// <param name="container">The IoC container.</param>
        /// <param name="windowManager">The window manager.</param>
        public RootViewModel(IContainer container, IWindowManager windowManager)
        {
            _container = container;
            _windowManager = windowManager;
        }

        /// <inheritdoc/>
        protected override void OnViewLoaded()
        {
            InitViewModels();
        }

        private void InitViewModels()
        {
            var copilot = _container.Get<CopilotViewModel>();
            var settings = _container.Get<SettingsViewModel>();
            Items.Add(copilot);
            Items.Add(settings);
            ActiveItem = copilot;
        }

        private string _windowTitle = "广东财经大学";

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetAndNotify(ref _windowTitle, value);
        }

        /// <inheritdoc/>
        protected override void OnClose()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
