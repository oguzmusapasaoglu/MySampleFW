using MySampleFW.RoleDomain.Services.CacheInterfaces;

using MyCore.Cache.Interfaces;
using MySampleFW.RoleDomain.Libraries.Models;

using System.Linq.Expressions;
using MySampleFW.Helper.Maps;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.RoleDomain.Repositores.Interfaces;

namespace MySampleFW.RoleDomain.Services.Cache;
public class PagesCache : IPagesCache
{
    #region Fields
    public string CacheKey => cache.GetCacheKey(BaseName);
    public string BaseName => "Pages";
    IPagesRepository repository;
    IMemCacheServices cache;
    #endregion

    #region Ctor
    public PagesCache(IMemCacheServices _cache, IPagesRepository _repository)
    {
        cache = _cache;
        repository = _repository;
    }
    #endregion

    #region Methods
    public void AddBulkData(IQueryable<PagesModel> data)
    {
        var result = cache.GetCachedData<PagesModel>(CacheKey).ToList();
        result.AddRange(data);
        cache.FillCache(CacheKey, result);
    }
    public void AddSingleData(PagesModel data)
    {
        var result = GetAllData();
        result.ToList().Add(data);
        cache.FillCache(CacheKey, result);
    }
    public IQueryable<PagesModel> FillData()
    {
        var data = repository.GetAllActive().ConfigureAwait(false).GetAwaiter().GetResult();
        var model = MapperInstance.Instance.Map<IEnumerable<PagesEntity>, IEnumerable<PagesModel>>(data);
        cache.FillCache(CacheKey, model);
        return model.AsQueryable();
    }
    public IQueryable<PagesModel> GetAllData()
    {
        var result = cache.GetCachedData<PagesModel>(CacheKey);
        if (result == null)
            result = FillData();
        return result;
    }
    public IQueryable<PagesModel> GetDataByFilter(Expression<Func<PagesModel, bool>> predicate)
    {
        var result = cache.GetCachedData<PagesModel>(CacheKey);
        if (result.IsNullOrEmpty())
            result = FillData();
        return result.Where(predicate);
    }
    public PagesModel GetSingleDataByFilter(Expression<Func<PagesModel, bool>> predicate)
    {
        var result = cache.GetCachedData<PagesModel>(CacheKey);
        if (result.IsNullOrEmpty())
            result = FillData();
        return result.FirstOrDefault(predicate);
    }
    public PagesModel GetSingleDataById(int id)
    {
        var result = cache.GetCachedData<PagesModel>(CacheKey);
        if (result.IsNullOrEmpty())
            result = FillData();
        return result.FirstOrDefault(q => q.ID == id);
    }
    public void ReFillCache()
    {
        FillData();
    }
    #endregion
}
