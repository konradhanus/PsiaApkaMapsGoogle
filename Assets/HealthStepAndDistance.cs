//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using BeliefEngine.HealthKit;
//public class HealthStepAndDistance : MonoBehaviour
//{

//	private HealthStore healthStore;
//	public Text day1ago;
//	public Text day2ago;
//	public Text day3ago;
//	public Text day4ago;
//	public Text day5ago;
//	public Text day6ago;
//	public Text day7ago;

//	public Text Sum;

//	public void ReadSteps()
//	{
//		//DateTimeOffset end = DateTimeOffset.UtcNow;
//		//DateTimeOffset start = end.AddDays(-1);

//		// Ustawiamy datę końcową na wczoraj o 23:59:59
//		DateTimeOffset end = DateTimeOffset.UtcNow.AddDays(-1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

//		// Ustawiamy datę początkową na wczoraj o 00:00:00
//		DateTimeOffset start = end.Date;

//		this.healthStore.ReadSteps(start, end, delegate (double steps, Error error) {
//			if (steps > 0)
//			{
//				this.resultsLabel.text += "total steps:" + steps + ": " + end.ToString() + " - " + start.ToString();
//			}
//			else
//			{
//				this.resultsLabel.text += "No steps during this period." + end.ToString() + " - " + start.ToString();
//			}

//			// all done
//			reading = false;
//		});
//	}

//	// 1. Metoda liczy kroki od podanej daty (od początku tego dnia) do końca dzisiejszego dnia (23:59:59)
//	public void ReadStepsFromDateToNow(DateTimeOffset startDate)
//	{
//		// Ustawiamy start na początek podanego dnia (00:00:00)
//		DateTimeOffset start = startDate.Date;
//		// Ustawiamy koniec na koniec dzisiejszego dnia (23:59:59)
//		DateTimeOffset end = DateTimeOffset.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

//		// Wyświetlamy zakresy dat w logach z nową linią
//		Debug.Log($"Reading steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n");

//		this.healthStore.ReadSteps(start, end, delegate (double steps, Error error) {
//			if (steps > 0)
//			{
//				this.resultsLabel.text += $"Total steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}: {steps}\n";
//			}
//			else
//			{
//				this.resultsLabel.text += $"No steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n";
//			}
//			reading = false;
//		});
//	}

//	// 2. Metoda liczy kroki z ostatniego tygodnia (każdy dzień od 00:00:00 do 23:59:59)
//	public void ReadStepsFromLastWeek()
//	{
//		// Ustawiamy start na początek dnia, 7 dni temu (00:00:00)
//		DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(-7).Date;
//		// Ustawiamy koniec na koniec dzisiejszego dnia (23:59:59)
//		DateTimeOffset end = DateTimeOffset.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

//		// Wyświetlamy zakresy dat w logach z nową linią
//		Debug.Log($"Reading steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n");

//		this.healthStore.ReadSteps(start, end, delegate (double steps, Error error) {
//			if (steps > 0)
//			{
//				this.resultsLabel.text += $"Total steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}: {steps}\n";
//			}
//			else
//			{
//				this.resultsLabel.text += $"No steps in the last week from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n";
//			}
//			reading = false;
//		});
//	}

//	// 3. Metoda liczy kroki dla konkretnego dnia wstecz (od 00:00:00 do 23:59:59)
//	public void ReadStepsForSpecificDayAgo(int daysAgo)
//	{
//		// Obliczamy koniec dnia dla odpowiedniego dnia wstecz (23:59:59)
//		DateTimeOffset end = DateTimeOffset.UtcNow.AddDays(-daysAgo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
//		// Ustawiamy początek na początek tego dnia (00:00:00)
//		DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(-daysAgo).Date.AddHours(0).AddMinutes(0).AddSeconds(0);

//		// Wyświetlamy zakresy dat w logach z nową linią
//		Debug.Log($"Reading steps for {daysAgo} days ago: from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n");

//		this.healthStore.ReadSteps(start, end, delegate (double steps, Error error) {
//			if (steps > 0)
//			{
//				this.resultsLabel.text += $"Total steps for {daysAgo} days ago: from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}: {steps}\n";
//			}
//			else
//			{
//				this.resultsLabel.text += $"No steps recorded {daysAgo} days ago: from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n";
//			}
//			reading = false;
//		});
//	}


//	public void Wczoraj()
//	{

//		ReadStepsForSpecificDayAgo(1);
//	}

//	public void Przedwczoraj()
//	{

//		ReadStepsForSpecificDayAgo(2);
//	}

//	public void TrzyDniTemu()
//	{

//		ReadStepsForSpecificDayAgo(3);
//	}

//	public void CzteryDniTemu()
//	{

//		ReadStepsForSpecificDayAgo(4);
//	}

//	public void PiecDniTemu()
//	{

//		ReadStepsForSpecificDayAgo(5);
//	}

//	public void SzescDniTemu()
//	{

//		ReadStepsForSpecificDayAgo(6);
//	}

//	public void SiedemDniTemu()
//	{

//		ReadStepsForSpecificDayAgo(7);
//	}
//}
