using UnityEngine;

public class ClickSys : ISystem
{
    public string Name => "ClickSys";

    

    public void UpdateSystem()
    {
        
        //Trouver mieux /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseClick=(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
        World.Instance.ForEach<Position>((entity, position) =>
        {
            
            var radius = World.Instance.GetComponent<Size>(entity).Value.Radius;
           
            if (Mathf.Abs(mousePosition.x - position.Value.X)< radius/2 && Mathf.Abs(mousePosition.y - position.Value.Y)< radius/2
            && mouseClick)
            {
                World.Instance.SetComponent<IsClicked>(entity, new IsClicked());
            }
        });

    }
}
