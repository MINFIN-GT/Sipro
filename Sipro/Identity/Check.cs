using System;
using System.Security.Principal;
using System.Security.Claims;

namespace Identity
{
	public static class Check
	{

		public static bool Permission(IIdentity user, String permission){
			return ((ClaimsIdentity)user).HasClaim("sipro/permission", permission);
		}
	}
}
