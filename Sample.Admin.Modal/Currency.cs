using System.ComponentModel.DataAnnotations;

namespace Sample.Admin.Model
{
    public class CurrencyModel
    {
        /// <summary>
        /// Currency identifier
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Currency  Code 
        /// </summary>
        [Required(ErrorMessage = "Please enter Code"), MaxLength(20, ErrorMessage = "Maximum lenght is 20")]
        public string Code { get; set; }
        /// <summary>
        /// Currency  Display Name 
        /// </summary>
        [Required(ErrorMessage = "Please enter Display Name"), MaxLength(100, ErrorMessage = "Maximum lenght is 100")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Currency is Base Currency or not
        /// </summary>        
        public bool BaseCurrency { get; set; }

        /// <summary>
        /// Currency  Description 
        /// </summary>
        [MaxLength(250, ErrorMessage = "Maximum lenght is 250")]
        public string Description { get; set; }
    }
}
