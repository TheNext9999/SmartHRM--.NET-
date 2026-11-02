namespace SmartHRM.DTO
{
    public class BaseDTO
    {
        [Field("ID", Hidden = true, InGrid = false)]
        public int Id { get; set; }
    }
}
