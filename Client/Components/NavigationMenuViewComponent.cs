using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PetStore.Models;

namespace PetStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        #region fields

        private ICategoryRepository _repository;

        #endregion

        public NavigationMenuViewComponent(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            return View(_repository.Categories);
            //return View(_repository.Products
            //    .Select(x => x.Category)
            //    .Distinct()
            //    .OrderBy(x => x));
        }
    }
}
