namespace ludusGestao.Eventos.Domain.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T valor);
        string ErrorMessage { get; }
    }
} 