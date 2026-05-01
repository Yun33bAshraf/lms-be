using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Common.Models;

namespace LMS.Application.Auth.ForgotPassword;

public class ForgotPasswordRequest : IRequest<ResponseBase>
{
    public string Email { get; set; } = default!;
}
