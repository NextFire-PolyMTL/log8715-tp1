public class InitSys : ISystem
{
    public string Name => "InitSys";

    private uint max_id = 0;
    private bool initializing = true;

    public void UpdateSystem()
    {

        if (initializing)
        {
            foreach (var shape in ECSManager.Instance.Config.circleInstancesToSpawn)
            {
                ECSManager.Instance.CreateShape(max_id, shape.initialSize);
                max_id += 1;
            }
            initializing = false;
        }
    }
}
