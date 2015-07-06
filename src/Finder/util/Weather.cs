using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finder.util
{
    public class Weather
    {
        public delegate void WeatherEventHandler(string[] weatherData);
        public WeatherEventHandler weh;
        public event WeatherEventHandler WeatherCallBack
        {
            add { weh += new WeatherEventHandler(value); }
            remove { weh -= new WeatherEventHandler(value); }
        }
        //本机外网IP所在城市
        private string city;

        public Weather()
        {
            GetCityFromIp();
        }
        /// <summary>
        /// 根据本机外网IP获取所在城市
        /// </summary>
        private void GetCityFromIp()
        {
            try
            {
                string ipinfo = Comm.GetIpInfo();
                if (ipinfo != null)
                {
                    string[] t = ipinfo.Split('：');
                    string _city = t[2].Substring(0, t[2].IndexOf(" "));
                    int start = _city.IndexOf("省") + 1;
                    int end = _city.Length - start - 1;
                    city = _city.Substring(start, end);
                }
            }
            catch (Exception e)
            {
                city = null;
            }
        }
        public void ReadWeather()
        {
            try
            {
                string[] r = null;
                if (city != null)
                {
                    CityWeather.WeatherWebServiceSoapClient c = new CityWeather.WeatherWebServiceSoapClient("WeatherWebServiceSoap");
                    r = c.getWeatherbyCityName(city);
                }
                if (weh != null)
                {
                    weh(r);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
