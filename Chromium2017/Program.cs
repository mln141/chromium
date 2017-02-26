using System;
using System.Collections.Generic;
using System.Linq;

namespace Chromium2017
	{
		public class Mdb
		{
			public static bool Print;
			public static bool ConsoleApp;
			public static Dictionary<int, long> TestResultsDictionary = new Dictionary<int, long> ();
			public static Dictionary<int, long> ResultsDictionary = new Dictionary<int, long> ();
		}
	public class Program
	{
		private static void Main(string[] args)
			{
			int[] s = {171, 185, 66, 157, 129, 50, 197, 12, 147};
			//s = new[] {4, 6, 2, 1, 5};
			//s = new[] {13, 2, 5};
			s = new[] {101, 187, 160, 139, 60, 96, 158, 38, 170, 59, 40, 152, 186, 88, 160, 125, 146};
			s = new[] { 46,74,20,57,15,199,32,46,52,12,116,19,46,169,191,72,109};
			var sol = new Solution();
			Mdb.ConsoleApp = true;
			var test = sol.solution(s);
			var fact = sol.solutionStd(s);
			Console.WriteLine($"=={test}");
			Console.WriteLine($"=={fact}");
			if (test != fact && Mdb.ConsoleApp)
			{
				Console.WriteLine("Differs");
				var zv = " *** ";
				for (int i=0;i<s.Length;i++)
					Console.WriteLine($"{s[i]}:fact= {Mdb.ResultsDictionary[i]} test={Mdb.TestResultsDictionary[i]} " +
					                  $"{(Mdb.ResultsDictionary[i]!=Mdb.TestResultsDictionary[i])}");
			}
			Console.ReadKey();
			}
		public class Solution
			{
			#region test solution

			public int solutionStd(int[] H)
				{
				long count = 0;
				for (var i = 0; i < H.Length; i++)
				{
					count++; //When start=root=end
					int mid = H[i];
					var left = new List<int>();
					var right = new List<int>();
					for (var j = 0; j < i; j++) if (H[j] > mid) left.Add(H[j]);
					for (int j = H.Length - 1; j > i; j--) if (H[j] > mid) right.Add(H[j]);
					count += ZigZags(left.OrderBy(x=>x).ToList(), right.OrderBy(x=>x).ToList()) +
						ZigZags(right.OrderBy(x=>x).ToList(), left.OrderBy(x=>x).ToList());
					if(Mdb.ConsoleApp) Mdb.TestResultsDictionary.Add(i,count);
				}
				return (int) (count % 1000000007);
				}


			public long ZigZags(List<int> first, List<int> next)
				{
				if (first.Count == 0)
					return 0;
				if (next.Count == 0) return first.Count;
				long count = 0;
				for (int i = first.Count - 1; i >= 0; i--)
				{
					count++;
					//count += ZigZags (next.Where (x => x > first[i]).ToList (), first.Where (x => x > first[i]).ToList ());
					count += ZigZags(next.Where(x=>x > first[i]).ToList(), first.Skip(i).ToList());
				}
				return count;
				}

			#endregion
			enum NodePositionType { Left, Right, None, Node }
			
			#region V2
			public int SolutionV2(int[] H)
			{
				const int Norma = 1000000007;
				long count = 0;
				int dimension = H.Length;
				for (int i = 0;  i < dimension; i++)
				{
					count = (count + CalcOneV2(H,i)) % Norma;
				}
				return (int) (count % Norma);
				}
			private long CalcOneV2 (int[] h, int nodePosition)
			{
				var tmpPlaces = new Dictionary<int, NodePositionType>();
				int dimension = h.Length;
				int node = h[nodePosition];
				var replacements = new Dictionary<int, int> ();
				var ways = new long[dimension];
				long sumL = 0, sumR = 0;
				tmpPlaces.Add(nodePosition,NodePositionType.Node);
				for (int i = 0; i < nodePosition; i++)
					tmpPlaces.Add (i, h[i] <= node ? NodePositionType.None : NodePositionType.Left);
				for (int i = nodePosition+1; i < dimension; i++)
					tmpPlaces.Add (i, h[i] <= node ? NodePositionType.None : NodePositionType.Right);
				var ordered=new List<KeyValuePair<int,int>>();
				for (int i=0;i<dimension;i++) ordered.Add(new KeyValuePair<int, int>(i,i));
				ordered = ordered.OrderBy(x => h[x.Key]).ToList();
				for (int i = 0; i < dimension; i++)
					replacements.Add(i,ordered[i].Key);
				for (int i = dimension - 1; i >= 0; i--)
				{
					switch (tmpPlaces[replacements[i]])
					{
						case NodePositionType.Left:
							ways[i] = 1 + sumR;
							for (int j=i+1; j<dimension 
								&& h[replacements[i]] == h[replacements[j]];j++)
													if(tmpPlaces[replacements[i]] != tmpPlaces[replacements[j]]) ways[i] -= ways[j];
							sumL += ways[i];
							break;
						case NodePositionType.Right:
							ways[i] = 1 + sumL;
							for (int j = i + 1;
								j < dimension
								&& h[replacements[i]] == h[replacements[j]]; j++)
									if(tmpPlaces[replacements[i]] != tmpPlaces[replacements[j]]) ways[i] -= ways[j];
							sumR += ways[i];
							break;
							case NodePositionType.None:
							if (i < dimension - 1) ways[i] = ways[i + 1];
							break;
						case NodePositionType.Node:
							ways[i] = sumL + sumR + 1;
							break;
					}
					//if(Mdb.Print && Mdb.ConsoleApp) Console.WriteLine ($"From {h[replacements[i]]}({tmpPlaces[replacements[i]]}): {ways[i]}");
				}
				return ways[0];
			}
			#endregion
			#region V3
			public int solutionV3 (int[] H)
			{
				const int norma = 1000000007;
				long count = 0;
				int dimension = H.Length;
				var tmpPlaces = new NodePositionType[dimension];
				var replacements = new int[dimension];
				var ordered = new List<KeyValuePair<int, int>> ();
				for (int i = 0; i < dimension; i++) ordered.Add (new KeyValuePair<int, int> (i, i));
				ordered = ordered.OrderBy (x => H[x.Key]).ToList ();
				for (int i = 0; i < dimension; i++)
					replacements[i]= ordered[i].Key;
				for (int i = 0; i < dimension; i++)
				{
					int node = H[i];
					tmpPlaces[i]=NodePositionType.Node;
					for (int j = 0; j < i; j++)
						tmpPlaces[j]= H[j] <= node ? NodePositionType.None : NodePositionType.Left;
					for (int j = i + 1; j < dimension; j++)
						tmpPlaces[j]= H[j] <= node ? NodePositionType.None : NodePositionType.Right;

					//Mdb.Print = H[i] == 12;
					//if (Mdb.Print && Mdb.ConsoleApp) Console.WriteLine ($"**** {H[i]} ***");
					count = (count + CalcOneV3 (H, tmpPlaces, replacements) % norma) % norma;
					//if (Mdb.ConsoleApp) Mdb.ResultsDictionary.Add (i, count);
				}
				return (int)(count % norma);
			}
			private long CalcOneV3 (int[] h, NodePositionType[] tmpPlaces, int[] replacements)
			{
				int dimension = h.Length;
				var ways = new long[dimension];
				long sumL = 0, sumR = 0;
				
				for (int i = dimension - 1; i >= 0; i--)
				{
					int rpm = replacements[i];
					var tmpP = tmpPlaces[rpm];
					switch (tmpP)
					{
						case NodePositionType.Left:
							ways[i] = 1 + sumR;
							for (int j = i + 1; j < dimension
								&& h[rpm] == h[replacements[j]]; j++)
								if (tmpP != tmpPlaces[replacements[j]]) ways[i] -= ways[j];
							sumL += ways[i];
							break;
						case NodePositionType.Right:
							ways[i] = 1 + sumL;
							for (int j = i + 1;
								j < dimension
								&& h[rpm] == h[replacements[j]]; j++)
								if (tmpP != tmpPlaces[replacements[j]]) ways[i] -= ways[j];
							sumR += ways[i];
							break;
						case NodePositionType.None:
							return sumL + sumR + 1;
						case NodePositionType.Node:
							ways[i] = sumL + sumR + 1;
							break;
					}
					//if (Mdb.Print && Mdb.ConsoleApp) Console.WriteLine ($"From {h[replacements[i]]}({tmpPlaces[replacements[i]]}): {ways[i]}");
				}
				return ways[0];
			}
		#endregion
				public int solution (int[] H)
			{
				const int norma = 1000000007;
				long count = 0;
				int dimension = H.Length;
				var tmpPlaces = new NodePositionType[dimension];
				var replacements = new int[dimension];
				var ordered = new List<KeyValuePair<int, int>> ();
				for (int i = 0; i < dimension; i++) ordered.Add (new KeyValuePair<int, int> (i, i));
				ordered = ordered.OrderBy (x => H[x.Key]).ToList ();
				for (int i = 0; i < dimension; i++)
					replacements[i] = ordered[i].Key;
				for (int i = 0; i < dimension; tmpPlaces[i++]= NodePositionType.Left)
				{
					int node = H[i];
					if (i == 0)
					{
						tmpPlaces[i] = NodePositionType.Node;
						for (int j = 1; j < dimension; j++)
							tmpPlaces[j] =  NodePositionType.Right;
					}
					else
					{
						tmpPlaces[i] = NodePositionType.Node;
					}
					Mdb.Print = H[i] == 46;
					if (Mdb.Print && Mdb.ConsoleApp) Console.WriteLine ($"**** {H[i]} ***");
					count = (count + CalcOne (H, tmpPlaces, replacements, node));
					if (Mdb.ConsoleApp) Mdb.ResultsDictionary.Add (i, count);
				}
				return (int)(count % norma);
			}
			private long CalcOne (int[] h, NodePositionType[] tmpPlaces, int[] replacements, int nodeVal)
			{
				int dimension = h.Length;
				var ways = new long[dimension];
				long sumL = 0, sumR = 0;

				for (int i = dimension - 1; i >= 0; i--)
				{
					int rpm = replacements[i];
					var tmpP = tmpPlaces[rpm];
					if(tmpP!=NodePositionType.Node && h[rpm]== nodeVal) continue;
					switch (tmpP)
					{
						case NodePositionType.Left:
							ways[i] = 1 + sumR;
							for (int j = i + 1; j < dimension
								&& h[rpm] == h[replacements[j]]; j++)
								if (tmpP != tmpPlaces[replacements[j]]) ways[i] -= ways[j];
							sumL += ways[i];
							break;
						case NodePositionType.Right:
							ways[i] = 1 + sumL;
							for (int j = i + 1;
								j < dimension
								&& h[rpm] == h[replacements[j]]; j++)
								if (tmpP != tmpPlaces[replacements[j]]) ways[i] -= ways[j];
							sumR += ways[i];
							break;
						case NodePositionType.None:
							return sumL + sumR + 1;
						case NodePositionType.Node:
							ways[i] = sumL + sumR + 1;
							return ways[i];
							break;
					}
					if (Mdb.Print && Mdb.ConsoleApp) Console.WriteLine ($"From {h[replacements[i]]}({tmpPlaces[replacements[i]]}): {ways[i]}");
				}
				return ways[0];
			}
		}

		


		}
	}