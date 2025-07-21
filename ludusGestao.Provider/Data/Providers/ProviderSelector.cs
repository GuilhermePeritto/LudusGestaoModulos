using System;
using Microsoft.Extensions.Configuration;
using ludusGestao.Eventos.Domain.Providers;
using ludusGestao.Provider.Data.Providers.Eventos.Local;

namespace ludusGestao.Provider.Data.Providers
{
    public class ProviderSelector
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public ProviderSelector(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public ILocalReadProvider GetLocalReadProvider()
        {
            return (ILocalReadProvider)_serviceProvider.GetService(typeof(LocalPostgresReadProvider))!;
        }

        public ILocalWriteProvider GetLocalWriteProvider()
        {
            return (ILocalWriteProvider)_serviceProvider.GetService(typeof(LocalPostgresWriteProvider))!;
        }
    }
} 