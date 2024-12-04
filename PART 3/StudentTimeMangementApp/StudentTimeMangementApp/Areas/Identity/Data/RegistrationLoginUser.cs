using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StudentTimeMangementApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the RegistrationLoginUser class
public class RegistrationLoginUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Firstname { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string surname { get; set; }

}

