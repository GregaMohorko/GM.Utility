using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class StatisticUtility
	{
		/// <summary>
		/// Calculates the standard deviation of the provided values.
		/// <para>
		/// https://www.khanacademy.org/math/probability/data-distributions-a1/summarizing-spread-distributions/a/calculating-standard-deviation-step-by-step
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		public static double CalculateStandardDeviation(IEnumerable<double> values)
		{
			int count = values.Count();
			double mean = values.Sum() / count;
			double standardDeviation = values.Sum(v => Math.Pow(Math.Abs(v - mean), 2));
			standardDeviation = Math.Sqrt(standardDeviation / count);
			return standardDeviation;
		}

		/// <summary>
		/// Finds the median, which is a value separating the higher half of the values from the lower half.
		/// </summary>
		/// <param name="values">The values.</param>
		public static double FindMedianValue(IEnumerable<double> values)
		{
			int count = values.Count();
			int center = count / 2;
			if(count % 2 != 0) {
				// is the value at the center
				return values.ElementAt(center);
			}

			// median is the average between the two values at the center
			double left = values.ElementAt(center - 1);
			double right = values.ElementAt(center);
			return (left + right) / 2;
		}

		/// <summary>
		/// Returns the quartile values of a set of doubles. The provided list will be ordered. If you already have an ordered list, please use method <see cref="FindQuartilesOfOrdered(IEnumerable{double})"/>.
		/// <para>
		/// https://en.wikipedia.org/wiki/Quartile
		/// </para>
		/// <para>
		/// This actually turns out to be a bit of a PITA, because there is no universal agreement
		/// on choosing the quartile values. In the case of odd values, some count the median value
		/// in finding the 1st and 3rd quartile and some discard the median value.
		/// the two different methods result in two different answers.
		/// The below method produces the arithmatic mean of the two methods, and insures the median
		/// is given it's correct weight so that the median changes as smoothly as possible as
		/// more data ppints are added.
		/// </para>
		/// <para>
		/// This method uses the following logic:
		/// </para>
		/// <para>
		/// If there are an even number of data points:
		///    Use the median to divide the ordered data set into two halves.
		///    The lower quartile value is the median of the lower half of the data.
		///    The upper quartile value is the median of the upper half of the data.
		/// </para>
		/// <para>
		/// If there are (4n+1) data points:
		///    The lower quartile is 25% of the nth data value plus 75% of the (n+1)th data value.
		///    The upper quartile is 75% of the (3n+1)th data point plus 25% of the (3n+2)th data point.
		/// </para>
		/// <para>
		/// If there are (4n+3) data points:
		///   The lower quartile is 75% of the (n+1)th data value plus 25% of the (n+2)th data value.
		///   The upper quartile is 25% of the (3n+2)th data point plus 75% of the (3n+3)th data point.
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		public static Tuple<double, double, double> FindQuartiles(IEnumerable<double> values)
		{
			return FindQuartilesOfOrdered(values.OrderBy(v => v));
		}

		/// <summary>
		/// Returns the quartile values of an ordered set of doubles. The provided list must be already ordered. If it is not, please use method <see cref="FindQuartiles(IEnumerable{double})"/>.
		/// <para>
		/// https://en.wikipedia.org/wiki/Quartile
		/// </para>
		/// <para>
		/// This actually turns out to be a bit of a PITA, because there is no universal agreement
		/// on choosing the quartile values. In the case of odd values, some count the median value
		/// in finding the 1st and 3rd quartile and some discard the median value.
		/// the two different methods result in two different answers.
		/// The below method produces the arithmatic mean of the two methods, and insures the median
		/// is given it's correct weight so that the median changes as smoothly as possible as
		/// more data ppints are added.
		/// </para>
		/// <para>
		/// This method uses the following logic:
		/// </para>
		/// <para>
		/// If there are an even number of data points:
		///    Use the median to divide the ordered data set into two halves.
		///    The lower quartile value is the median of the lower half of the data.
		///    The upper quartile value is the median of the upper half of the data.
		/// </para>
		/// <para>
		/// If there are (4n+1) data points:
		///    The lower quartile is 25% of the nth data value plus 75% of the (n+1)th data value.
		///    The upper quartile is 75% of the (3n+1)th data point plus 25% of the (3n+2)th data point.
		/// </para>
		/// <para>
		/// If there are (4n+3) data points:
		///   The lower quartile is 75% of the (n+1)th data value plus 25% of the (n+2)th data value.
		///   The upper quartile is 25% of the (3n+2)th data point plus 75% of the (3n+3)th data point.
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		public static Tuple<double, double, double> FindQuartilesOfOrdered(IEnumerable<double> values)
		{
			int count = values.Count();
			int center = count / 2; //this is the mid from a zero based index, eg mid of 7 = 3;

			double left, right;
			int centerLeft, centerRight;

			double Q1;
			double Q2;
			double Q3;

			if(count % 2 == 0) {
				//================ EVEN NUMBER OF POINTS: =====================
				//even between left and right point

				left = values.ElementAt(center - 1);
				right = values.ElementAt(center);
				Q2 = (left + right) / 2;

				centerLeft = center / 2;
				centerRight = center + centerLeft;

				//easy split
				if(center % 2 == 0) {
					left = values.ElementAt(centerLeft - 1);
					right = values.ElementAt(centerLeft);
					Q1 = (left + right) / 2;
					left = values.ElementAt(centerRight - 1);
					right = values.ElementAt(centerRight);
					Q3 = (left + right) / 2;
				} else {
					Q1 = values.ElementAt(centerLeft);
					Q3 = values.ElementAt(centerRight);
				}
			} else if(count == 1) {
				//================= special case, sorry ================
				Q1 = Q2 = Q3 = values.First();
			} else {
				//odd number so the median is just the midpoint in the array
				Q2 = values.ElementAt(center);

				if((count - 1) % 4 == 0) {
					//======================(4n-1) POINTS =========================
					centerLeft = (count - 1) / 4;
					centerRight = centerLeft * 3;
					left = values.ElementAt(centerLeft - 1);
					right = values.ElementAt(centerLeft);
					Q1 = (left * 0.25) + (right * 0.75);
					left = values.ElementAt(centerRight);
					right = values.ElementAt(centerRight + 1);
					Q3 = (left * 0.75) + (right * 0.25);
				} else {
					//======================(4n-3) POINTS =========================
					centerLeft = (count - 3) / 4;
					centerRight = centerLeft * 3 + 1;
					left = values.ElementAt(centerLeft);
					right = values.ElementAt(centerLeft + 1);
					Q1 = (left * 0.75) + (right * 0.25);
					left = values.ElementAt(centerRight);
					right = values.ElementAt(centerRight + 1);
					Q3 = (left * 0.25) + (right * 0.75);
				}
			}

			return Tuple.Create(Q1, Q2, Q3);
		}
	}
}
