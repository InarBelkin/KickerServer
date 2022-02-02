using BLLAdapter.Models;
using BLLAdapter.Models.Auth;

namespace BLLAdapter.Interfaces;

public interface IAuthRepos
{
    public Task AddNewUser(UserAuthM user);
}