namespace SFR.SceneSurfer
{
    public abstract class WindowBuilder
    {
        private readonly EditorWindowInitializer _editorWindowInitializer;

        public WindowBuilder(EditorWindowInitializer window)
        {
            _editorWindowInitializer = window;
            Initialize();
        }

        private void Initialize()
        {
            CreateUI(_editorWindowInitializer);
            RegisterEvents();
        }

        protected abstract void CreateUI(EditorWindowInitializer windowInitializer);

        protected abstract void RegisterEvents();

        public abstract void Cleanup();
    }
}