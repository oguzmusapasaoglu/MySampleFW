using MySampleFW.RoleDomain.Services.CacheInterfaces;

using MyCore.Cache.Interfaces;
using MySampleFW.RoleDomain.Libraries.Models;

using System.Linq.Expressions;
using MySampleFW.Helper.Maps;
using MySampleFW.RoleDomain.Repositores.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;

namespace MySampleFW.RoleDomain.Services.Cache;

public class RolePageObjectCache : IRolePageObjectCache
{
    #region Fields
    public string CacheKey => cache.GetCacheKey(BaseName);
    public string BaseName => "RolePageObject";
    IRolePageObjectRepository repository;
    IMemCacheServices cache;
    #endregion

    #region Ctor
    public RolePageObjectCache(IMemCacheServices _cache, IRolePageObjectRepository _repository)
    {
        cache = _cache;
        repository = _repository;
    }
    #endregion

    #region Methods
    public void AddBulkData(IQueryable<RolePageObjectModel> data)
    {
        var result = cache.GetCachedData<RolePageObjectModel>(CacheKey).ToList();
        result.AddRange(data);
        cache.FillCache(CacheKey, result);
    }
    public void AddSingleData(RolePageObjectModel data)
    {
        var result = GetAllData();
        result.ToList().Add(data);
        cache.FillCache(CacheKey, result);
    }
    public IQueryable<RolePageObjectModel> FillData()
    {
        var data = repository.GetAllActive().Result.AsEnumerable();
        var model = MapperInstance.Instance.Map<IEnumerable<RolePageObjectEntity>, IEnumerable<RolePageObjectModel>>(data);
        cache.FillCache(CacheKey, model);
        return model.AsQueryable();
    }
    public IQueryable<RolePageObjectModel> GetAllData()
    {
        var result = cache.GetCachedData<RolePageObjectModel>(CacheKey);
        if (result == null)
            result = FillData();
        return result;
    }
    public IQueryable<RolePageObjectModel> GetDataByFilter(Expression<Func<RolePageObjectModel, bool>> predicate)
    {
        var result = cache.GetCachedData<RolePageObjectModel>(CacheKey);
        if (result.IsNullOrEmpty())
            result = FillData();
        return result.Where(predicate);
    }
    public RolePageObjectModel GetSingleDataByFilter(Expression<Func<RolePageObjectModel, bool>> predicate)
    {
        var result = cache.GetCachedData<RolePageObjectModel>(CacheKey);
        if (result.IsNullOrEmpty())
            result = FillData();
        return result.FirstOrDefault(predicate);
    }
    public IQueryable<RolePageObjectModel> GetDataByRoleId(int roleId)
    {
        var result = cache.GetCachedData<RolePageObjectModel>(CacheKey);
        if (result.IsNullOrEmpty())
            result = FillData();
        return result.Where(q => q.RoleID == roleId);
    }
    public RolePageObjectModel GetSingleDataById(int id)
    {
        var result = cache.GetCachedData<RolePageObjectModel>(CacheKey);
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
