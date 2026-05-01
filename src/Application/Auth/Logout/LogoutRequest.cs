using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Common.Models;

namespace LMS.Application.Auth.Logout;

public class LogoutRequest : IRequest<ResponseBase>
{
    public string RefreshToken { get; set; } = default!;
    public bool LogoutAllDevices { get; set; } = false;
}
