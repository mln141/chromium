using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Chromium2017;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
	{
	[TestClass]
	public class UnitTest1
		{
		[TestMethod]
		public void TestMethod1()
			{
			int[] s ;
			var rnd = new Random();
			var sol = new Program.Solution();
			var sw = new Stopwatch();
			var swStd = new Stopwatch();
			for (var i = 0; i < 50; i++)
			{
				var lst = new List<int>();
				switch (i)
				{
					case 2:
						s = new[] {171, 185, 66, 157, 129, 50, 197, 12, 147};
						break;
					case 1:
						s = new[] {4, 6, 2, 1, 5};
						break;
					case 0:
						s = new[] {13, 2, 5};
						break;
					default:
						for (int j = rnd.Next(5, 500); j >= 0; j--) lst.Add(rnd.Next(20000));
						s = lst.ToArray();
						break;
				}
				sw.Start();
				int result = sol.solution(s);
				sw.Stop();
				swStd.Start();
				int resultStd = sol.solutionV3(s);
				swStd.Stop();
				Assert.AreEqual(resultStd, result, $"Failed {i}: {ShowArray(s)}");
				Console.WriteLine($"OK {i}: {ShowArray(s)}");
				Console.WriteLine("---------------");
			}
			Console.WriteLine($"Effect: {(double)sw.ElapsedTicks / swStd.ElapsedTicks}");
			}

		

		private static string ShowArray(int[] s)
			{
			var str = "";
			//s.ToList().ForEach(x=>str += $" {x}");
			return str;
			}
		}
	}