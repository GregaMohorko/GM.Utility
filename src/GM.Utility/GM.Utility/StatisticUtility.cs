/*
MIT License

Copyright (c) 2019 Grega Mohorko

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Project: GM.Utility
Created: 2017-10-27
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Statistic utilities.
	/// </summary>
	public static class StatisticUtility
	{
		/// <summary>
		/// Calculates the coefficient of variation (CV), also known as relative standard deviation (RSD), of the provided values. It shows the extent of variability in relation to the mean of the population.
		/// <para>
		/// https://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </para>
		/// <para>This method is equivalent to <see cref="CalculateRSD(IEnumerable{double}, bool, bool)"/>.</para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		/// <param name="asPercentage">If true, the result will be multiplied by 100 and will represent a percentage.</param>
		public static double CalculateCV(IEnumerable<double> values, bool forSample = false, bool asPercentage = false)
		{
			return CalculateRSD(values, forSample, asPercentage);
		}

		/// <summary>
		/// Calculates the coefficient of variation (CV), also known as relative standard deviation (RSD), of the provided values. It shows the extent of variability in relation to the mean of the population.
		/// <para>
		/// https://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </para>
		/// <para>This method is equivalent to <see cref="CalculateRSD(IEnumerable{decimal}, bool, bool)"/>.</para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		/// <param name="asPercentage">If true, the result will be multiplied by 100 and will represent a percentage.</param>
		public static decimal CalculateCV(IEnumerable<decimal> values, bool forSample = false, bool asPercentage = false)
		{
			return CalculateRSD(values, forSample, asPercentage);
		}

		/// <summary>
		/// Calculates the mean value of the provided values.
		/// </summary>
		/// <param name="values">The values.</param>
		public static double CalculateMean(ICollection<double> values)
		{
			int count = values.Count();
			if(count == 0) {
				return 0;
			}
			return values.Sum() / count;
		}

		/// <summary>
		/// Calculates the mean value of the provided values.
		/// </summary>
		/// <param name="values">The values.</param>
		public static decimal CalculateMean(ICollection<decimal> values)
		{
			int count = values.Count();
			if(count == 0) {
				return 0;
			}
			return values.Sum() / count;
		}

		/// <summary>
		/// Calculates the linear regression line and returns the a and b parameters of the line. The equation for the line itself is: y = x*a + b.
		/// <para>
		/// https://en.wikipedia.org/wiki/Simple_linear_regression
		/// </para>
		/// </summary>
		/// <param name="x">Values of the independant (X-axis) variable.</param>
		/// <param name="y">Values of the dependant (Y-axis) variable.</param>
		public static Tuple<double, double> CalculateSimpleLinearRegressionLine(IEnumerable<double> x, IEnumerable<double> y)
		{
			int n;
			{
				int xCount = x.Count();
				int yCount = y.Count();

				if(xCount != yCount) {
					throw new ArgumentException("The count of both value collections must be the same.");
				}
				n = xCount;
			}

			double xMean = x.Sum() / n;
			double yMean = y.Sum() / n;

			double a;
			{
				double aTop = 0;
				double aBottom = 0;
				for(int i = n - 1; i >= 0; --i) {
					double xi = x.ElementAt(i);
					double yi = y.ElementAt(i);

					double tmp = xi - xMean;

					aTop += tmp * (yi - yMean);
					aBottom += Math.Pow(tmp, 2);
				}
				a = aTop / aBottom;
			}

			double b = yMean - a * xMean;

			return Tuple.Create(a, b);
		}

		/// <summary>
		/// Calculates the sample correlation coefficient (also called "R squared") between x and y values. Sample correlation coefficient is the proportion of the variance in the dependent variable (x) that is predictable from the independent variable (y).
		/// <para>
		/// https://en.wikipedia.org/wiki/Correlation_and_dependence#Pearson.27s_product-moment_coefficient
		/// </para>
		/// </summary>
		/// <param name="x">Values of the independant (X-axis) variable.</param>
		/// <param name="y">Values of the dependant (Y-axis) variable.</param>
		public static double CalculateR2(IEnumerable<double> x, IEnumerable<double> y)
		{
			int n;
			{
				int xCount = x.Count();
				int yCount = y.Count();

				if(xCount != yCount) {
					throw new ArgumentException("The count of both value collections must be the same.");
				}
				n = xCount;
			}

			double xMean = 0;
			double yMean = 0;
			double xyMean = 0;
			double x2Mean = 0;
			double y2Mean = 0;
			for(int i = n - 1; i >= 0; --i) {
				double xi = x.ElementAt(i);
				double yi = y.ElementAt(i);

				xMean += xi;
				yMean += yi;
				xyMean += xi * yi;
				x2Mean += xi * xi;
				y2Mean += yi * yi;
			}
			xMean /= n;
			yMean /= n;
			xyMean /= n;
			x2Mean /= n;
			y2Mean /= n;

			return Math.Pow((xyMean - xMean * yMean), 2) / ((x2Mean - xMean * xMean) * (y2Mean - yMean * yMean));
		}

		/// <summary>
		/// Calculates the relative standard deviation (RSD), also known as coefficient of variation (CV), of the provided values. It shows the extent of variability in relation to the mean of the population.
		/// <para>
		/// https://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		/// <param name="asPercentage">If true, the result will be multiplied by 100 and will represent a percentage.</param>
		public static double CalculateRSD(IEnumerable<double> values, bool forSample = false, bool asPercentage = false)
		{
			return CalculateRSD(values, out double mean, forSample, asPercentage);
		}

		/// <summary>
		/// Calculates the relative standard deviation (RSD), also known as coefficient of variation (CV), of the provided values. It shows the extent of variability in relation to the mean of the population.
		/// <para>
		/// https://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		/// <param name="asPercentage">If true, the result will be multiplied by 100 and will represent a percentage.</param>
		public static decimal CalculateRSD(IEnumerable<decimal> values, bool forSample = false, bool asPercentage = false)
		{
			return CalculateRSD(values, out decimal mean, forSample, asPercentage);
		}

		/// <summary>
		/// Calculates the relative standard deviation (RSD), also known as coefficient of variation (CV), of the provided values. It shows the extent of variability in relation to the mean of the population.
		/// <para>
		/// https://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="mean">The mean value that is calculated during the calculation (in case you need it).</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		/// <param name="asPercentage">If true, the result will be multiplied by 100 and will represent a percentage.</param>
		public static double CalculateRSD(IEnumerable<double> values, out double mean, bool forSample = false, bool asPercentage = false)
		{
			double sd = CalculateStandardDeviation(values, out mean, forSample);
			if(mean == 0) {
				return 0;
			}
			if(asPercentage) {
				sd *= 100;
			}
			return sd / mean;
		}

		/// <summary>
		/// Calculates the relative standard deviation (RSD), also known as coefficient of variation (CV), of the provided values. It shows the extent of variability in relation to the mean of the population.
		/// <para>
		/// https://en.wikipedia.org/wiki/Coefficient_of_variation
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="mean">The mean value that is calculated during the calculation (in case you need it).</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		/// <param name="asPercentage">If true, the result will be multiplied by 100 and will represent a percentage.</param>
		public static decimal CalculateRSD(IEnumerable<decimal> values, out decimal mean, bool forSample = false, bool asPercentage = false)
		{
			decimal sd = CalculateStandardDeviation(values, out mean, forSample);
			if(mean == 0) {
				return 0;
			}
			if(asPercentage) {
				sd *= 100;
			}
			return sd / mean;
		}

		/// <summary>
		/// Calculates the standard deviation of the provided values.
		/// <para>
		/// https://www.khanacademy.org/math/probability/data-distributions-a1/summarizing-spread-distributions/a/calculating-standard-deviation-step-by-step
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		public static double CalculateStandardDeviation(IEnumerable<double> values, bool forSample = false)
		{
			return CalculateStandardDeviation(values, out double mean, forSample);
		}

		/// <summary>
		/// Calculates the standard deviation of the provided values.
		/// <para>
		/// https://www.khanacademy.org/math/probability/data-distributions-a1/summarizing-spread-distributions/a/calculating-standard-deviation-step-by-step
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		public static decimal CalculateStandardDeviation(IEnumerable<decimal> values, bool forSample = false)
		{
			return CalculateStandardDeviation(values, out decimal mean, forSample);
		}

		/// <summary>
		/// Calculates the standard deviation of the provided values.
		/// <para>
		/// https://www.khanacademy.org/math/probability/data-distributions-a1/summarizing-spread-distributions/a/calculating-standard-deviation-step-by-step
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="mean">The mean value that is calculated during the calculation (in case you need it).</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		public static double CalculateStandardDeviation(IEnumerable<double> values, out double mean, bool forSample = false)
		{
			int count = values.Count();
			if(count == 0) {
				mean = 0;
				return 0;
			}
			mean = values.Sum() / count;
			double m = mean;
			double standardDeviation = values.Sum(v => Math.Pow(Math.Abs(v - m), 2));
			if(forSample) {
				--count;
			}
			standardDeviation = Math.Sqrt(standardDeviation / count);
			return standardDeviation;
		}

		/// <summary>
		/// Calculates the standard deviation of the provided values.
		/// <para>
		/// https://www.khanacademy.org/math/probability/data-distributions-a1/summarizing-spread-distributions/a/calculating-standard-deviation-step-by-step
		/// </para>
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="mean">The mean value that is calculated during the calculation (in case you need it).</param>
		/// <param name="forSample">The default formula is for finding the standard deviation of a population. If you're dealing with a sample, you'll want to use a slightly different formula, which uses n-1 instead of N.</param>
		public static decimal CalculateStandardDeviation(IEnumerable<decimal> values, out decimal mean, bool forSample = false)
		{
			int count = values.Count();
			if(count == 0) {
				mean = 0;
				return 0;
			}
			mean = values.Sum() / count;
			decimal m = mean;
			decimal standardDeviation = values.Sum(v => (decimal)Math.Pow((double)(v - m), 2));
			if(forSample) {
				--count;
			}
			standardDeviation /= count;
			standardDeviation = (decimal)Math.Sqrt((double)standardDeviation);
			return standardDeviation;
		}

		/// <summary>
		/// Difference (in percentage 0-100) of this decimal to the specified decimal.
		/// <para>If the result is negative, it means that this decimal is smaller than the specified decimal.</para>
		/// <para>If the specified decimal is zero, <see cref="decimal.MaxValue"/> is returned.</para>
		/// </summary>
		/// <param name="d1">This decimal.</param>
		/// <param name="d2">The decimal to compare to.</param>
		public static decimal DifferenceInPercentage(this decimal d1, decimal d2)
		{
			if(d2 == 0m) {
				return decimal.MaxValue;
			}
			return 100 * (-1m + (d1 / d2));
		}

		/// <summary>
		/// Find the median, which is a value separating the higher half of the values from the lower half.
		/// </summary>
		/// <param name="values">The values.</param>
		public static double FindMedianValue(IEnumerable<int> values)
		{
			return FindMedianValue(values.Select(i => (double)i));
		}

		/// <summary>
		/// Finds the median, which is a value separating the higher half of the values from the lower half.
		/// </summary>
		/// <param name="values">The values.</param>
		public static double FindMedianValue(IEnumerable<double> values)
		{
			List<double> orderedList = values.OrderBy(v => v).ToList();
			int center = orderedList.Count / 2;
			if(orderedList.Count % 2 != 0) {
				// is the value at the center
				return orderedList[center];
			}

			// median is the average between the two values at the center
			double left = orderedList[center - 1];
			double right = orderedList[center];
			return (left + right) / 2d;
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
