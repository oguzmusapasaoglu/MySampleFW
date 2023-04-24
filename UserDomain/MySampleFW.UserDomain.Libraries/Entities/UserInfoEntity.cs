using MyCore.Dapper.Attributes;
using MyCore.Dapper.Base;
namespace MySampleFW.UserDomain.Libraries.Entities;

[DapperTable("UserInfo")]
public class UserInfoEntity : ExtendBaseDapperEntity
{
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string Password { get; set; }
    public string GSM { get; set; }
    public int UserGroup { get; set; }
    public string UserType { get; set; }
    public bool Gender { get; set; }
    public DateTime? BirthDay { get; set; }
}
