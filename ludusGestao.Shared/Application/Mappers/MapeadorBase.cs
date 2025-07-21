namespace LudusGestao.Shared.Application.Mappers
{
    public abstract class MapeadorBase<TCreate, TUpdate, TEntity, TResponse>
    {
        public abstract TEntity Mapear(TCreate dto);
        public abstract void Mapear(TUpdate dto, TEntity entidade);
        public abstract TResponse Mapear(TEntity entidade);
    }
} 