using System.ComponentModel.DataAnnotations;

namespace B2BPriceAdmin.Database.Entities
{
    public interface ITimeStamp : IBaseEntity
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
