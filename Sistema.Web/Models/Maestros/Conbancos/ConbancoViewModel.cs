﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Maestros.Conbancos
{
    public class ConbancoViewModel
    {
        public int Id { get; set; }
        public int empresaId { get; set; }
        public string empresa { get; set; }
        public string orden { get; set; }
        public string nombre { get; set; }
        public int bancoId { get; set; }
        public string banco { get; set; }
        public int grpconceptoId { get; set; }
        public string grpconcepto { get; set; }
        public int iduseralta { get; set; }
        public DateTime fecalta { get; set; }
        public int iduserumod { get; set; }
        public DateTime fecumod { get; set; }
        public bool activo { get; set; }
    }
}
