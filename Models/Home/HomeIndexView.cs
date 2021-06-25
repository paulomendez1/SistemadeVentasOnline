﻿using PagedList;
using SistemadeVentasOnline.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SistemadeVentasOnline.Models.Home
{
    public class HomeIndexView
    {
        public IPagedList<Producto> ListaProductos { get; set; }
        public List<Categoria> ListaCategorias { get; set; }

        public HomeIndexView CreateModel(string search, int pageSize, int? page)
        {
            SistemadeVentasEntities db = new SistemadeVentasEntities();
            SqlParameter[] parameter = new SqlParameter[] {
                   new SqlParameter("@search",search??(object)DBNull.Value)
                   };
            IPagedList<Producto> data = db.Database.SqlQuery<Producto>("BuscadorIndex @search", parameter).ToList().ToPagedList(page ?? 1, pageSize);

            List<Categoria> lst;
            using (SistemadeVentasEntities db2 = new SistemadeVentasEntities())
            {
                lst = (from d in db2.Categorias
                       select new Categoria
                       {
                           CategoriaId = d.CategoriaId,
                           CategoriaNombre = d.CategoriaNombre
                       }).ToList();
            }

            return new HomeIndexView()
            {
                ListaProductos = data,
                ListaCategorias = lst
            };
        }
    }
}