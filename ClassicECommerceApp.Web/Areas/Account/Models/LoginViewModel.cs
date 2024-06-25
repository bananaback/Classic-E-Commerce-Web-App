using ClassicECommerceApp.Web.Areas.Account.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace ClassicECommerceApp.Web.Areas.Account.Models
{
	public class LoginViewModel
	{
		[Required]
		[CustomPasswordValidation]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}
}
