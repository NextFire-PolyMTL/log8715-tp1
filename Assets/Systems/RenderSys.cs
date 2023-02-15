using UnityEngine;

//Make the render of the components' update in the game
public class RenderSys : ISystem
{
    public string Name => nameof(RenderSys);

    public void UpdateSystem()
    {

        //Render of entities' position
        World.Instance.ForEach<Position>((entity, position) =>
        {
            ECSManager.Instance.UpdateShapePosition(entity.Id, new Vector2(position.Value.X, position.Value.Y));
        });

        //Render of entities' size
        World.Instance.ForEach<Size>((entity, size) =>
        {
            ECSManager.Instance.UpdateShapeSize(entity.Id, size.Value.Scale);
        });

        //Render of entities' color
        World.Instance.ForEach<ColorCompo>((entity, colorCompo) =>
        {
            if (colorCompo.HasValue)
            {
                ECSManager.Instance.UpdateShapeColor(entity.Id, colorCompo.Value.ShapeColor);
            }
        });
    }
}
