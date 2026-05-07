using System;

namespace Enterprise.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Tipo { get; set; } = "Particular";
  
        public override string ToString() => Nome;
    }
}