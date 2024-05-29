using DekoBimApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DekoBimApi.Data
{
    public class RepositoryContext:DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<SubDiscipline> SubDisciplines { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<FileType>FileTypes { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<User>Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Istatistics> Istatistics { get; set; }
        public DbSet<DownloadedFile> DownloadedFiles { get; set; }

    }
}
