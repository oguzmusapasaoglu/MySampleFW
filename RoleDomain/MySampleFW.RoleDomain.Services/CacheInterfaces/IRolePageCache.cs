﻿using MyCore.Cache.Interfaces;
using MySampleFW.RoleDomain.Libraries.Models;

namespace MySampleFW.RoleDomain.Services.CacheInterfaces;

public interface IRolePageCache : ICacheManager<RolePageListModel>
{
}
