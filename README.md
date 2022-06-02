# BPSolar
Practical part of bachelor project Photovoltaic Power Plant Prediction of Production available at: [bpsolar.azurewebsites.net](https://bpsolar.azurewebsites.net/)
# Config power plant
### Location configuration:
The position is needed to correctly determine the weather forecast.
1. Latitude and Longitude<br/>
  -Manual Add<br/>
  -Autofind using Geolocation API<br/>
  -Use map to generate location<br/>
2. City name and state ISO
### Power plant configuration:
Data that must be filled is:<br/>
1.Panel orientation(-180°-180°)<br/>
2.Panel tilt(0°-180°)<br/>
3.Maximal power output of power plant(W/h)<br/>
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/Location.png?raw=true)
# Visualization
Visualitation of forecast is separed into tree timezones and this is for following 3hour,7days or 7days hour by hour.<br>
For constrast ,there is also weather forecast showed for specified timezones.
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/Visualization.png?raw=true)
# API
### Query Link 
In query link must be specified time and all atributes like in visualization.
Timezones:
1. 3HRS -forecast for following 3 hours.
2. 7DYS -forecast for folowing 7 days avarage.
3. WEEK -forecast for folowing 7 days hour by hour.
```
bpsolar.azurewebsites.net/api/3HRS?panel_tilt=45&panel_azimuth=10&wattpeek=95&longitude=18.218062&latitude=49.6380361
```
### Response
Response for query above.
```
{"hours":[
{"hour_num":"9","powerOutput":11.649465443292128},
{"hour_num":"10","powerOutput":12.934617461828141},
{"hour_num":"11","powerOutput":9.379211101974105}
]}
```
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/API.png?raw=true)
# Comparation
Comparation is made for one day with forecast.solar and for actual week with real power plant.<br>
More info is on the Comparation page.
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/comparation.png?raw=true)
# DataSources
Weather Forecast: [visualcrossing API](https://www.visualcrossing.com/) <br/>
Solar forecast for comparation : [forecast.solar](https://forecast.solar/)

