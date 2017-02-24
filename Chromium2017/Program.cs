using System;
using System.Collections.Generic;
using System.Linq;

namespace Chromium2017
	{
	public class Program
		{
		private static void Main(string[] args)
			{
			int[] s = {171, 185, 66, 157, 129, 50, 197, 12, 147};
			s = new[] {4, 6, 2, 1, 5};
			//s = new[] {13, 2, 5};
			var sol = new Solution();
			Console.WriteLine($"=={sol.solution(s)}");
			Console.Write($"=={sol.solutionStd(s)}");
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

			public int solution(int[] H)
				{
				long count = 0;
				int dimension = H.Length;
				for (int i = 0; i < dimension; i++)
				{
					long smallcount = 1;
					int node = H[i];
					//Form left and right arrays
					int leftPtr = 0, rightPtr = 0;
					int[] leftArray = new int[i];
					int[] rightArray = new int[dimension - i - 1];
					for (int j = 0; j < i; j++)
					{
						if (H[j] >= node) leftArray[leftPtr++] = H[j];
					}
					for (int j = i + 1; j < dimension; j++)
					{
						if (H[j] >= node) rightArray[rightPtr++] = H[j];
					}
					leftPtr--;
					rightPtr--;
					bool useLeft;
					if (leftPtr >= 0 && rightPtr >= 0) useLeft = leftArray[leftPtr] > rightArray[rightPtr];
					else useLeft = leftPtr > 0;
					while (leftPtr >= 0|| rightPtr >= 0)
					{
						int[] array = useLeft ? leftArray : rightArray;
						int[] otherarray = useLeft ? rightArray : leftArray;
						smallcount++;
						if (useLeft)
						{
							leftPtr--;
							if (leftPtr <0)
							{
								useLeft = false;
								continue;
							}
						}
						else
						{
							rightPtr--;
							if (rightPtr <0)
							{
								useLeft = true;
								continue;
							}
						}
						if (leftPtr<0 || rightPtr<0) continue;
						if (array[useLeft ? leftPtr : rightPtr] < otherarray[useLeft ? rightPtr : leftPtr]) useLeft = !useLeft;
					}
					count = (count + smallcount) % 1000000007;
				}
				return (int) (count % 1000000007);
				}
			}
		}
	}