using System;

namespace MyThirdSDL
{
	public static class DateTimeHelper
	{
		public static readonly int DaysInAYear = 365;

		public static double DaysToYears(double days)
		{
			return days / DaysInAYear;
		}

		public static double DaysRemainderInYears(double years)
		{
			return (years - Math.Truncate(years)) * DateTimeHelper.DaysInAYear;
		}
	}
}

