//To execute command line put in the command buffer
public class CommandBufferSys : ISystem
{
    public string Name => nameof(CommandBufferSys);

    public void UpdateSystem()
    {
        var commandBuffer = World.Instance.GetSingleton<CommandBuffer>();
        var commands = commandBuffer.Value.Commands;
        while (commands.Count > 0)
        {
            commands.Dequeue().Invoke();
        }
    }
}
