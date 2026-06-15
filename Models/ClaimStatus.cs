using System.ComponentModel.DataAnnotations;

namespace FlyJusticeLite.Models;

public enum ClaimStatus
{
    [Display(Name = "Pending")]
    Pending = 0,

    [Display(Name = "Under Review")]
    UnderReview = 1,

    [Display(Name = "Approved")]
    Approved = 2,

    [Display(Name = "Rejected")]
    Rejected = 3
}
