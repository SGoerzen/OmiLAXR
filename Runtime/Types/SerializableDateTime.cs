using System;

namespace OmiLAXR.Types
{
    [Serializable]
    public class SerializableDateTime
    {
        public int year = 2025;
        public int month = 1;
        public int day = 1;
        public int hour = 0;
        public int minute = 0;
        public int second = 0;
        public int millisecond = 0;

        public DateTime ToDateTime() =>
            new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);

        public void FromDateTime(DateTime dt)
        {
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            hour = dt.Hour;
            minute = dt.Minute;
            second = dt.Second;
            millisecond = dt.Millisecond;
        }
        
        public static SerializableDateTime Now => new SerializableDateTime();
        
        public static implicit operator DateTime(SerializableDateTime sdt) => sdt.ToDateTime();
        public static implicit operator SerializableDateTime(DateTime dt)
        {
            var sdt = new SerializableDateTime();
            sdt.FromDateTime(dt);
            return sdt;
        }
    }
   
}