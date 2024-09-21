using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.DbModels.Auth0;

public record UserAuth0(string UserId, string Email, string Password)
{
    public string? UserAuth0Id { get; set; }
};