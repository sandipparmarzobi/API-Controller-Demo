using System.Text.Json.Serialization;

namespace DomainLayer.Enums
{
    public enum MovieGenre
    {
        Action,
        Comedy,
        Drama ,
        ScienceFiction,
        Fantasy,
        Horror,
        Mystery,
        Romance,
        Adventure,
        Animation,
        War,
        Documentary
    }


    public enum Status
    {
        Success,
        Failed
    }

    public enum BookingStatus
    {
        Confirmed,
        Cancelled
    }
}
