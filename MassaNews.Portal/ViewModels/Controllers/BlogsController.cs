using Entities.Classes;
using Entities.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MassaNews.Service.Models;
using MassaNews.Service.Services;
using MassaNews.Portal.Functions;
using MassaNews.Portal.ViewModels;
using MassaNews.Service.Util;

namespace MassaNews.Portal.Controllers
{
  public class BlogsController : BaseController
  {
    #region Services

    private BlogService BlogSrv { get; }
    #endregion

    #region Private Properties

    private EntitiesDb Db { get; }

    #endregion

    #region Constructor

    public BlogsController()
    {
      BlogSrv = new BlogService();
      Db = new EntitiesDb();
    }

    #endregion

    #region Actions

    [Route("blogs")]
    public ActionResult Index()
    {
      var lstBlogs = Blog.GetAllByStatus(true);

      var lstViewBlogs = GetListViewBlog(lstBlogs);

      var lstblogCategories = MassaNews.Service.Models.Categoria.GetAllHasBlog().Where(c => c.Status);

      var model = new CategoriaBlogViewModel
      {
        //BaseModel
        Title = "Blogs e Opinião - Massa News",
        Description = "Confira todos os Blogs do Massa News. Economia, esportes, política, agronegócio, policial, cotidiano e muito mais. Confira!",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/blogs",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/editorial/avatar-blogs.jpg",
        //CategoriaBlogViewModel
        Blogs = lstViewBlogs,
        Categories = lstblogCategories
      };

      ViewBag.ActiveNav = "Blogs e Opinião";

      ViewBag.CategoriaUrl = "todos";

      // Página
      ViewBag.Pagina = "blogs";

      // Editoria
      ViewBag.EditoriaUrl = "blogs";

      //Categoria
      ViewBag.Categoria = ViewBag.CategoriaUrl;

      return View("Categoria", model);
    }

    [Route("blogs/{categoria}")]
    public ActionResult Categoria(string categoria)
    {
      if (categoria == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var objCategory = BlogSrv.GetBlogCategoryByUrlCached(categoria);

      var lstBlogs = BlogSrv.GetAllBlogsByCategory(objCategory.Id);

      var lstViewBlogs = GetListViewBlog(lstBlogs);

      var lstblogCategories = MassaNews.Service.Models.Categoria.GetAllHasBlog().Where(c => c.Status);

      var model = new CategoriaBlogViewModel
      {
        //BaseModel
        Title = $"{objCategory.Titulo} - Blogs e Opinião Massa News",
        Description = $"Confira todos os Blogs e Opiniões sobre {objCategory.Titulo} no Massa News.",
        Robots = "index, follow",
        Canonical = $"{Constants.UrlWeb}/blogs/{categoria}",
        ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/editorial/avatar-blogs.jpg",
        //CategoriaBlogViewModel
        Blogs = lstViewBlogs,
        Categories = lstblogCategories
      };

      ViewBag.ActiveNav = "Blogs e Opinião";
      ViewBag.CategoriaUrl = categoria;

      // Página
      ViewBag.Pagina = "blogs";

      // Editoria
      ViewBag.EditoriaUrl = "blogs";

      //Categoria
      ViewBag.Categoria = ViewBag.CategoriaUrl;

      return View(model);
    }

    [Route("blogs/{categoria}/{blog}/posts")]
    public ActionResult BlogPosts(string categoria, string blog, int p = 1)
    {
      if (categoria == null || blog == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      var objBlog = Db.Blogs.FirstOrDefault(b => b.Url == blog);

      if (objBlog == null)
        return new HttpStatusCodeResult(HttpStatusCode.NotFound);

      if(objBlog.Categoria.Url != categoria)
        return new RedirectResult($"/blogs/{objBlog.Categoria.Url}/{objBlog.Url}/posts/", true);

      ViewBag.TotalRegistros = objBlog.Noticias.Count;

      //Get the list of blog autors
      var lstAutors = Autor.GetAllByBlogId(objBlog.Id).ToList();
      
      //Set the path in the images
      lstAutors.ForEach(a=> a.Avatar = $"{Constants.UrlDominioEstaticoUploads}/{"autores"}/{a.Avatar}");

      var model = new BlogPostViewModel
      {
        Blog = objBlog,
        Autors = lstAutors
      };

      model.Blog.Noticias = objBlog.Noticias.Where(post => post.StatusId == Status.Publicada.Id)
          .OrderByDescending(b => b.DataPublicacao)
          .Skip((p - 1)*Constants.TakeNoticias)
          .Take(Constants.TakeNoticias)
          .ToList();

      //Paginação
      ViewBag.PaginaAtual = p;

      var totalPaginas = Math.Ceiling(((double) ViewBag.TotalRegistros/Constants.TakeNoticias));
      if (Convert.ToInt32(totalPaginas) > 1)
        ViewBag.Paginacao = Pagination.AddPagination(ViewBag.PaginaAtual, Convert.ToInt32(totalPaginas), 5, true);

      //base model defaults
      model.Title = $"Blog {objBlog.Titulo} - Massa News";
      model.Description = $"{objBlog.Descricao} Confira o Blog {objBlog.Titulo}!";
      model.Robots = "index, follow";
      model.Canonical = $"{Constants.UrlWeb}/blogs/{categoria}/{blog}/posts";
      model.ImgOpenGraph = $"{Constants.UrlWeb}/content/images/avatar/blogs/{blog}.jpg";

      ViewBag.ActiveNav = objBlog.Titulo;

      // Página
      ViewBag.Pagina = "blog";

      // Editoria
      ViewBag.EditoriaUrl = "blogs";

      // Categoria
      ViewBag.Categoria = model.Blog.Categoria.Url;

      // Blog
      ViewBag.Blog = model.Blog.Url;

      return View(model);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        Db.Dispose();

      base.Dispose(disposing);
    }

    #endregion

    #region Private Methods
    private IEnumerable<CategoriaBlogViewModel.Blog> GetListViewBlog(IEnumerable<Blog> lstBlogs)
    {
      var lstViewBlogs = new List<CategoriaBlogViewModel.Blog>();

      foreach (var objBlog in lstBlogs)
      {
        var objBlogView = new CategoriaBlogViewModel.Blog
        {
          Id = objBlog.Id,
          Titulo = objBlog.Titulo,
          Url = objBlog.Url,
          Img = objBlog.Img,
          CategoriaUrl = objBlog.Categoria.Url
        };

        objBlogView.LastPost = Noticia.GetLastestPostsByBlog(1, objBlog.Id).FirstOrDefault();

        if (string.IsNullOrEmpty(objBlogView.Img))
        {
          var objautor = objBlog.Autores.FirstOrDefault();

          if (objautor != null)
            objBlogView.Img = $"{Constants.UrlDominioEstaticoUploads}/{"autores"}/{objautor.Avatar}";
          else
            objBlogView.Img = Url.Content("~/content/images/placeholders/no-avatar.png");
        }
        else
        {
          objBlogView.Img = $"{Constants.UrlDominioEstaticoUploads}/{"blog"}/{objBlog.Img}";
        }

        lstViewBlogs.Add(objBlogView);
      }

      return lstViewBlogs;
    }

    #endregion
  }
}