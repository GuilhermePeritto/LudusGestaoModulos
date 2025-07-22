using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Shared.Application.Events
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _handlers = new();

        public Task Publicar<T>(T evento) where T : class
        {
            var tipo = typeof(T);
            if (_handlers.TryGetValue(tipo, out var handlers))
            {
                var tasks = new List<Task>();
                foreach (var handler in handlers)
                {
                    tasks.Add(handler(evento));
                }
                return Task.WhenAll(tasks);
            }
            return Task.CompletedTask;
        }

        public void RegistrarHandler<T>(Func<T, Task> handler) where T : class
        {
            var tipo = typeof(T);
            var wrapper = new Func<object, Task>(e => handler((T)e));
            _handlers.AddOrUpdate(tipo,
                _ => new List<Func<object, Task>> { wrapper },
                (_, list) => { list.Add(wrapper); return list; });
        }
    }
} 