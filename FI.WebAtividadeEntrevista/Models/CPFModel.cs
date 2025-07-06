using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FI.WebAtividadeEntrevista.Models
{
    public class CPFModel : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            /* Se o CPF for enviado vazio, sera retornado true,
             * pois a validação de obrigatoriedade do campo CPF não ser vazio ésta no Required*/
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            return ValidaCPF(value.ToString());
        }

        public static bool ValidaCpfBeneficiarios(string cpf)
        {
            return ValidaCPF(cpf);
        }

        private static bool ValidaCPF(string cpf)
        {
            cpf = RemoveNaoNumericos(cpf);

            if (cpf.Length != 11)
                return false;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;
            if (igual)
                return false;

            int soma = 0;
            int resto;


            for (int i = 0; i < 9; i++)
                soma += (10 - i) * int.Parse(cpf[i].ToString());

            resto = 11 - (soma % 11);

            if (resto == 10 || resto == 11)
                resto = 0;

            if (resto != int.Parse(cpf[9].ToString()))
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * int.Parse(cpf[i].ToString());

            resto = 11 - (soma % 11);

            if (resto == 10 || resto == 11)
                resto = 0;

            if (resto != int.Parse(cpf[10].ToString()))
                return false;

            return true;
        }


        public static string RemoveNaoNumericos(string textoDoCpf)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            string retorno = regex.Replace(textoDoCpf, string.Empty);
            return retorno;
        }
    }
}