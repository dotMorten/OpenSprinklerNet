using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OpenSprinklerApp
{
    public sealed partial class CalendarControl : UserControl
    {
		public class Week
		{
			public Week(DateTime week)
			{
				DateTime w = new DateTime(week.Year, week.Month, week.Day);
				if(w.DayOfWeek == DayOfWeek.Sunday)
					w = w.AddDays(-7);
				else
					w = w.AddDays(-((int)w.DayOfWeek)+1);
				Start = w;
				Year = w.Year;
				MonthName = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[w.Month - 1] + " " + Year.ToString();
				Days = new CalendarDayItem[] {
					new CalendarDayItem(w),
					new CalendarDayItem(w.AddDays(1)),
					new CalendarDayItem(w.AddDays(2)),
					new CalendarDayItem(w.AddDays(3)),
					new CalendarDayItem(w.AddDays(4)),
					new CalendarDayItem(w.AddDays(5)),
					new CalendarDayItem(w.AddDays(6))
				};
			}
			public string MonthName { get; set; }
			public int Year { get; set; }
			public DateTime Start { get; set; }

			public CalendarDayItem[] Days { get; set; }
		}

		public class CalendarDayItem
		{
			public CalendarDayItem(DateTime day)
			{
				Day = day;
				Entries = new List<OpenSprinklerApp.CalendarDayPanel.Entry>();
			}
			public DateTime Day { get; set; }
			public IList<OpenSprinklerApp.CalendarDayPanel.Entry> Entries { get; set; }
		}

		private ObservableCollection<Week> weeks = new ObservableCollection<Week>();

		public CalendarControl()
        {
            this.InitializeComponent();
			LoadWeeks();
		}

		private void LoadWeeks()
		{
			weeks = new ObservableCollection<Week>();
			var now = DateTime.Now;
			now = new DateTime(now.Year, now.Month, now.Day);
			for (int i = 0; i < 2; i++)
			{
				InsertWeek(now.AddDays(i * 7));	
			}
			flipview.ItemsSource = weeks;
			flipview.SelectedIndex = 1;
        }

		private void flipview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (flipview.SelectedIndex >= weeks.Count - 2)
				InsertWeek(weeks.Last().Start.AddDays(7));
			else if(flipview.SelectedIndex == 0)
				InsertWeek(weeks[0].Start.AddDays(-7), 0);
		}

		private void InsertWeek(DateTime start, int index = -1)
		{
			var week = new Week(start);
			week.Days[0].Entries = AddCalendarEntries(week, DayOfWeek.Monday);
			week.Days[1].Entries = AddCalendarEntries(week, DayOfWeek.Tuesday);
			week.Days[2].Entries = AddCalendarEntries(week, DayOfWeek.Wednesday);
			week.Days[3].Entries = AddCalendarEntries(week, DayOfWeek.Thursday);
			week.Days[4].Entries = AddCalendarEntries(week, DayOfWeek.Friday);
			week.Days[5].Entries = AddCalendarEntries(week, DayOfWeek.Saturday);
			week.Days[6].Entries = AddCalendarEntries(week, DayOfWeek.Sunday);
			if (index == -1)
				weeks.Add(week);
			else
				weeks.Insert(index, week);
		}

		private Color[] StationColors = new Color[] {
			Colors.Red, Colors.Green, Colors.Blue, Colors.Orange, Colors.Purple, Colors.Brown
		};
		private IList<OpenSprinklerApp.CalendarDayPanel.Entry> AddCalendarEntries(Week week, DayOfWeek dayOfWeek)
		{
			if (Programs == null)
				return new List<OpenSprinklerApp.CalendarDayPanel.Entry>();
			var day = new List<OpenSprinklerApp.CalendarDayPanel.Entry>();
			var d = week.Start.AddDays((dayOfWeek == DayOfWeek.Sunday ? 6 : ((int)dayOfWeek - 1))).Day;
			int i = 1;
			TimeSpan lastEndTime = TimeSpan.Zero;
			foreach (var program in Programs)
			{
				var e = new OpenSprinklerApp.CalendarDayPanel.Entry();
				e.Title = i.ToString();
				if(!program.Enabled)
				{
					e.Color = Colors.Gray;
				}
				else
				{
					e.Color = StationColors[(i - 1) % StationColors.Length];
				}
				e.Start = program.Start;
				e.Duration = program.Duration;
				if (lastEndTime > e.Start && IsSequential)
					e.Start = lastEndTime;
				if(program.SkipDays == OpenSprinklerNet.SkipDays.None ||		
					program.SkipDays == OpenSprinklerNet.SkipDays.Odd && d % 2 == 1 ||
					program.SkipDays == OpenSprinklerNet.SkipDays.Even && d % 2 == 0)
				{
					day.Add(e);
					if(program.Enabled)
						lastEndTime = e.Start + e.Duration;
				}

				i++;
			}
			return day;

			day.Add(new CalendarDayPanel.Entry()
			{
				Title = "1",
				Color = Colors.Red,
				Start = TimeSpan.FromHours(2),
				Duration = TimeSpan.FromMinutes(15)
			});
			if (d % 2 == 1)
			{
				day.Add(new CalendarDayPanel.Entry()
				{
					Title = "2",
					Color = Colors.Green,
					Start = TimeSpan.FromHours(3.5),
					Duration = TimeSpan.FromMinutes(45)
				});
				day.Add(new CalendarDayPanel.Entry()
				{
					Title = "3",
					Color = Colors.Blue,
					Start = TimeSpan.FromHours(9),
					Duration = TimeSpan.FromMinutes(10)
				});
				day.Add(new CalendarDayPanel.Entry()
				{
					Title = "4",
					Color = Colors.Orange,
					Start = TimeSpan.FromHours(12),
					Duration = TimeSpan.FromHours(1)
				});
			}
			day.Add(new CalendarDayPanel.Entry()
			{
				Title = "5",
				Color = Colors.Blue,
				Start = TimeSpan.FromHours(20),
				Duration = TimeSpan.FromHours(3)
			});
			return day;
		}

		public IEnumerable<OpenSprinklerNet.Program> Programs
		{
			get { return (IEnumerable<OpenSprinklerNet.Program>)GetValue(ProgramsProperty); }
			set { SetValue(ProgramsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Appointments.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ProgramsProperty =
			DependencyProperty.Register("Programs", typeof(IEnumerable<OpenSprinklerNet.Program>), typeof(CalendarControl), new PropertyMetadata(null, OnProgramsPropertyChanged));

		private static void OnProgramsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CalendarControl)d).LoadWeeks();
		}

		public bool IsSequential
		{
			get { return (bool)GetValue(IsSequentialProperty); }
			set { SetValue(IsSequentialProperty, value); }
		}

		public static readonly DependencyProperty IsSequentialProperty =
			DependencyProperty.Register("IsSequential", typeof(bool), typeof(CalendarControl), new PropertyMetadata(true, OnProgramsPropertyChanged));

    }
}
