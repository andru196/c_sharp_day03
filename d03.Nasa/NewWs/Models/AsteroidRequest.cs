using System;

namespace d03.Nasa.NewWs.Models
{
    public record AsteroidRequest 
    {
        public DateTime StartDT {get; init;}
        public DateTime EndDT {get; init;}
        public ushort Count {get; init;}
    }
}