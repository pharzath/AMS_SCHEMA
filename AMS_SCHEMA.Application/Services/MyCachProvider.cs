using AMS.Model.Services;
using QOQNOS.Core;
using Olive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMS_SCHEMA.Application.Services;

public class MyCachProvider : MemoryCache
{

    readonly List<CachKey> _keys = new();
    public CachKey CreateCachKey<T>(string name, int elementId) where T : IHaveId<int>
    {
        var key = new CachKey(name)
        {
            Type = typeof(T),
            Id = elementId,
        };
        key = GetUniqCachKey<T>(key);
        return key;
    }

    public CachKey CreateCachKey<T>(string name, T? element) where T : IHaveId<int>
    {
        var key = new CachKey(name)
        {
            Type = typeof(T),
            Id = element?.Id,
        };
        key = GetUniqCachKey<T>(key);
        return key;
    }

    CachKey GetUniqCachKey<T>(CachKey key) where T : IHaveId<int>
    {
        var any = _keys
            .FirstOrDefault(x => x.Name == key.Name && x.Type == key.Type && x.Id == key.Id);
        if (any != null)
            key = any;
        else
            _keys.Add(key);
        return key;
    }

    public CachKey CreateCachKey<T>(string name, params Type[]? relatedTo) where T : IHaveId<int>
    {
        var key = new CachKey(name)
        {
            Type = typeof(T),
            RelatedTypes = relatedTo ?? Array.Empty<Type>()
        };
        key = GetUniqCachKey<T>(key);
        return key;
    }

    public CachKey CreateCachKey<T>(string name, T? element, params Type[] relatedTo) where T : IHaveId<int>
    {
        var key = new CachKey(name)
        {
            Type = typeof(T),
            Id = element?.Id,
            RelatedTypes = relatedTo
        };
        key = GetUniqCachKey<T>(key);
        return key;
    }

    public CachKey CreateCachKey<T>(string name, T? element, params CachKey[] relatedKeys) where T : IHaveId<int>
    {
        var key = new CachKey(name)
        {
            Type = typeof(T),
            Id = element?.Id,
            RelatedKeys = relatedKeys,
        };
        key = GetUniqCachKey<T>(key);
        return key;
    }

    public void ExpireCachKeys<T>(T _) where T : IHaveId<int>
    {
        Clear();
        return;
        // var cachKeys = _keys.Where(x =>
        //                                                     (x.Type == typeof(T) && x.Id == _.Id) ||
        //                                                     (x.RelatedKeys != null && x.RelatedKeys.Any(z => z.Type == typeof(T) && z.Id == _.Id)) ||
        //                                                     (x.RelatedTypes != null && x.RelatedTypes.Contains(typeof(T)))
        // )
        //     .ToList();
        var cachKeys = _keys.Where(x =>   x.Type == typeof(T) ||
                                                            (x.RelatedKeys != null && x.RelatedKeys.Any(z => z.Type == typeof(T))) ||
                                                            (x.RelatedTypes != null && x.RelatedTypes.Contains(typeof(T)))
        )
            .ToList();
        foreach (var cachKey in cachKeys)
            Remove(cachKey);
    }

    public MyCachProvider(IOptions<MemoryCacheOptions> optionsAccessor) : base(optionsAccessor)
    {
    }

    public MyCachProvider(IOptions<MemoryCacheOptions> optionsAccessor, ILoggerFactory loggerFactory) : base(optionsAccessor, loggerFactory)
    {
    }

    public void Clear()
    {
        foreach (var cachKey in _keys) 
            Remove(cachKey);
        _keys.Clear();
    }
}