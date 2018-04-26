namespace ErieGarbage.Models
{
	public class ProfileForm
	{
		public string Account { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string Street { get; set; }
		public string PhoneNumber { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public bool Suspended { get; set; }
		public string Error { get; set; }
	}
}