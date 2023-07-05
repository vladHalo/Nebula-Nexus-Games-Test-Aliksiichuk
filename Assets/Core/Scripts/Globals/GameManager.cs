using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public BezierCurve bezierCurve;

    protected override void Awake()
    {
        base.Awake();
        bezierCurve = new BezierCurve();
    }
}