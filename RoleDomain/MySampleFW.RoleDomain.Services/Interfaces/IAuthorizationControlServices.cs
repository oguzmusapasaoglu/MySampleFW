namespace MySampleFW.RoleDomain.Services.Interfaces;

public interface IAuthorizationControlServices
{
    bool AuthorizationControlByUser(int userID, string servicesName);
}
