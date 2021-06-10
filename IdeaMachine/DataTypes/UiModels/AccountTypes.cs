using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaMachine.DataTypes.UiModels
{
	public record SignInInfo(string Email, string Password, bool? RememberMe = false);
}