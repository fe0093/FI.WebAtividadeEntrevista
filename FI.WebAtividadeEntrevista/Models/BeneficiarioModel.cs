using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FI.WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        /// <summary>
        /// Id do Beneficiario
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// CPF do Beneficiario
        /// </summary>
        [MaxLength(14)]
        [RegularExpression(@"^(((\d{3}).(\d{3}).(\d{3})-(\d{2}))?((\d{2}).(\d{3}).(\d{3})/(\d{4})-(\d{2}))?)*$", ErrorMessage = "Digite um CPF no formato 000.000.000-00.")]
        [CPFModel(ErrorMessage = "Digite um CPF Válido")]
        public string CPF { get; set; }


        /// <summary>
        /// Nome do Beneficiario
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Id do Cliente ao qual o beneficiario está vinculado
        /// </summary>
        public long IdCliente { get; set; }

        /// <summary>
        /// CPF Original do Cliente
        /// </summary>
        public string CPFOriginal { get; set; }

    }
}