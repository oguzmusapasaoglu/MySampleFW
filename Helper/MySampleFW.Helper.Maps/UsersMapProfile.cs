using AutoMapper;

using MySampleFW.UserDomain.Libraries.Entities;
using MySampleFW.UserDomain.Libraries.Helper;
using MySampleFW.UserDomain.Libraries.Models;

namespace MySampleFW.Helper.Maps;

public class UsersMapProfile : Profile
{
    public UsersMapProfile()
    {
        #region User Info
        CreateMap<UserInfoModel, UserInfoEntity>()
           .ForMember(d => d.ActivationStatus, o => o.MapFrom(s => Enum.GetName(typeof(ActivationStatusEnum), s.ActivationStatusName)))
           .ForMember(d => d.ActivationStatus, o => o.MapFrom(s => s.ActivationStatusData))
           .ReverseMap();

        CreateMap<UserInfoCreateModel, UserInfoEntity>();
        CreateMap<UserInfoUpdateModel, UserInfoEntity>();

        CreateMap<UserInfoListModel, UserInfoEntity>()
            .ForMember(d => d.ActivationStatus, o => o.MapFrom(s => Enum.GetName(typeof(ActivationStatusEnum), s.ActivationStatusName)));
        //.ForMember(d => d.Gender, o => o.MapFrom(s => Enum.GetName(typeof(GenderEnum), s.GenderName)));

        CreateMap<UserInfoModel, UserInfoListModel>()
            .ForMember(d => d.ActivationStatus, o => o.MapFrom(s => Enum.GetName(typeof(ActivationStatusEnum), s.ActivationStatusName)))
            .ForMember(d => d.UserGroupName, o => o.MapFrom(s =>Enum.GetName(typeof(UserGroupEnum), s.UserGroupName)))
            .ForMember(d => d.GenderName, o => o.MapFrom(s => Enum.GetName(typeof(GenderEnum), s.Gender)));
        #endregion

        #region UsersRoles
        CreateMap<UsersRolesModel, UsersRolesEntity>()
          .ForMember(d => d.ActivationStatus, o => o.MapFrom(s => Enum.GetName(typeof(ActivationStatusEnum), s.ActivationStatusName)))
          .ForMember(d => d.ActivationStatus, o => o.MapFrom(s => s.ActivationStatusData));

        CreateMap<UsersRolesEntity, UsersRolesModel>()
            .ForMember(q => q.ActivationStatusName, o => o.MapFrom(s => Enum.GetName(typeof(ActivationStatusEnum), s.ActivationStatus)));

        CreateMap<UsersRolesCreateOrUpdateModel, UsersRolesEntity>();

        CreateMap<UsersRolesBulkCreateModel, UsersRolesEntity>()
            .ForMember(q => q.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
            .ForMember(q => q.CreatedDate, o => o.MapFrom(s => DateTime.Now));
        #endregion
    }
}
