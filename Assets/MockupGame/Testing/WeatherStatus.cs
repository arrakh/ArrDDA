
// Conditions explained: https://openweathermap.org/weather-conditions

public class WeatherStatus 
{
	public int weatherId;
	public string main;
	public string description;
	public float temperature; // in kelvin
	public float pressure;
	public float windSpeed;

	public float Celsius () {
		return temperature - 273.15f;
	}

	public float Fahrenheit () {
		return Celsius () * 9.0f / 5.0f + 32.0f;
	}
}