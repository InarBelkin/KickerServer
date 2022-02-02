using System.ComponentModel.DataAnnotations;
using BLLAdapter.Interfaces;
using BLLAdapter.Models.Auth;
using BLLAdapter.Repositories.Base;
using DAL;
using DAL.Entities;
using DAL.Entities.Auth;

namespace BLLAdapter.Repositories;

public class AuthRepos : EfReposBase, IAuthRepos
{
    public AuthRepos(KickerContext db) : base(db)
    {
    }

    public async Task AddNewUser(UserAuthM user)
    {
        if (user.AuthInfos.Count == 0)
            throw new ValidationException("User must have at least one AuthInfo");
        switch (user.AuthInfos.FirstOrDefault())
        {
            case AuthInfoFirebaseM fb:
                _db.Users.Add(new User()
                {
                    Name = user.Name,
                    AuthInfos = new[] {new AuthInfoFirebase() {FirebaseUuid = fb.FirebaseUuid}},
                    StatsOneVsOne = new StatsOneVsOne(), StatsTwoVsTwo = new StatsTwoVsTwo()
                });
                await _db.SaveChangesAsync(); //TODO: FirebaseUUid must be unique
                break;
            case AuthInfoMailM m:
                _db.Users.Add(new User()
                {
                    Name = user.Name,
                    AuthInfos = new[] {new AuthInfoMail() {Email = m.Email, HashPassword = m.HashPassword}},
                    StatsOneVsOne = new StatsOneVsOne(), StatsTwoVsTwo = new StatsTwoVsTwo()
                });
                await _db.SaveChangesAsync(); //TODO: Email must be unique
                break;
        }
    }
}