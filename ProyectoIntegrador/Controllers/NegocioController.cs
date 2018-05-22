using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.ReferenciaNegocio;

namespace ProyectoIntegrador.Controllers
{
    public class NegocioController : Controller
    {
        ServicioNegocioClient servicio = new ServicioNegocioClient();

        public ActionResult RegistroUsuario()
        {
            //ViewBag.tipoUsu = new SelectList(servicio.ListarTipoUsuario(), "ID_TIPO_USU", "DES_TIPO_USU");
            ViewBag.tipoDoc = new SelectList(servicio.ListarTipoDocumento(), "ID_DOC", "DES_DOC");
            ViewBag.lstDist = new SelectList(servicio.ListarDistritos(), "ID_DISTRITO", "NOM_DISTRITO");

            return View(new TB_USUARIO());
        }

        [HttpPost]
        public ActionResult RegistroUsuario(TB_USUARIO reg)
        {
            string mensaje = servicio.RegistrarUsuario(reg);
            string msg = servicio.EnviarCorreoConfimacion();

            return RedirectToAction("RegistroUsuario");
        }
    }
}