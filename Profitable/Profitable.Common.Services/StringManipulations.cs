using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Common.Services
{
	public static class StringManipulations
	{
		public static string DivideCapitalizedStringToWords(string capitalizedString)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(capitalizedString[0]);
			for (int i = 1; i < capitalizedString.Length; i++)
			{
				if(char.IsUpper(capitalizedString[i]))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(capitalizedString[i]);
			}

			return stringBuilder.ToString();
		}
	}
}
