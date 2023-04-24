using MyCore.Cache.Interfaces;
using MySampleFW.UserDomain.Libraries.Models;

namespace MySampleFW.UserDomain.Repositories.CacheInterfaces;
public interface IUserInfoCache : ICacheManager<UserInfoModel>
{
}