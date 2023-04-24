using MySampleFW.RoleDomain.Services.CacheInterfaces;
using MySampleFW.RoleDomain.Services.Interfaces;

namespace MySampleFW.RoleDomain.Services.ServicesManager;

public class AuthorizationControlServices : IAuthorizationControlServices
{
    private IAuthorizationControlCache cache;

    public AuthorizationControlServices(IAuthorizationControlCache _cache)
    {
        cache = _cache;
    }
    public bool AuthorizationControlByUser(int userID, string servicesName)
    {
        var cachedData = cache.GetAllData();
        if (cachedData != null)
            return cachedData.Any(q => q.UserID == userID && q.ServicesName == servicesName);
        return false;
    }
}
