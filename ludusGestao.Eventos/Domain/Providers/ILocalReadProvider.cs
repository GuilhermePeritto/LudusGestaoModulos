using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities;

namespace ludusGestao.Eventos.Domain.Providers
{
    public interface ILocalReadProvider
    {
        Task<Local?> BuscarPorId(Guid id);
        Task<IEnumerable<Local>> ListarTodos();
        Task<IEnumerable<Local>> BuscarPorCapacidade(int capacidadeMinima);
        Task<IEnumerable<Local>> BuscarPorFaixaPreco(decimal valorMinimo, decimal valorMaximo);
        Task<IEnumerable<Local>> BuscarPorDisponibilidade(bool disponivel);
        Task<bool> ExistePorNome(string nome, Guid? idExcluir = null);
        Task<int> Contar();
        Task<IEnumerable<Local>> BuscarPaginado(int pagina, int tamanhoPagina);
    }
} 