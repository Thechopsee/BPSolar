# BPSolar
Practical part of bachelor project Photovoltaic Power Plant Prediction of Production available at: [bpsolar.azurewebsites.net](https://bpsolar.azurewebsites.net/)
# Config power plant
### Location configuration:
The position is needed to correctly determine the weather forecast.
1. Latitude and Longitude
⋅⋅1. Manual Add
⋅⋅2. Autofind using Geolocation API
⋅⋅3. Use map to generate location
2. City name and state ISO
### Power plant configuration:
Data that must be filled is:
1.Panel orientation(-180°-180°)
2.Panel tilt(0°-180°)
3.Maximal power output of power plant(W/h)
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/Location.png?raw=true)
# Visualization
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/Visualization.png?raw=true)
# API
### Link 
```
bpsolar.azurewebsites.net/api/3HRS?panel_tilt=45&panel_azimuth=10&wattpeek=95&longitude=18.218062&latitude=49.6380361
```
### Response
```
{"hours":[{"hour_num":"9","powerOutput":11.649465443292128},
{"hour_num":"10","powerOutput":12.934617461828141},
{"hour_num":"11","powerOutput":9.379211101974105}]}
```
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/API.png?raw=true)
# Comparation
![alt text](https://github.com/Thechopsee/BPSolar/blob/main/img/comparation.png?raw=true)
# DataSources
Weather Forecast: [visualcrossing API](https://www.visualcrossing.com/)
Solar forecast for comparation : [forecast.solar](https://forecast.solar/)

