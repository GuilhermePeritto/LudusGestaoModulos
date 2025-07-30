using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LudusGestao.Shared.Domain.QueryParams.Enums;
using LudusGestao.Shared.Domain.QueryParams.Exceptions;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Implementations
{
    /// <summary>
    /// Implementação do conversor de filtros
    /// </summary>
    public class ConversorFiltro : IConversorFiltro
    {
        private readonly IValidadorFiltro _validador;

        public ConversorFiltro(IValidadorFiltro validador)
        {
            _validador = validador;
        }

        /// <summary>
        /// Converte QueryFilters para CriterioFiltro
        /// </summary>
        public List<CriterioFiltro> ConverterParaCriterios(List<QueryFilter> queryFilters, Type tipoEntidade)
        {
            if (queryFilters == null || !queryFilters.Any())
                return new List<CriterioFiltro>();

            var criterios = new List<CriterioFiltro>();

            foreach (var queryFilter in queryFilters)
            {
                if (!string.IsNullOrWhiteSpace(queryFilter.Property))
                {
                    var criterio = ConverterParaCriterio(queryFilter, tipoEntidade);
                    criterios.Add(criterio);
                }
            }

            return criterios;
        }

        /// <summary>
        /// Converte um QueryFilter para CriterioFiltro
        /// </summary>
        public CriterioFiltro ConverterParaCriterio(QueryFilter queryFilter, Type tipoEntidade)
        {
            if (queryFilter == null)
                throw new QueryFilterException("QueryFilter não pode ser nulo", "QUERY_FILTER_NULL");

            if (string.IsNullOrWhiteSpace(queryFilter.Property))
                throw new QueryFilterException("Propriedade do filtro não pode ser vazia", "PROPERTY_EMPTY");

            // Verifica se é uma propriedade aninhada (ex: Email.Endereco)
            if (queryFilter.Property.Contains('.'))
            {
                return ConverterParaCriterioAninhado(queryFilter, tipoEntidade);
            }

            // Obtém a propriedade da entidade
            var propriedade = tipoEntidade.GetProperty(queryFilter.Property, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propriedade == null)
                throw new QueryFilterException(
                    $"Propriedade '{queryFilter.Property}' não encontrada no tipo '{tipoEntidade.Name}'", 
                    "PROPERTY_NOT_FOUND", 
                    queryFilter.Property, 
                    null);

            // Converte o valor para o tipo da propriedade
            var valorConvertido = ConverterValor(queryFilter.Value, propriedade.PropertyType);

            return new CriterioFiltro(
                propriedade.Name, // Usa o nome exato da propriedade
                queryFilter.Operator ?? "eq",
                valorConvertido,
                propriedade.PropertyType
            );
        }

        /// <summary>
        /// Converte filtro com propriedade aninhada (ex: Email.Endereco)
        /// </summary>
        private CriterioFiltro ConverterParaCriterioAninhado(QueryFilter queryFilter, Type tipoEntidade)
        {
            var partes = queryFilter.Property.Split('.');
            if (partes.Length != 2)
                throw new QueryFilterException(
                    $"Propriedade aninhada '{queryFilter.Property}' deve ter exatamente 2 partes (ex: Email.Endereco)", 
                    "INVALID_NESTED_PROPERTY", 
                    queryFilter.Property, 
                    null);

            var propriedadePrincipal = partes[0];
            var propriedadeSecundaria = partes[1];

            // Obtém a propriedade principal
            var propPrincipal = tipoEntidade.GetProperty(propriedadePrincipal, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propPrincipal == null)
                throw new QueryFilterException(
                    $"Propriedade '{propriedadePrincipal}' não encontrada no tipo '{tipoEntidade.Name}'", 
                    "PROPERTY_NOT_FOUND", 
                    propriedadePrincipal, 
                    null);

            // Obtém a propriedade secundária do tipo da propriedade principal
            var tipoSecundario = propPrincipal.PropertyType;
            var propSecundaria = tipoSecundario.GetProperty(propriedadeSecundaria, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propSecundaria == null)
                throw new QueryFilterException(
                    $"Propriedade '{propriedadeSecundaria}' não encontrada no tipo '{tipoSecundario.Name}'", 
                    "PROPERTY_NOT_FOUND", 
                    propriedadeSecundaria, 
                    null);

            // Converte o valor para o tipo da propriedade secundária
            var valorConvertido = ConverterValor(queryFilter.Value, propSecundaria.PropertyType);

            return new CriterioFiltro(
                queryFilter.Property, // Mantém o caminho completo para o ProcessadorFiltro
                queryFilter.Operator ?? "eq",
                valorConvertido,
                propSecundaria.PropertyType
            );
        }

        /// <summary>
        /// Converte valor para o tipo da propriedade
        /// </summary>
        public object? ConverterValor(object? valor, Type tipoDestino)
        {
            if (valor == null)
                return null;

            try
            {
                // Se o tipo já é compatível, retorna o valor
                if (tipoDestino.IsAssignableFrom(valor.GetType()))
                    return valor;

                // Se o valor é string e o tipo de destino é Guid
                if (valor is string strValor && tipoDestino == typeof(Guid))
                {
                    if (Guid.TryParse(strValor, out var guid))
                        return guid;
                    throw new QueryFilterException($"Valor '{strValor}' não é um Guid válido", "INVALID_GUID", null, valor);
                }

                // Se o valor é string e o tipo de destino é DateTime
                if (valor is string strData && tipoDestino == typeof(DateTime))
                {
                    if (DateTime.TryParse(strData, out var data))
                        return data;
                    throw new QueryFilterException($"Valor '{strData}' não é uma data válida", "INVALID_DATE", null, valor);
                }

                // Se o valor é string e o tipo de destino é bool
                if (valor is string strBool && tipoDestino == typeof(bool))
                {
                    if (bool.TryParse(strBool, out var boolValor))
                        return boolValor;
                    throw new QueryFilterException($"Valor '{strBool}' não é um boolean válido", "INVALID_BOOL", null, valor);
                }

                // Se o valor é string e o tipo de destino é numérico
                if (valor is string strNumero && tipoDestino.IsPrimitive && tipoDestino != typeof(bool))
                {
                    return Convert.ChangeType(strNumero, tipoDestino);
                }

                // Conversão genérica
                return Convert.ChangeType(valor, tipoDestino);
            }
            catch (Exception ex) when (ex is not QueryFilterException)
            {
                throw new QueryFilterException(
                    $"Erro ao converter valor '{valor}' para tipo '{tipoDestino.Name}'", 
                    "CONVERSION_ERROR", 
                    null, 
                    valor, 
                    ex);
            }
        }

        /// <summary>
        /// Separa filtros entre banco de dados e memória
        /// </summary>
        public (List<CriterioFiltro> FiltrosBanco, List<CriterioFiltro> FiltrosMemoria) SepararFiltros(List<CriterioFiltro> criterios)
        {
            var filtrosBanco = new List<CriterioFiltro>();
            var filtrosMemoria = new List<CriterioFiltro>();

            if (criterios == null || !criterios.Any())
                return (filtrosBanco, filtrosMemoria);

            foreach (var criterio in criterios)
            {
                if (criterio.PodeAplicarNoBanco)
                {
                    filtrosBanco.Add(criterio);
                }
                else if (criterio.PrecisaAplicarEmMemoria)
                {
                    filtrosMemoria.Add(criterio);
                }
                // Se não for nenhum dos dois, ignora o filtro
            }

            return (filtrosBanco, filtrosMemoria);
        }
    }
} 