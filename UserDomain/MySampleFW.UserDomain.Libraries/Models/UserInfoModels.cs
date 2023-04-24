using MySampleFW.UserDomain.Libraries.Helper;

using MyCore.Common.Base;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace MySampleFW.UserDomain.Libraries.Models;

public class UserInfoModel : ExtendBaseModel
{
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string Password { get; set; }
    public string GSM { get; set; }
    public int UserGroup { get; set; }
    public int UserGroupName { get; set; }
    public string UserType { get; set; }
    public bool Gender { get; set; }
    public DateTime BirthDay { get; set; }
    public string ActivationStatusName { get; set; }

    [NotMapped]
    public UserGroupEnum UserGroupData { get; set; }
}
public class UserInfoCreateModel
{
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string EMail { get; set; }
    public string GSM { get; set; }
    public int UserGroup { get; set; }
    public string UserType { get; set; }
    public bool? Gender { get; set; }
    public DateTime? BirthDay { get; set; }
}
public class UserInfoUpdateModel
{
    public int ID { get; set; }
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string GSM { get; set; }
    public int UserGroup { get; set; }
    public string UserType { get; set; }
    public bool Gender { get; set; }
    public DateTime BirthDay { get; set; }
}
public class UserInfoListModel : BaseModel
{
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string GSM { get; set; }
    public string UserGroupName { get; set; }
    public string UserType { get; set; }
    public DateTime BirthDay { get; set; }
    public string GenderName { get; set; }
    public string ActivationStatusName { get; set; }
}
public class UserInfoFilterModel
{
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string GSM { get; set; }
    public int? UserGroup { get; set; }
    public string UserType { get; set; }
}
public class UserInfoChangePasswordModel
{
    public int Id { get; set; }
    public string NewPassword { get; set; }
}
public class UserInfoStatusChangeModel
{
    public int ID { get; set; }
    public int ActivationStatus { get; set; }

    [JsonIgnore]
    [NotMapped]
    public ActivationStatusEnum ActivationStatusValue { get; set; }
}