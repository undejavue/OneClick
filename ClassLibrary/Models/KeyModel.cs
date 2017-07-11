namespace ClassLibrary.Models
{
    public class KeyModel : BaseEntityModel
    {
        public int KeyId { get; set; }

        public virtual CategoryModel CategoryModel { get; set; }
    }
}
