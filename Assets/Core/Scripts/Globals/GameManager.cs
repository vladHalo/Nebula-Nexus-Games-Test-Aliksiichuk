public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public BezierCurve bezierCurve;
    //public BackpackPlayer backpackPlayer;
    
    protected override void Awake()
    {
        base.Awake();
        bezierCurve = new BezierCurve();
    }
}