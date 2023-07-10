using System.ComponentModel.DataAnnotations;

namespace PayAppApi.Common
{
    public enum PaymentStatus
    {
        [Display(Name = "UnPaid")]
        UnPaid ,
        [Display(Name = "Paid")]
        Paid
    }
}
