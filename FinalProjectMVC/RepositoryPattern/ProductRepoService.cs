using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Areas.SellerPanel.Models;
using FinalProjectMVC.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FinalProjectMVC.RepositoryPattern
{
    public class ProductRepoService : EFCoreRepo<Product>

    {
        private readonly IRepository<Review> _reviewtRepository;
        public ProductRepoService(ApplicationDbContext context, IRepository<Review> reviewtRepository) : base(context)
        {
            _reviewtRepository = reviewtRepository;
        }
    
        //public async Task<ICollection<Review>> GetReviews(int? productId)
        //{
        //    if (productId == null)
        //    {
        //        return new List<Review>();
        //    }

        //    var reviews = await _reviewtRepository.Where(p => p.ProductId == productId);

        //    return reviews;
        //}

        //public virtual List<Product> GetAll()
        //{
        //    return _context.Set<Product>().ToList();
        //}
    }
}
