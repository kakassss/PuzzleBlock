using Zenject;

public interface IBaseCommand
{
    void SetObjectResolver(DiContainer diContainer);
    void ResolveDependencies();
}