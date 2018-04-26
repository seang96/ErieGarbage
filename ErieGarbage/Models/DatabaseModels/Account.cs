using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErieGarbage.Models.DatabaseModels
{
	public class Account
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int AccountID { get; set; }
		
		[Required]
		public string AccountNumber { get; set; }
		
		public int? CustomerID { get; set; }
		
		public virtual Customer Customer { get; set; }
	}
}