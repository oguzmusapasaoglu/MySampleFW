using MySampleFW.RoleDomain.Libraries.Models;

namespace MySampleFW.UserDomain.Libraries.Models;

public class UserLoginRequestModel
{
    public string LoginName { get; set; }
    public string Password { get; set; }
}
public class UserLoginResponseModel
{
    public int ID { get; set; }
    public string UserName { get; set; }
    public string EMail { get; set; }
    public string UserToken { get; set; }
    public int UserGroupID { get; set; }
    public string UserGroupName { get; set; }
    public string UserType { get; set; }
    public string GSM { get; set; }
    public ICollection<LeftMainMenuModel> LeftMenuList { get; set; }
}
