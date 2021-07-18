namespace foxer.Render
{
    public interface INativeViewFactory
    {
        INativeView CreateView(IRenderer renderer);
    }
}
