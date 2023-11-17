namespace Sienar.Constants;

public static class Urls
{
	public const string Home = "/Index";
	public const string Privacy = "/Privacy";

	public static class Account
	{
		private const string AccountPrefix = "/Account";
		public const string Login = $"{AccountPrefix}/Login";
		public const string Logout = $"{AccountPrefix}/Logout";
		public const string Forbidden = $"{AccountPrefix}/Forbidden";
		public const string Deleted = $"{AccountPrefix}/Deleted";

		public static class Register
		{
			private const string RegisterPrefix = $"{AccountPrefix}/Register";
			public const string RegisterIndex = $"{RegisterPrefix}/Index";
			public const string Successful = $"{RegisterPrefix}/Successful";
		}

		public static class Confirm
		{
			private const string ConfirmPrefix = $"{AccountPrefix}/Confirm";
			public const string ConfirmIndex = ConfirmPrefix;
			public const string Successful = $"{ConfirmPrefix}/Successful";
		}

		public static class ForgotPassword
		{
			private const string ForgotPasswordPrefix = $"{AccountPrefix}/ForgotPassword";
			public const string ForgotPasswordIndex = $"{ForgotPasswordPrefix}/Index";
			public const string Successful = $"{ForgotPasswordPrefix}/Successful";
		}

		public static class ResetPassword
		{
			private const string ResetPasswordPrefix = $"{AccountPrefix}/ResetPassword";
			public const string ResetPasswordIndex = $"{ResetPasswordPrefix}";
			public const string Successful = $"{ResetPasswordPrefix}/Successful";
		}
	}

	public static class Dashboard
	{
		private const string DashboardPrefix = "/Dashboard";
		public const string DashboardIndex = DashboardPrefix;
		public const string Plugins = $"{DashboardPrefix}/Plugins";

		public static class Account
		{
			private const string AccountPrefix = $"{DashboardPrefix}/Account";
			public const string PersonalData = $"{AccountPrefix}/PersonalData";

			public static class EmailChange
			{
				private const string EmailChangePrefix = $"{AccountPrefix}/Email";
				public const string EmailChangeIndex = $"{EmailChangePrefix}/Index";
				public const string Requested = $"{EmailChangePrefix}/Requested";
				public const string Confirm = $"{EmailChangePrefix}/Confirm";
				public const string Successful = $"{EmailChangePrefix}/Successful";
			}

			public static class PasswordChange
			{
				private const string PasswordChangePrefix = $"{AccountPrefix}/Password";
				public const string PasswordChangeIndex = $"{PasswordChangePrefix}/Index";
				public const string Successful = $"{PasswordChangePrefix}/Successful";
			}
		}

		public static class Users
		{
			private const string UsersPrefix = $"{DashboardPrefix}/Users";
			public const string UsersIndex = $"{UsersPrefix}/Index";
			public const string Add = $"{UsersPrefix}/Add";
			public const string Edit = $"{UsersPrefix}/Edit";
			public const string Delete = $"{UsersPrefix}/Delete";
			public const string Roles = $"{UsersPrefix}/Roles";
		}

		public static class States
		{
			private const string StatesPrefix = $"{DashboardPrefix}/States";
			public const string StatesIndex = $"{StatesPrefix}/Index";
			public const string Add = $"{StatesPrefix}/Add";
			public const string Edit = $"{StatesPrefix}/Edit";
			public const string Delete = $"{StatesPrefix}/Delete";
		}
	}
}