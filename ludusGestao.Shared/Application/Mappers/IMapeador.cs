namespace LudusGestao.Shared.Application.Mappers
{
    public interface IMapeador<TCreate, TUpdate, TEntity, TResponse>
    {
        TEntity Mapear(TCreate dto);
        void Mapear(TUpdate dto, TEntity entidade);
        TResponse Mapear(TEntity entidade);
    }
} 