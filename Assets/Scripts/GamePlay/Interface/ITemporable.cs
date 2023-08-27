public interface ITemporable
{
    public float LifeTime { get; set; }

    public void AddLifetime(float time);
    
    public void RemoveLifetime(float time);

    public void Dead();
}