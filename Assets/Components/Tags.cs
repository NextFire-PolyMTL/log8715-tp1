//To tag entities which are static
public readonly struct IsStatic : IComponent { }

//To tag entities which are protected
public readonly struct IsProtected : IComponent { }

//To tag entities which are colliding each other or the border of the screen
public readonly struct IsColliding : IComponent { }

//To tag entities which are clicked by the user
public readonly struct IsClicked : IComponent { }

//To tag entities which result of an explosion due to a mouse click
public readonly struct BornOfClick : IComponent { }

// To tag entities which should be ignored by the physic systems
public readonly struct PhysicsIgnore : IComponent { }
