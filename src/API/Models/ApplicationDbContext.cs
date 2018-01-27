using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class ApplicationDbContext : DbContext
    {
    }
	
	public class ApplicationDbContextSettings
    {
        public string DefaultConnection { get; set; }
    }
}
