using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IdeaMachine.Common.AspNetIdentity.Extension
{
	// ReSharper disable once InconsistentNaming
	public static class IServiceCollectionExtensions
	{
		public static IdentityBuilder AddIdentityWithoutDefaultAuthSchemes<TUser, TRole>(
			this IServiceCollection services,
			Action<IdentityOptions>? setupAction = null)
			where TUser : class
			where TRole : class
		{
			// Hosting doesn't add IHttpContextAccessor by default
			services.AddHttpContextAccessor();
			// Identity services
			services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
			services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
			services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
			services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
			services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
			// No interface for the error describer so we can add errors without rev'ing the interface
			services.TryAddScoped<IdentityErrorDescriber>();
			services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
			services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<TUser>>();
			services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
			services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
			services.TryAddScoped<UserManager<TUser>>();
			services.TryAddScoped<SignInManager<TUser>>();
			services.TryAddScoped<RoleManager<TRole>>();

			if (setupAction is not null)
			{
				services.Configure(setupAction);
			}

			return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
		}
	}
}