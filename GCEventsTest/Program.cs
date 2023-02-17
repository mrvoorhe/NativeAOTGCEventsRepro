// See https://aka.ms/new-console-template for more information

using System.Diagnostics.Tracing;

using var listener = new GCEventListener();

Console.WriteLine("Hello, World!");

System.Text.StringBuilder sb = new();
for (int i = 0; i < 1000000; i++)
{
    sb.Append(i.ToString());
}

Console.WriteLine("Done");

internal class GCEventListener : EventListener
{
    protected override ClrRuntimeEventKeywords ListenForKeywords => ClrRuntimeEventKeywords.GC;
    
    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {
        if (eventData.EventName != null && eventData.EventName.StartsWith("GCStart"))
        {
            var generation = (uint)eventData.Payload![1]!;
            Console.WriteLine($"GC - Gen {generation}");
        }
        else if (eventData.EventName != null && eventData.EventName.StartsWith("GCEnd"))
        {
            var generation = (uint)eventData.Payload![1]!;
            Console.WriteLine($"GC End - Gen - {generation}");
        }
    }
}

public abstract class EventListener : System.Diagnostics.Tracing.EventListener
{
    private EventSource? _subscribedEventSource;

    [Flags]
    protected enum ClrRuntimeEventKeywords
    {
        GC = 0x1,
    }

    // This must return a const or static value
    // OnEventSourceCreated is called from the EventLister::.ctor, so this
    // property will be called before the derived class constructor has run
    protected abstract ClrRuntimeEventKeywords ListenForKeywords { get; }

    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        Console.WriteLine($"Got Event Source");
        switch (eventSource.Name)
        {
            case "Microsoft-Windows-DotNETRuntime":
                EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)ListenForKeywords);
                _subscribedEventSource = eventSource;
                break;
        }
    }

    public override void Dispose()
    {
        if (_subscribedEventSource != null)
            DisableEvents(_subscribedEventSource);

        base.Dispose();
    }
}