using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

namespace Data.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;
        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }
        public void ActualizarCategoria(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            _context.SaveChanges();
        }

        public Categoria CrearCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public void EliminarCategoria(int idCategoria)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Id == idCategoria);
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }

        public Categoria ObtenerCategoria(int idCategoria)
        {
            return _context.Categorias.FirstOrDefault(c => c.Id == idCategoria);
        }

        public Categoria ObtenerCategoriaConProductos(int idCategoria)
        {
            return _context.Categorias.Include(c => c.Productos).FirstOrDefault(c => c.Id == idCategoria);
        }

        public ICollection<Categoria> ObtenerCategorias()
        {
            return _context.Categorias.ToList();
        }
    }
}
