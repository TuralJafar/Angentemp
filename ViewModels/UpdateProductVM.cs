namespace WebApplication8.ViewModels
{
	public class UpdateProductVM
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public IFormFile Photo { get; set; }
		public int CategoryId { get; set; }
	}
}
