using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Global.Util
{
    public static class TextExtensions
    {

        public static string GetRacaValue(string value)
        {
            switch (value)
            {
                case "Branco":
                    return "Branca";
                case "Preto":
                    return "Preta";
                case "Pardo":
                    return "Parda";
                case "Indígena":
                    return "Indigena";
                case "Amarelo":
                    return "Amarela";
                case "Não desejo informar":
                    return "Indisponivel";
                default:
                    return value;

            }
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return "!@" + new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public static string GetISOCountryNameByCode(int code) 
        {

            return ISO3166.Country.List.Where(x => Convert.ToInt32(x.NumericCode) == code).FirstOrDefault()?.TwoLetterCode;     
        
        }
        public static string RandomPassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string numbers = "0123456789";

            return "!@" + new string(Enumerable.Repeat(chars, length / 2)
              .Select(s => s[random.Next(s.Length)]).ToArray())
                + new string(Enumerable.Repeat(numbers, length / 2)
              .Select(s => s[random.Next(s.Length)]).ToArray())
                ;
        }

        public static string ConverterEstados(this string text)
        {

            switch (text.ToUpper())
            {
                /* UFs */
                case "AC": text = "Acre"; break;
                case "AL": text = "Alagoas"; break;
                case "AM": text = "Amazonas"; break;
                case "AP": text = "Amapá"; break;
                case "BA": text = "Bahia"; break;
                case "CE": text = "Ceará"; break;
                case "DF": text = "Distrito Federal"; break;
                case "ES": text = "Espírito Santo"; break;
                case "GO": text = "Goiás"; break;
                case "MA": text = "Maranhão"; break;
                case "MG": text = "Minas Gerais"; break;
                case "MS": text = "Mato Grosso do Sul"; break;
                case "MT": text = "Mato Grosso"; break;
                case "PA": text = "Pará"; break;
                case "PB": text = "Paraíba"; break;
                case "PE": text = "Pernambuco"; break;
                case "PI": text = "Piauí"; break;
                case "PR": text = "Paraná"; break;
                case "RJ": text = "Rio de Janeiro"; break;
                case "RN": text = "Rio Grande do Norte"; break;
                case "RO": text = "Rondônia"; break;
                case "RR": text = "Roraima"; break;
                case "RS": text = "Rio Grande do Sul"; break;
                case "SC": text = "Santa Catarina"; break;
                case "SE": text = "Sergipe"; break;
                case "SP": text = "São Paulo"; break;
                case "TO": text = "Tocantíns"; break;

                /* Estados */
                case "ACRE": text = "AC"; break;
                case "ALAGOAS": text = "AL"; break;
                case "AMAZONAS": text = "AM"; break;
                case "AMAPÁ": text = "AP"; break;
                case "BAHIA": text = "BA"; break;
                case "CEARÁ": text = "CE"; break;
                case "DISTRITO FEDERAL": text = "DF"; break;
                case "ESPÍRITO SANTO": text = "ES"; break;
                case "GOIÁS": text = "GO"; break;
                case "MARANHÃO": text = "MA"; break;
                case "MINAS GERAIS": text = "MG"; break;
                case "MATO GROSSO DO SUL": text = "MS"; break;
                case "MATO GROSSO": text = "MT"; break;
                case "PARÁ": text = "PA"; break;
                case "PARAÍBA": text = "PB"; break;
                case "PERNAMBUCO": text = "PE"; break;
                case "PIAUÍ": text = "PI"; break;
                case "PARANÁ": text = "PR"; break;
                case "RIO DE JANEIRO": text = "RJ"; break;
                case "RIO GRANDE DO NORTE": text = "RN"; break;
                case "RONDÔNIA": text = "RO"; break;
                case "RORAIMA": text = "RR"; break;
                case "RIO GRANDE DO SUL": text = "RS"; break;
                case "SANTA CATARINA": text = "SC"; break;
                case "SERGIPE": text = "SE"; break;
                case "SÃO PAULO": text = "SP"; break;
                case "TOCANTÍNS": text = "TO"; break;
                default: text = "N/A";  break;
            }

            return text;
        }
    }
}
