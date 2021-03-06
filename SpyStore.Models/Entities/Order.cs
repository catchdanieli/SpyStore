using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using SpyStore.Models.Entities.Base;

[Table("Orders", Schema = "Store")]
public class Order : OrderBase
{
    [InverseProperty(nameof(OrderDetail.OrderNavigation))]
    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey(nameof(CustomerId))]
    public Customer CustomerNavigation { get; set; }

    [Display(Name="Total")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal OrderTotal { get; set; }
}