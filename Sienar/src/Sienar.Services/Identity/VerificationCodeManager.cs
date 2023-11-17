using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sienar.Identity;

public class VerificationCodeManager<TUser, TContext>
	: IVerificationCodeManager<TUser>
	where TUser : SienarUser
	where TContext : DbContext
{
	protected readonly TContext Context;
	protected DbSet<TUser> UserSet => Context.Set<TUser>();

	public VerificationCodeManager(TContext context)
	{
		Context = context;
	}

	/// <inheritdoc/>
	public virtual async Task<VerificationCode> CreateCode(
		TUser user,
		string type)
	{
		await UserSet
			.Entry(user)
			.Collection(u => u.VerificationCodes)
			.LoadAsync();

		await DeleteCode(user, type);

		var code = new VerificationCode
		{
			Code = Guid.NewGuid(),
			Type = type,
			ExpiresAt = GetVerificationCodeExpiration(type)
		};

		user.VerificationCodes.Add(code);
		UserSet.Update(user);
		await Context.SaveChangesAsync();

		return code;
	}

	/// <inheritdoc/>
	public virtual Task DeleteCode(
		TUser user,
		string type)
		=> DeleteVerificationCode(user, type, true);

	protected virtual async Task DeleteVerificationCode(
		TUser user,
		string type,
		bool saveChanges)
	{
		await UserSet
			.Entry(user)
			.Collection(u => u.VerificationCodes)
			.LoadAsync();

		var code = user.VerificationCodes.FirstOrDefault(c => c.Type == type);
		if (code is null)
		{
			return;
		}

		user.VerificationCodes.Remove(code);
		Context.Remove(code);

		if (saveChanges)
		{
			await Context.SaveChangesAsync();
		}
	}

	/// <inheritdoc/>
	public virtual async Task<VerificationCode?> GetCode(
		TUser user,
		string type)
	{
		await UserSet
			.Entry(user)
			.Collection(u => u.VerificationCodes)
			.LoadAsync();

		return user.VerificationCodes.FirstOrDefault(v => v.Type == type);
	}

	public virtual VerificationCodeStatus GetCodeStatus(
		VerificationCode? code,
		Guid suppliedCode)
	{
		if (code is null 
		|| code.Code != suppliedCode)
		{
			return VerificationCodeStatus.Invalid;
		}

		if (code.ExpiresAt < DateTime.Now)
		{
			return VerificationCodeStatus.Expired;
		}

		return VerificationCodeStatus.Valid;
	}

	public virtual async Task<VerificationCodeStatus> GetCodeStatus(
		TUser user,
		string type,
		Guid suppliedCode)
	{
		var code = await GetCode(user, type);
		return GetCodeStatus(code, suppliedCode);
	}

	public virtual async Task<VerificationCodeStatus> VerifyCode(
		TUser user,
		string type,
		Guid suppliedCode,
		bool deleteIfValid)
	{
		var code = await GetCode(user, type);
		var status = GetCodeStatus(code, suppliedCode);
		if (status == VerificationCodeStatus.Valid && deleteIfValid)
		{
			await DeleteCode(user, type);
		}

		return status;
	}

	protected virtual DateTime GetVerificationCodeExpiration(string type)
	{
		return DateTime.Now.AddMinutes(30);
	}
}