using MyCore.Common.Base;
namespace MySampleFW.UserDomain.Libraries.Models;
public class UsersRolesModel : ExtendBaseModel
{
    public int UserID { get; set; }
    public int RoleID { get; set; }
    public string UserName { get; set; }
    public string RoleName { get; set; }
    public string ActivationStatusName { get; set; }
}
public class UsersRolesCreateOrUpdateModel : BaseModel
{
    public int UserID { get; set; }
    public int RoleID { get; set; }
}
public class UsersRolesBulkCreateModel : BaseModel
{
    public int UserID { get; set; }
    public int RoleID { get; set; }
    public int CreatedBy { get; set; }
}
public class UsersRolesFilterModel
{
    public int? ID { get; set; }
    public int? ActivationStatus { get; set; }
    public int? UserID { get; set; }
    public int? RoleID { get; set; }
}