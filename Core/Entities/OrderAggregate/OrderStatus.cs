using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value= "Pending")]
        Pending,
        [EnumMember(Value= "PaymentRecevied")]
        PaymentRecevied,
        [EnumMember(Value= "PaymentFailed")]
        PaymentFailed

    }
}