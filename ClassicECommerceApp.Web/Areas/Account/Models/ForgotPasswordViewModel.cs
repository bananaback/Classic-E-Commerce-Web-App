using System.ComponentModel.DataAnnotations;

namespace ClassicECommerceApp.Web.Areas.Account.Models
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}
}
