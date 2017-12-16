//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownload.CIClient
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        /// <summary>
        /// Runtime Parameters
        /// </summary>
        private static List<ConfigurationParameter> parameters = new List<ConfigurationParameter>(); 

        static void Main(string[] args)
        {
            GetParameter(args);
        }

        private static void GetParameter(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Missing Parameter");
                }

                int counter = 0;
                ConfigurationParameter parameter;
                foreach (string arg in args)
                {
                    if (counter == 0 || (counter % 2) == 0)
                    {
                        parameter = new ConfigurationParameter();

                        switch (arg.ToLowerInvariant())
                        {
                            case "--connection-string":
                                parameter.Name = arg.Remove(0,2);
                                parameter.Value = args[counter + 1];
                                break;
                            case "--solution-name":
                                parameter.Name = arg.Remove(0, 2);
                                parameter.Value = args[counter + 1];
                                break;
                            case "--local-path":
                                parameter.Name = arg.Remove(0, 2);
                                parameter.Value = args[counter + 1];
                                break;
                            default:
                                Console.WriteLine("Unregonized parameter :" + arg);
                                break;
                        }
                        parameters.Add(parameter);
                    }
                    counter++;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
