using Spectre.Console;

namespace Sandbox.Interfaces;

internal interface IDisplayLayout
{
    public Layout Layout { get; }
    public void Update();
}