using System.Text.Json.Serialization;

namespace DomainLayer.Enums
{
    public enum MovieGenre
    {
        Action = 0,
        Comedy = 1,
        Drama = 2,
        ScienceFiction = 3,
        Fantasy = 4,
        Horror = 5,
        Mystery = 6,
        Romance = 7,
        Adventure = 8,
        Animation = 9,
        War = 10,
        Documentary = 11
    }

    public enum Status
    {
        Success,
        Failed
    }
}
