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
            List<string> factVar = new List<string>();
            List<string> factSgn = new List<string>();
            string statment = null;
            string query = null;

            Reader(sr, ref factVar, ref factSgn, ref statment, ref query);
            List<char> ft_variables = Ft_getvars(factVar);
            List<bool> ft_stats = Ft_status(Ft_getvars(factVar), statment);
            
            Ft_calc(ft_variables, factVar, factSgn, ref ft_stats, query);
            Ft_answers(ft_variables, ft_stats, query);
            Display_input(factVar, factSgn, statment, query);
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

        static List<int> Ft_searchInstance(List<string> factVar, List<string> factSgn, char toFind)
        {
            List<int> instance = new List<int>();
            bool isNegate = false;
            for (int x = 0; x < factSgn.Count; x++)
            {
                isNegate = false;
                foreach(char element in factSgn[x])
                {
                    isNegate = (element == '!') ? true : false;
                    if (isNegate == true)
                        break ;
                }
                if (factSgn[x][factSgn[x].Length - 1] == '>')
                {
                    if (isNegate == true)
                    {
                        if (factVar[x][factSgn[x].Length - 2] == toFind)
                            instance.Add(x);
                    }
                    else
                    {
                        if (factVar[x][factSgn[x].Length - 1] == toFind)
                            instance.Add(x);
                    }
                }
                // else
                // {
                //     if (factSgn[x][factSgn[x].Length - 2] == '>')
                //     {
                //         if (isNegate == true)
                //         {
                //             if (factVar[x][factSgn[x].Length - 1] == toFind)
                //             instance.Add(x);
                //         }
                //         else
                //         {
                //             if (factVar[x][factSgn[x].Length - 1] == toFind)
                //             instance.Add(x);
                //         }
                //     }
                // }        
            } 
            return instance;
        }

        static int Ft_varPos(List<char> vars, char toFind)
        {
            for (int x = 0; x < vars.Count; x++)
                if (toFind == vars[x])
                    return (x);
            return (-1);
        }

        static bool Ft_solveOne(string currentTree, string signs, List<bool> stats, List<char> vars)
        {
            if (signs[0] == '!')
                return (Ft_not(stats[Ft_varPos(vars, currentTree[0])]));
            return (stats[Ft_varPos(vars, currentTree[0])]);
        }

        static bool Ft_solveTwo(string currentTree, string signs, List<bool> stats, List<char> vars)
        {
            if (signs[0] == '+')
                {
                    if (signs[0] == '!')
                        return Ft_and(Ft_not(stats[Ft_varPos(vars, currentTree[0])]), stats[Ft_varPos(vars, currentTree[1])]);
                    else if (signs[1] == '!')
                        return Ft_and(stats[Ft_varPos(vars, currentTree[0])], Ft_not(stats[Ft_varPos(vars, currentTree[1])]));
                    return Ft_and(stats[Ft_varPos(vars, currentTree[0])], stats[Ft_varPos(vars, currentTree[1])]);
                }
                else if (signs[0] == '|')
                {
                    if (signs[0] == '!')
                        return Ft_or(Ft_not(stats[Ft_varPos(vars, currentTree[0])]), stats[Ft_varPos(vars, currentTree[1])]);
                    else if (signs[1] == '!')
                        return Ft_or(stats[Ft_varPos(vars, currentTree[0])], Ft_not(stats[Ft_varPos(vars, currentTree[1])]));
                    return Ft_or(stats[Ft_varPos(vars, currentTree[0])], stats[Ft_varPos(vars, currentTree[1])]);
                }
                else if (signs[0] == '^')
                {
                    if (signs[0] == '!')
                        return Ft_xor(Ft_not(stats[Ft_varPos(vars, currentTree[0])]), stats[Ft_varPos(vars, currentTree[1])]);
                    else if (signs[1] == '!')
                        return Ft_xor(stats[Ft_varPos(vars, currentTree[0])], Ft_not(stats[Ft_varPos(vars, currentTree[1])]));
                    return Ft_xor(stats[Ft_varPos(vars, currentTree[0])], stats[Ft_varPos(vars, currentTree[1])]);
                }
                return (false);
        }

        static bool ft_Poduct(string currentTree, string signs, List<bool> stats, List<char> vars)
        {
            int first = 0;
            int second = first + 1;

            if (currentTree.Length == 2)
                return Ft_solveOne(currentTree, signs, stats, vars);
            else if (currentTree.Length == 3)
                return Ft_solveTwo(currentTree,signs,stats,vars);
            else if (currentTree.Length > 3)
            {

            }
            return (false);
        }

        static bool Ft_backwardChain(char qry, List<string> factVar, List<string> factSgn, ref List<bool> stats, List<char> vars)
        {
            char toFind;
            string currentTree = null;
            string currentsigns = null;
            List<bool> tmpBool = new List<bool>();
            List<char> tmpChr  = new List<char>();

            toFind = qry;
            if (Ft_searchInstance(factVar, factSgn, toFind).Count == 0)
            {
                return (stats[Ft_varPos(vars, toFind)]);
            }
            foreach (int element in Ft_searchInstance(factVar, factSgn, toFind))
            {
                currentTree = factVar[element];
                currentsigns = factSgn[element];
                for (int x = 0; x < currentTree.Length - 1; x++)
                {
                    // Console.WriteLine("{0}---{1}", stats[Ft_varPos(vars, currentTree[x])], vars[Ft_varPos(vars, currentTree[x])]);
                    stats[Ft_varPos(vars, currentTree[x])] = Ft_backwardChain(currentTree[x], factVar, factSgn, ref stats, vars);
                    tmpBool.Add(stats[Ft_varPos(vars, currentTree[x])]);
                    tmpChr.Add(currentTree[x]);
                    // Console.WriteLine("{0}", tmpChr.Last());
                    for (int y = 0; y < tmpChr.Count; y++)
                        for (int z = y; z < tmpChr.Count; z++)
                            if (tmpChr[y] == tmpChr[z])
                                stats[Ft_varPos(vars, currentTree[y])] = Ft_or(stats[Ft_varPos(vars, currentTree[y])], Ft_or(tmpBool[y], tmpBool[z]));
                } 
            }
            return (ft_Poduct(currentTree, currentsigns, stats, vars));
        }
        
         static void Ft_calc(List<char> var, List<string> factVar, List<string> factSgn, ref List<bool> stats, string qry)
         {
            string tmpVar = null;
            string tmpSgn = null;

            int Count = 1;
            for (int x = 0; x < factSgn.Count; x++)
            {
                Count = 1;
                for (int y = factSgn[x].Length; y > 0; y--)
                {
                    if (factSgn[x][factSgn[x].Length - Count] == '>')
                        break;
                    Count++;
                }
                if (Count >= 2)
                {
                    for (int y = 0; y < Count; y++)
                    {
                        tmpVar = factVar[x].Substring(0 ,factVar[x].Length - Count);
                        tmpVar += new string(factVar[x][tmpVar.Length + y], 1);
                        for (int z = 0; z < Count; z++)
                            tmpSgn = factSgn[x].Substring(0, factSgn[x].Length - (z));
                        // Console.WriteLine("{0} {1}", tmpVar, tmpSgn);
                        factVar.Add(tmpVar);
                        factSgn.Add(tmpSgn);
                    }
                    factVar.Remove(factVar[x]);
                    factSgn.Remove(factSgn[x]);
                }
            }
            foreach (char element in qry)
            {
                stats[Ft_varPos(var, element)] = Ft_backwardChain(element, factVar,factSgn, ref stats, var);
            }
         }

        static void Display_input(List<string> factVar, List<string> factSgn, string statment, string query)
        {
            int c = 0;

            Console.WriteLine("---------------Input-----------------");
            for (c = 0; c < factVar.Count; c++)
                Console.WriteLine("{0}  : {1}", factVar[c], factSgn[c]);
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

        static void Reader(StreamReader sr, ref List<string> variables, ref List<string> signs, ref string statment, ref string query)
        {
            List<string> line = new List<string>();
            string toAdd = null;
            string tmpVar = null;
            string tmpSgn = null;
            bool stmt = false;
            bool qury = false;

            query = null;
            statment = null;
            variables = new List<string>();
            signs = new List<string>();
    	    do
            {
               toAdd = sr.ReadLine();
               if (toAdd != null)
               {
                    if (toAdd.IndexOf("#") >= 0)
                        toAdd = toAdd.Substring(0, toAdd.IndexOf("#")).Replace(" ",string.Empty);
                    if (toAdd != null)
                        if (toAdd.Length != 0)
                            line.Add(toAdd);
               }
            }while (toAdd != null);
    	    foreach (string element in line)
            {
                qury = (element[0] == '?') ? true : false;
                stmt = (element[0] == '=') ? true : false;
                for (int index = 0; index < element.Length; index++)
                {
                    if ((element[index] >= 'A' && element[index] <= 'Z') || element[index] == '(' || element[index] == ')')
                        tmpVar += new string(element[index], 1);
                    if (element[index] == '!' || element[index] == '+' || element[index] == '^' || element[index] == '<' || element[index] == '=' || element[index] == '>' || element[index] == '|')
                        tmpSgn += new string(element[index], 1);
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

        static List<bool> Ft_status(List<char> vrs, string stm)
        {
            List<bool> stat = new List<bool>();
            bool fl = false;

            if (stm == null)
                foreach (char element in vrs)
                    stat.Add(false);
            else
                foreach (char element in vrs)
                {
                    for (int x = 0; x < stm.Length; x++)
                    {
                        fl = (element == stm[x]) ? true : false;
                        if (fl == true)
                            break ;
                    }
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
                            Console.WriteLine("{0} is {1}", vrs[y], stat[y]);
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