using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Common.Models;

namespace LMS.Application.Auth.RefreshToken;

public class RefreshTokenRequest : IRequest<ResponseBase>
{
    public string RefreshToken { get; set; } = default!;
}
