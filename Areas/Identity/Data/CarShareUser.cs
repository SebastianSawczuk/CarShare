using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CarShare.Models;
using Microsoft.AspNetCore.Identity;

namespace CarShare.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CarShareUser class
public class CarShareUser : IdentityUser
{

    public Client? Client { get; set; }
}

