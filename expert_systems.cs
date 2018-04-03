using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expert_systems
{
    class Program
    {
        static void Main(string[] args)
        {
       	    StreamReader sr = new StreamReader(@"text.txt");
	    List<string> variables = new List<string>();
            List<string> signs = new List<string>();
            string statment = null;
            string query = null;

            Reader(sr, ref variables, ref signs, ref statment, ref query);
		//Ft_simplify(ref variables, ref signs, query);
            List<char> ft_variables = Ft_getvars(variables);
            List<bool> ft_stats = Ft_status(Ft_getvars(variables), statment, query);
            	//Ft_calc(ft_variables, variables, signs, ref ft_stats, query);
            Ft_answers(ft_variables, ft_stats, query);
            //Console.ReadLine();
            Display_input(variables, signs, statment, query);
        }

        static List<char> Ft_getvars(List<string> variables)
        {
            List<char> chrvars = new List<char>();
            bool fl = false;

            foreach(string element in variables)
            {
                for (int x = 0; x < element.Length; x++)
                {
                    fl = false;
                    for (int j = 0; j < chrvars.Count; j++)
                        if (element[x] == chrvars[j])
                            fl = true;       
                    if (fl == false && element[x] >= 'A' && element[x] <= 'Z')
                        chrvars.Add(element[x]);
                }
                Ft_sort(ref chrvars);
            }
            return chrvars;
        }

        static void Ft_sort(ref List<char> chrs)
        {
            char tmp;

            for (int x = 0; x < chrs.Count; x++)
                for (int y = x; y < chrs.Count; y++)
                    if (chrs[x] > chrs[y])
                    {
                        tmp = chrs[x];
                        chrs[x] = chrs[y];
                        chrs[y] = tmp;
                    }
        }
        
        /*static void Ft_calc(List<char> var, List<string> variables, List<string> signs, ref List<bool> stats, string qry)
        {
            bool ans;
            char val;
            int first = 0;
            int second = first + 1;
            int index = 0;
            int pos = 0;
            int pos1;
            int i = 0;
            List<bool> tempData = new List<bool>();

   
        }*/

        static void Display_input(List<string> variables, List<string> signs, string statment, string query)
        {
            int c = 0;

            Console.WriteLine("----------Variables-------------");
            for (c = 0; c < variables.Count; c++)
                Console.WriteLine("{0}  : {1}", variables[c], signs[c]);
            Console.WriteLine("stmt     : {0}", statment);
            Console.WriteLine("query    : {0}", query);
            Console.ReadLine();
        }

        static string Ft_validVar(string str)
        {
            string newStr = null;

            if (str == null)
                return (null);
            for (int i = 0; i < str.Length; i++)
                if (str[i] == '#')
                    return (newStr);
                else if (str[i] >= 'A' && str[i] <= 'Z')
                    newStr += str[i].ToString();
            return (newStr);
        }

        static string Ft_validSgn(string str)
        {
            string newStr = null;

            if (str == null)
                return (null);
            for (int i = 0; i < str.Length; i++)
                if (str[i] == '#')
                    return (newStr);
                else if (str[i] == '+' || str[i] == '|' || str[i] == '^' || str[i] == '<' || str[i] == '=' || str[i] == '>' || str[i] == '!')
                    newStr += str[i].ToString();
            return (newStr);
        }

        static void Reader(StreamReader sr, ref List<string> variables, ref List<string> signs, ref string statment, ref string query)
        {
            string toAdd = null;
            string tmpVar = null;
            string tmpSgn = null;
	    int c;
	    bool stmt = false;
	    bool qury = false;

            query = null;
            statment = null;
            variables = new List<string>();
            signs = new List<string>();
	    List<string> line = new List<string>();

	    do
            {
               toAdd = sr.ReadLine();
               if (toAdd != null)
               {
                  toAdd = toAdd.Substring(0, toAdd.IndexOf("#")).Replace(" ",string.Empty);
                  if (toAdd != null)
                     if (toAdd.Length != 0)
                        line.Add(toAdd);
               }
            }while (toAdd != null);
		
	    foreach (string element in line)
            {
		if (element[0] == '=')
		   stmt = true;
		else if (element[0] == '?')
                   qury = true;
                c = 0;
                while (c < element.Length)
                {
		    if (element[c] >= 'A' && element[c] <= 'Z')
			tmpVar += new string(element[c], 1);
		    if (element[c] == '!' || element[c] == '+' || element[c] == '<' || element[c] == '=' || element[c] == '>' || element[c] == '|')
			tmpSgn += new string(element[c], 1);
                    c++;
                }
		if (qury == true)
                   query = tmpVar;
                else if (stmt == true)
                   statment = tmpVar;
                else
		{
                   variables.Add(tmpVar);
                   signs.Add(tmpSgn);
		}
                tmpVar = null;
                tmpSgn = null;
	    }
        }

        static List<bool> Ft_status(List<char> vrs, string stm, string qry)
        {
            List<bool> stat = new List<bool>();
            if (stm == null)
                foreach (char element in vrs)
                    stat.Add(false);
            else
                foreach (char element in vrs)
                {
                    int x = 0;
                    bool fl = false;
                    do
                    {
                        if (element == stm[x])
                            fl = true;
                        x++;
                    } while (x < stm.Length);
                    if (fl == true)
                        stat.Add(true);
                    else
                        stat.Add(false);
                }
            return stat;
        }

        static void Ft_answers(List<char> vrs, List<bool> stat, string qry)
        {
            if (qry != null)
                for (int x = 0; x < qry.Length; x++)
                    for (int y = 0; y < vrs.Count; y++)
                        if (vrs[y] == qry[x])
                        {
                            Console.Write("{0} is ", vrs[y]);
                            Console.WriteLine("{0}", stat[y]);
                        }
        }

        static bool Ft_not(bool first)
        {
            if (first == true)
                return (false);
            return (true);
        }
        static bool Ft_and(bool first, bool second)
        {
            if (first == true && second == true)
                return (true);

            return (false);

        }



        static bool Ft_or(bool first, bool second)

        {

            if (first == true || second == true)

                return (true);

            return (false);

        }



        static bool Ft_xor(bool first, bool second)

        {

            if (first != second)

                return (true);

            return (false);

        }



        static bool Ft_impies(bool first, bool second)

        {

            if (first == true && second == false)

                return (false);

            return (true);

        }



        static bool Ft_if_and_only(bool first, bool second)

        {

            if (first == second)

                return (true);

            return (false);

        }

    }

}
