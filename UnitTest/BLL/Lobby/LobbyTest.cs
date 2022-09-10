using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Validation;
using GeneralLibrary.Enums;
using Xunit;

namespace UnitTest.BLL.Lobby;

public class LobbyTest
{
    [Fact]
    public void LobbyUserShortInfoValidation()
    {
        var list = new List<LobbyUserShortInfo>()
        {
            new() { Id = null, Name = "inar" },
            new() { Id = null, Name = "nochun" },
            new() { Id = Guid.Empty, Name = "Ella" },
            new() { Id = Guid.Empty, Name = "Ella2" }
        };
        var rez = list.Distinct(new LobbyUserShortInfoEqualityComparer()).ToList();
        var count = rez.Count();
        Assert.Equal(3, count);
    }

    [Fact]
    public void LobbyItemMValidationTest()
    {
        var lobby = new LobbyItemM()
        {
            SideA =
            {
                new() { Id = null, Name = "inar", Accepted = IsAccepted.Accepted },
                new() { Id = null, Name = "nochun", Accepted = IsAccepted.Accepted },
            },
            SideB =
            {
                new() { Id = Guid.Empty, Name = "Ella", Accepted = IsAccepted.Accepted },
                new() { Id = Guid.Empty, Name = "Ella2", Accepted = IsAccepted.Accepted }
            }
        };

        var results = new List<ValidationResult>();
        var context = new ValidationContext(lobby);

        var rez = Validator.TryValidateObject(lobby, context, results);

        Assert.False(rez);
    }
}