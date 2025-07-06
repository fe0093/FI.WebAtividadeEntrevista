using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class ClienteModel
    {
        
        /// <summary>
        /// Id do Cliente
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Digite um CEP no formato 00000-000.")]
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [Required]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        [Required]
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        [Required]
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required(ErrorMessage = "Campo de CPF é de Preenchimento Obrigatório")]
        [MaxLength(14)]
        [RegularExpression(@"^(((\d{3}).(\d{3}).(\d{3})-(\d{2}))?((\d{2}).(\d{3}).(\d{3})/(\d{4})-(\d{2}))?)*$", ErrorMessage = "Digite um CPF no formato 000.000.000-00.")]
        [CPFModel(ErrorMessage = "Digite um CPF Válido")]
        public string CPF { get; set; }

    }    
}