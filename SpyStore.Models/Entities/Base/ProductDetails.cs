using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

 // allows this class to be added as property to another entity class
[Owned]
public class ProductDetails
{
    [MaxLength(3800)]
    public string Description { get; set; }
    [MaxLength(50)]
    public string ModelNumber { get; set; }
    [MaxLength(50)]
    public string ModelName { get; set; }
    [MaxLength(150)]
    public string ProductImage { get; set; }
    [MaxLength(150)]
    public string ProductImageLarge { get; set; }
    [MaxLength(150)]
    public string ProductImageThumb { get; set; }
}