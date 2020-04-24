using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsScript : MonoBehaviour {

    public LineRenderer line;
    public LineRenderer line2;
    public LineRenderer line3;
    public LineRenderer lineinv1;
    public LineRenderer lineinv2;

    string JsonTextString = 
        @"{
            ""Meta Data"": {
                ""1. Information"": ""Monthly Adjusted Prices and Volumes"",
                ""2. Symbol"": ""FB"",
                ""3. Last Refreshed"": ""2018-11-09"",
                ""4. Time Zone"": ""US/Eastern""
            },
            ""Monthly Adjusted Time Series"": {
                ""2018-11-09"": {
                    ""1. open"": ""151.5200"",
                    ""2. high"": ""154.1300"",
                    ""3. low"": ""144.0700"",
                    ""4. close"": ""144.9600"",
                    ""5. adjusted close"": ""144.9600"",
                    ""6. volume"": ""146314943"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-10-31"": {
                    ""1. open"": ""163.0300"",
                    ""2. high"": ""165.8800"",
                    ""3. low"": ""139.0300"",
                    ""4. close"": ""151.7900"",
                    ""5. adjusted close"": ""151.7900"",
                    ""6. volume"": ""622446235"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-09-28"": {
                    ""1. open"": ""173.5000"",
                    ""2. high"": ""173.8900"",
                    ""3. low"": ""158.8656"",
                    ""4. close"": ""164.4600"",
                    ""5. adjusted close"": ""164.4600"",
                    ""6. volume"": ""500468912"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-08-31"": {
                    ""1. open"": ""173.9300"",
                    ""2. high"": ""188.3000"",
                    ""3. low"": ""170.2700"",
                    ""4. close"": ""175.7300"",
                    ""5. adjusted close"": ""175.7300"",
                    ""6. volume"": ""549016789"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-07-31"": {
                    ""1. open"": ""193.3700"",
                    ""2. high"": ""218.6200"",
                    ""3. low"": ""166.5600"",
                    ""4. close"": ""172.5800"",
                    ""5. adjusted close"": ""172.5800"",
                    ""6. volume"": ""652763259"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-06-29"": {
                    ""1. open"": ""193.0650"",
                    ""2. high"": ""203.5500"",
                    ""3. low"": ""186.4300"",
                    ""4. close"": ""194.3200"",
                    ""5. adjusted close"": ""194.3200"",
                    ""6. volume"": ""387265765"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-05-31"": {
                    ""1. open"": ""172.0000"",
                    ""2. high"": ""192.7200"",
                    ""3. low"": ""170.2300"",
                    ""4. close"": ""191.7800"",
                    ""5. adjusted close"": ""191.7800"",
                    ""6. volume"": ""401144183"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-04-30"": {
                    ""1. open"": ""157.8100"",
                    ""2. high"": ""177.1000"",
                    ""3. low"": ""150.5100"",
                    ""4. close"": ""172.0000"",
                    ""5. adjusted close"": ""172.0000"",
                    ""6. volume"": ""751130388"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-03-29"": {
                    ""1. open"": ""179.0100"",
                    ""2. high"": ""186.1000"",
                    ""3. low"": ""149.0200"",
                    ""4. close"": ""159.7900"",
                    ""5. adjusted close"": ""159.7900"",
                    ""6. volume"": ""983045332"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-02-28"": {
                    ""1. open"": ""188.2200"",
                    ""2. high"": ""195.3200"",
                    ""3. low"": ""167.1800"",
                    ""4. close"": ""178.3200"",
                    ""5. adjusted close"": ""178.3200"",
                    ""6. volume"": ""497869797"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2018-01-31"": {
                    ""1. open"": ""177.6800"",
                    ""2. high"": ""190.6600"",
                    ""3. low"": ""175.8000"",
                    ""4. close"": ""186.8900"",
                    ""5. adjusted close"": ""186.8900"",
                    ""6. volume"": ""464919779"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-12-29"": {
                    ""1. open"": ""176.0300"",
                    ""2. high"": ""182.2800"",
                    ""3. low"": ""169.0100"",
                    ""4. close"": ""176.4600"",
                    ""5. adjusted close"": ""176.4600"",
                    ""6. volume"": ""308647940"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-11-30"": {
                    ""1. open"": ""182.3600"",
                    ""2. high"": ""184.2500"",
                    ""3. low"": ""174.0000"",
                    ""4. close"": ""177.1800"",
                    ""5. adjusted close"": ""177.1800"",
                    ""6. volume"": ""335337226"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-10-31"": {
                    ""1. open"": ""171.3900"",
                    ""2. high"": ""180.8000"",
                    ""3. low"": ""168.2900"",
                    ""4. close"": ""180.0600"",
                    ""5. adjusted close"": ""180.0600"",
                    ""6. volume"": ""309836859"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-09-29"": {
                    ""1. open"": ""172.4000"",
                    ""2. high"": ""174.0000"",
                    ""3. low"": ""161.5600"",
                    ""4. close"": ""170.8700"",
                    ""5. adjusted close"": ""170.8700"",
                    ""6. volume"": ""300559928"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-08-31"": {
                    ""1. open"": ""169.8200"",
                    ""2. high"": ""173.0500"",
                    ""3. low"": ""165.0000"",
                    ""4. close"": ""171.9700"",
                    ""5. adjusted close"": ""171.9700"",
                    ""6. volume"": ""297781301"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-07-31"": {
                    ""1. open"": ""151.7200"",
                    ""2. high"": ""175.4900"",
                    ""3. low"": ""147.8000"",
                    ""4. close"": ""169.2500"",
                    ""5. adjusted close"": ""169.2500"",
                    ""6. volume"": ""402911091"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-06-30"": {
                    ""1. open"": ""151.7500"",
                    ""2. high"": ""156.5000"",
                    ""3. low"": ""144.5600"",
                    ""4. close"": ""150.9800"",
                    ""5. adjusted close"": ""150.9800"",
                    ""6. volume"": ""405705533"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-05-31"": {
                    ""1. open"": ""151.7400"",
                    ""2. high"": ""153.6000"",
                    ""3. low"": ""144.4200"",
                    ""4. close"": ""151.4600"",
                    ""5. adjusted close"": ""151.4600"",
                    ""6. volume"": ""390036541"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-04-28"": {
                    ""1. open"": ""141.9300"",
                    ""2. high"": ""151.5300"",
                    ""3. low"": ""138.8100"",
                    ""4. close"": ""150.2500"",
                    ""5. adjusted close"": ""150.2500"",
                    ""6. volume"": ""273330661"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-03-31"": {
                    ""1. open"": ""136.4700"",
                    ""2. high"": ""142.9500"",
                    ""3. low"": ""136.0800"",
                    ""4. close"": ""142.0500"",
                    ""5. adjusted close"": ""142.0500"",
                    ""6. volume"": ""342098632"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-02-28"": {
                    ""1. open"": ""132.2500"",
                    ""2. high"": ""137.1800"",
                    ""3. low"": ""130.3000"",
                    ""4. close"": ""135.5400"",
                    ""5. adjusted close"": ""135.5400"",
                    ""6. volume"": ""384700625"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2017-01-31"": {
                    ""1. open"": ""116.0300"",
                    ""2. high"": ""133.1400"",
                    ""3. low"": ""115.5100"",
                    ""4. close"": ""130.3200"",
                    ""5. adjusted close"": ""130.3200"",
                    ""6. volume"": ""379004804"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-12-30"": {
                    ""1. open"": ""118.3800"",
                    ""2. high"": ""122.5000"",
                    ""3. low"": ""114.0000"",
                    ""4. close"": ""115.0500"",
                    ""5. adjusted close"": ""115.0500"",
                    ""6. volume"": ""408728358"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-11-30"": {
                    ""1. open"": ""131.4100"",
                    ""2. high"": ""131.9400"",
                    ""3. low"": ""113.5535"",
                    ""4. close"": ""118.4200"",
                    ""5. adjusted close"": ""118.4200"",
                    ""6. volume"": ""654602125"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-10-31"": {
                    ""1. open"": ""128.3800"",
                    ""2. high"": ""133.5000"",
                    ""3. low"": ""126.7500"",
                    ""4. close"": ""130.9900"",
                    ""5. adjusted close"": ""130.9900"",
                    ""6. volume"": ""313284563"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-09-30"": {
                    ""1. open"": ""126.3800"",
                    ""2. high"": ""131.9800"",
                    ""3. low"": ""125.6000"",
                    ""4. close"": ""128.2700"",
                    ""5. adjusted close"": ""128.2700"",
                    ""6. volume"": ""376582063"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-08-31"": {
                    ""1. open"": ""123.8500"",
                    ""2. high"": ""126.7300"",
                    ""3. low"": ""122.0700"",
                    ""4. close"": ""126.1200"",
                    ""5. adjusted close"": ""126.1200"",
                    ""6. volume"": ""365670913"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-07-29"": {
                    ""1. open"": ""114.2000"",
                    ""2. high"": ""128.3300"",
                    ""3. low"": ""112.9700"",
                    ""4. close"": ""123.9400"",
                    ""5. adjusted close"": ""123.9400"",
                    ""6. volume"": ""470108473"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-06-30"": {
                    ""1. open"": ""118.5000"",
                    ""2. high"": ""119.4400"",
                    ""3. low"": ""108.2300"",
                    ""4. close"": ""114.2800"",
                    ""5. adjusted close"": ""114.2800"",
                    ""6. volume"": ""451155442"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-05-31"": {
                    ""1. open"": ""117.8300"",
                    ""2. high"": ""121.0800"",
                    ""3. low"": ""114.8000"",
                    ""4. close"": ""118.8100"",
                    ""5. adjusted close"": ""118.8100"",
                    ""6. volume"": ""460559986"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-04-29"": {
                    ""1. open"": ""113.7500"",
                    ""2. high"": ""120.7900"",
                    ""3. low"": ""106.3100"",
                    ""4. close"": ""117.5800"",
                    ""5. adjusted close"": ""117.5800"",
                    ""6. volume"": ""741613866"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-03-31"": {
                    ""1. open"": ""107.8300"",
                    ""2. high"": ""116.9900"",
                    ""3. low"": ""104.4000"",
                    ""4. close"": ""114.1000"",
                    ""5. adjusted close"": ""114.1000"",
                    ""6. volume"": ""521143963"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-02-29"": {
                    ""1. open"": ""112.2700"",
                    ""2. high"": ""117.5900"",
                    ""3. low"": ""96.8200"",
                    ""4. close"": ""106.9200"",
                    ""5. adjusted close"": ""106.9200"",
                    ""6. volume"": ""874152635"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2016-01-29"": {
                    ""1. open"": ""101.9500"",
                    ""2. high"": ""112.8400"",
                    ""3. low"": ""89.3700"",
                    ""4. close"": ""112.2100"",
                    ""5. adjusted close"": ""112.2100"",
                    ""6. volume"": ""792709100"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-12-31"": {
                    ""1. open"": ""104.8300"",
                    ""2. high"": ""107.9200"",
                    ""3. low"": ""101.4600"",
                    ""4. close"": ""104.6600"",
                    ""5. adjusted close"": ""104.6600"",
                    ""6. volume"": ""440479804"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-11-30"": {
                    ""1. open"": ""102.4600"",
                    ""2. high"": ""110.6500"",
                    ""3. low"": ""100.4700"",
                    ""4. close"": ""104.2400"",
                    ""5. adjusted close"": ""104.2400"",
                    ""6. volume"": ""547199822"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-10-30"": {
                    ""1. open"": ""90.0500"",
                    ""2. high"": ""105.1200"",
                    ""3. low"": ""88.3600"",
                    ""4. close"": ""101.9700"",
                    ""5. adjusted close"": ""101.9700"",
                    ""6. volume"": ""571991600"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-09-30"": {
                    ""1. open"": ""86.9900"",
                    ""2. high"": ""96.4900"",
                    ""3. low"": ""85.7200"",
                    ""4. close"": ""89.9000"",
                    ""5. adjusted close"": ""89.9000"",
                    ""6. volume"": ""634104916"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-08-31"": {
                    ""1. open"": ""93.5300"",
                    ""2. high"": ""98.7400"",
                    ""3. low"": ""72.0000"",
                    ""4. close"": ""89.4300"",
                    ""5. adjusted close"": ""89.4300"",
                    ""6. volume"": ""709770559"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-07-31"": {
                    ""1. open"": ""86.7700"",
                    ""2. high"": ""99.2400"",
                    ""3. low"": ""85.2310"",
                    ""4. close"": ""94.0100"",
                    ""5. adjusted close"": ""94.0100"",
                    ""6. volume"": ""790781334"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-06-30"": {
                    ""1. open"": ""79.3000"",
                    ""2. high"": ""89.4000"",
                    ""3. low"": ""78.6600"",
                    ""4. close"": ""85.7650"",
                    ""5. adjusted close"": ""85.7650"",
                    ""6. volume"": ""537956820"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-05-29"": {
                    ""1. open"": ""79.2400"",
                    ""2. high"": ""81.8450"",
                    ""3. low"": ""76.7900"",
                    ""4. close"": ""79.1900"",
                    ""5. adjusted close"": ""79.1900"",
                    ""6. volume"": ""421870709"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-04-30"": {
                    ""1. open"": ""82.5000"",
                    ""2. high"": ""85.5900"",
                    ""3. low"": ""78.3200"",
                    ""4. close"": ""78.7700"",
                    ""5. adjusted close"": ""78.7700"",
                    ""6. volume"": ""542124478"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-03-31"": {
                    ""1. open"": ""79.0000"",
                    ""2. high"": ""86.0699"",
                    ""3. low"": ""77.2600"",
                    ""4. close"": ""82.2150"",
                    ""5. adjusted close"": ""82.2150"",
                    ""6. volume"": ""575349997"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-02-27"": {
                    ""1. open"": ""76.1100"",
                    ""2. high"": ""81.3700"",
                    ""3. low"": ""73.4500"",
                    ""4. close"": ""78.9700"",
                    ""5. adjusted close"": ""78.9700"",
                    ""6. volume"": ""475148609"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2015-01-30"": {
                    ""1. open"": ""78.5800"",
                    ""2. high"": ""79.2455"",
                    ""3. low"": ""73.5400"",
                    ""4. close"": ""75.9100"",
                    ""5. adjusted close"": ""75.9100"",
                    ""6. volume"": ""546057661"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-12-31"": {
                    ""1. open"": ""77.2600"",
                    ""2. high"": ""82.1700"",
                    ""3. low"": ""74.4000"",
                    ""4. close"": ""78.0200"",
                    ""5. adjusted close"": ""78.0200"",
                    ""6. volume"": ""534690249"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-11-28"": {
                    ""1. open"": ""75.4700"",
                    ""2. high"": ""78.2700"",
                    ""3. low"": ""72.5100"",
                    ""4. close"": ""77.7000"",
                    ""5. adjusted close"": ""77.7000"",
                    ""6. volume"": ""491018048"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-10-31"": {
                    ""1. open"": ""78.7800"",
                    ""2. high"": ""81.1600"",
                    ""3. low"": ""70.3200"",
                    ""4. close"": ""74.9900"",
                    ""5. adjusted close"": ""74.9900"",
                    ""6. volume"": ""1083643275"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-09-30"": {
                    ""1. open"": ""75.0100"",
                    ""2. high"": ""79.7067"",
                    ""3. low"": ""73.0700"",
                    ""4. close"": ""79.0400"",
                    ""5. adjusted close"": ""79.0400"",
                    ""6. volume"": ""719738102"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-08-29"": {
                    ""1. open"": ""72.2200"",
                    ""2. high"": ""75.9900"",
                    ""3. low"": ""71.5500"",
                    ""4. close"": ""74.8200"",
                    ""5. adjusted close"": ""74.8200"",
                    ""6. volume"": ""589592700"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-07-31"": {
                    ""1. open"": ""67.5800"",
                    ""2. high"": ""76.7400"",
                    ""3. low"": ""62.2100"",
                    ""4. close"": ""72.6500"",
                    ""5. adjusted close"": ""72.6500"",
                    ""6. volume"": ""1025679500"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-06-30"": {
                    ""1. open"": ""63.2300"",
                    ""2. high"": ""68.0000"",
                    ""3. low"": ""61.7900"",
                    ""4. close"": ""67.2900"",
                    ""5. adjusted close"": ""67.2900"",
                    ""6. volume"": ""862067700"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-05-30"": {
                    ""1. open"": ""60.4300"",
                    ""2. high"": ""64.3000"",
                    ""3. low"": ""56.2600"",
                    ""4. close"": ""63.3000"",
                    ""5. adjusted close"": ""63.3000"",
                    ""6. volume"": ""1120155500"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-04-30"": {
                    ""1. open"": ""60.4600"",
                    ""2. high"": ""63.9100"",
                    ""3. low"": ""54.6600"",
                    ""4. close"": ""59.7800"",
                    ""5. adjusted close"": ""59.7800"",
                    ""6. volume"": ""1884621400"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-03-31"": {
                    ""1. open"": ""66.9600"",
                    ""2. high"": ""72.5900"",
                    ""3. low"": ""57.9800"",
                    ""4. close"": ""60.2400"",
                    ""5. adjusted close"": ""60.2400"",
                    ""6. volume"": ""1255440000"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-02-28"": {
                    ""1. open"": ""63.0300"",
                    ""2. high"": ""71.4400"",
                    ""3. low"": ""60.7000"",
                    ""4. close"": ""68.4600"",
                    ""5. adjusted close"": ""68.4600"",
                    ""6. volume"": ""1110774100"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2014-01-31"": {
                    ""1. open"": ""54.8300"",
                    ""2. high"": ""63.3700"",
                    ""3. low"": ""51.8500"",
                    ""4. close"": ""62.5700"",
                    ""5. adjusted close"": ""62.5700"",
                    ""6. volume"": ""1294714800"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-12-31"": {
                    ""1. open"": ""46.9000"",
                    ""2. high"": ""58.5800"",
                    ""3. low"": ""46.2600"",
                    ""4. close"": ""54.6490"",
                    ""5. adjusted close"": ""54.6490"",
                    ""6. volume"": ""1517533200"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-11-29"": {
                    ""1. open"": ""50.8500"",
                    ""2. high"": ""52.0900"",
                    ""3. low"": ""43.5500"",
                    ""4. close"": ""47.0100"",
                    ""5. adjusted close"": ""47.0100"",
                    ""6. volume"": ""1357311900"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-10-31"": {
                    ""1. open"": ""49.9700"",
                    ""2. high"": ""54.8250"",
                    ""3. low"": ""45.2600"",
                    ""4. close"": ""50.2050"",
                    ""5. adjusted close"": ""50.2050"",
                    ""6. volume"": ""2032635000"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-09-30"": {
                    ""1. open"": ""41.8400"",
                    ""2. high"": ""51.6000"",
                    ""3. low"": ""41.4400"",
                    ""4. close"": ""50.2300"",
                    ""5. adjusted close"": ""50.2300"",
                    ""6. volume"": ""1583083800"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-08-30"": {
                    ""1. open"": ""37.3000"",
                    ""2. high"": ""42.2600"",
                    ""3. low"": ""36.0201"",
                    ""4. close"": ""41.2940"",
                    ""5. adjusted close"": ""41.2940"",
                    ""6. volume"": ""1344994100"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-07-31"": {
                    ""1. open"": ""24.9694"",
                    ""2. high"": ""38.3100"",
                    ""3. low"": ""24.1500"",
                    ""4. close"": ""36.8000"",
                    ""5. adjusted close"": ""36.8000"",
                    ""6. volume"": ""1438017100"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-06-28"": {
                    ""1. open"": ""24.2650"",
                    ""2. high"": ""25.1900"",
                    ""3. low"": ""22.6700"",
                    ""4. close"": ""24.8800"",
                    ""5. adjusted close"": ""24.8800"",
                    ""6. volume"": ""788331500"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-05-31"": {
                    ""1. open"": ""27.8500"",
                    ""2. high"": ""29.0700"",
                    ""3. low"": ""23.2600"",
                    ""4. close"": ""24.3480"",
                    ""5. adjusted close"": ""24.3480"",
                    ""6. volume"": ""982094800"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-04-30"": {
                    ""1. open"": ""25.6300"",
                    ""2. high"": ""28.1000"",
                    ""3. low"": ""25.1500"",
                    ""4. close"": ""27.7690"",
                    ""5. adjusted close"": ""27.7690"",
                    ""6. volume"": ""738509200"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-03-28"": {
                    ""1. open"": ""27.0500"",
                    ""2. high"": ""28.6750"",
                    ""3. low"": ""24.7200"",
                    ""4. close"": ""25.5800"",
                    ""5. adjusted close"": ""25.5800"",
                    ""6. volume"": ""727180500"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-02-28"": {
                    ""1. open"": ""31.0100"",
                    ""2. high"": ""31.0200"",
                    ""3. low"": ""26.3400"",
                    ""4. close"": ""27.2500"",
                    ""5. adjusted close"": ""27.2500"",
                    ""6. volume"": ""957639800"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2013-01-31"": {
                    ""1. open"": ""27.4400"",
                    ""2. high"": ""32.5063"",
                    ""3. low"": ""27.4200"",
                    ""4. close"": ""30.9810"",
                    ""5. adjusted close"": ""30.9810"",
                    ""6. volume"": ""1675851700"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-12-31"": {
                    ""1. open"": ""27.9999"",
                    ""2. high"": ""28.8800"",
                    ""3. low"": ""25.1500"",
                    ""4. close"": ""26.6197"",
                    ""5. adjusted close"": ""26.6197"",
                    ""6. volume"": ""1191832200"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-11-30"": {
                    ""1. open"": ""21.0800"",
                    ""2. high"": ""28.0000"",
                    ""3. low"": ""18.8700"",
                    ""4. close"": ""28.0000"",
                    ""5. adjusted close"": ""28.0000"",
                    ""6. volume"": ""1527490200"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-10-31"": {
                    ""1. open"": ""22.0800"",
                    ""2. high"": ""24.2500"",
                    ""3. low"": ""18.8000"",
                    ""4. close"": ""21.1100"",
                    ""5. adjusted close"": ""21.1100"",
                    ""6. volume"": ""1100938300"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-09-28"": {
                    ""1. open"": ""18.0800"",
                    ""2. high"": ""23.3700"",
                    ""3. low"": ""17.5500"",
                    ""4. close"": ""21.6600"",
                    ""5. adjusted close"": ""21.6600"",
                    ""6. volume"": ""1058643700"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-08-31"": {
                    ""1. open"": ""21.5000"",
                    ""2. high"": ""22.4500"",
                    ""3. low"": ""18.0300"",
                    ""4. close"": ""18.0580"",
                    ""5. adjusted close"": ""18.0580"",
                    ""6. volume"": ""1151944900"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-07-31"": {
                    ""1. open"": ""31.2500"",
                    ""2. high"": ""32.8800"",
                    ""3. low"": ""21.6100"",
                    ""4. close"": ""21.7100"",
                    ""5. adjusted close"": ""21.7100"",
                    ""6. volume"": ""520189700"",
                    ""7. dividend amount"": ""0.0000""
                },
                ""2012-06-29"": {
                    ""1. open"": ""28.8920"",
                    ""2. high"": ""33.4500"",
                    ""3. low"": ""25.5200"",
                    ""4. close"": ""31.0950"",
                    ""5. adjusted close"": ""31.0950"",
                    ""6. volume"": ""667910500"",
                    ""7. dividend amount"": ""0.0000""
                }
            }
        }";

    public class DataClass
    {
        public string StockSymbol { get; set; }
        public DateTime StockDate { get; set; }
        public double Volume { get; set; }
        public double DividendAmount { get; set; }
        public double StockHigh { get; set; }
        public double StockLow { get; set; }
        public double StockOpen { get; set; }
        public double StockClose { get; set; }
        public double StockAdjustedClose { get; set; }
        public double TotalSeconds { get; set; }
        public double TimeValue { get; set; }
    }

    public float Harmonic = 0f;
    public float ChartScale = 1f;
    public MathExpression.Expression.BuiltInMethods.FrequencyChart chart;
    public MathExpression.Expression.BuiltInMethods.TimeSampledData[] data;
    public List<MathExpression.Expression.BuiltInMethods.FrequencyOutput> frequencies;
    public List<DataClass> testData;

    public static decimal Sqrt(decimal x, decimal? guess = null)
    {
        var ourGuess = guess.GetValueOrDefault(x / 2m);
        var result = x / ourGuess;
        var average = (ourGuess + result) / 2m;

        if (average == ourGuess) // This checks for the maximum precision possible with a decimal.
            return average;
        else
            return Sqrt(x, average);
    }

    // Use this for initialization
    void Start () {
        decimal piTest = MathExpression.Expression.BuiltInMethods.PiCalculator.PI(1000M);

        decimal sqrt2_2 = Sqrt(2M) / 2M;
        decimal cos45_1 = MathExpression.Expression.BuiltInMethods.AngleCosine(0.5M);
        decimal sine45_1 = MathExpression.Expression.BuiltInMethods.AngleSine(0.5M);
        decimal cos45 = MathExpression.Expression.BuiltInMethods.CosineTaylor(0.5M, 9);
        decimal sine45 = MathExpression.Expression.BuiltInMethods.SineTaylor(0.5M, 9);

        decimal deltaCos = cos45 - cos45_1;
        decimal deltaSine = sine45 - sine45_1;

        //Debug.Log(sqrt2_2);
        Debug.Log(cos45);
        Debug.Log(sine45);
        Debug.Log(deltaCos);
        Debug.Log(deltaSine);
        //Debug.Log(sqrt2_2 - cos45);
        //Debug.Log(sqrt2_2 - sine45);

        //MathExpression.Expression.BuiltInMethods.TestAngles();
        //return;

        System.Random r = new System.Random();
        List<double> doubles = new List<double>();
        for (int z=0;z<2048;z++)
        {
            int rand = r.Next(1024);
            //double randDouble = (double)(rand - 512) / 1024.0;
            //randDouble *= 500.0;
            double xval = (double)z / (double)2048;
            double randDouble = 1000.0 * Math.Cos(2.0 * Math.PI * xval * (0.247 * 2048.0));
            //double randDouble = 1000.0 * Math.Cos(2.0 * Math.PI * xval);
            //double xVal = (double)z / (double)1024;
            //double randDouble = 1000.0 * (Math.Pow(Math.Cos(xVal), 2.0) + Math.Pow(Math.Sin(xVal), 2.0));
            doubles.Add(randDouble);
            //doubles.Add(1.0);
        }

        JObject test = Newtonsoft.Json.Linq.JObject.Parse(JsonTextString);
        testData = new List<DataClass>();
        string stockSymbol = (test["Meta Data"] as JObject)["2. Symbol"].ToString();
        foreach (var t in test["Monthly Adjusted Time Series"] as JObject)
        {
            string key = t.Key;
            DateTime dd = DateTime.Parse(key);
            JObject j = t.Value as JObject;
            double d1 = double.Parse(j["1. open"].ToString());
            double d2 = double.Parse(j["2. high"].ToString());
            double d3 = double.Parse(j["3. low"].ToString());
            double d4 = double.Parse(j["4. close"].ToString());
            double d5 = double.Parse(j["5. adjusted close"].ToString());
            double d6 = double.Parse(j["6. volume"].ToString());
            double d7 = double.Parse(j["7. dividend amount"].ToString());

            DataClass dataClass = new DataClass();
            dataClass.StockSymbol = stockSymbol;
            dataClass.StockDate = dd;
            dataClass.StockOpen = d1;
            dataClass.StockHigh = d2;
            dataClass.StockLow = d3;
            dataClass.StockClose = d4;
            dataClass.StockAdjustedClose = d5;
            dataClass.Volume = d6;
            dataClass.DividendAmount = d7;

            testData.Add(dataClass);
        }
        DateTime firstDate = testData.Min(x => x.StockDate);
        foreach(DataClass d in testData)
        {
            d.TotalSeconds = (d.StockDate - firstDate).TotalSeconds;
        }
        double maxSeconds = testData.Max(x => x.TotalSeconds);
        foreach (DataClass d in testData)
        {
            d.TimeValue = d.TotalSeconds / maxSeconds;
        }
        testData = testData.OrderBy(x => x.TimeValue).ToList();

        data = new MathExpression.Expression.BuiltInMethods.TimeSampledData[testData.Count];
        for (int z=0;z<testData.Count;z++)
        {
            data[z] = new MathExpression.Expression.BuiltInMethods.TimeSampledData(testData[z].TimeValue, testData[z].StockClose);
        }
        double[] dpoints = data.Select(x => x.Value).ToArray();
        double[] smoothPoints = MathExpression.Expression.BuiltInMethods.SmoothDFT(dpoints, 0.9);

        //Vector3[] pointsA = new Vector3[doubles.Count];
        Vector3[] pointsA = new Vector3[testData.Count];
        for (int z=0;z<pointsA.Length;z++)
        {
            pointsA[z] = new Vector3(0f, (float)testData[z].StockClose, (float)testData[z].TimeValue / (float)1.0f * 1000f);
        }
        //for (int z=0;z<doubles.Count;z++)
        //{
        //    float v = (float)doubles[z];
        //    pointsA[z] = new Vector3(0f, v, (float)z / (float)doubles.Count * 1000f);
        //}

        //double inputMax = 0.0;

        ////frequencies = new List<MathExpression.Expression.BuiltInMethods.FrequencyOutput>();
        ////chart = MathExpression.Expression.BuiltInMethods.HGT(data, data.Length, data.Length, out inputMax, out frequencies, 1.0, 1.0);

        //double[] inputs = data.OrderBy(x => x.Time).Select(x => x.Value).ToArray();
        //MathExpression.Expression.Vector<double>[] r1 = MathExpression.Expression.BuiltInMethods.DFT(inputs);
        //double[] outputs = MathExpression.Expression.BuiltInMethods.InverseDFTHarmonic(r1, 3, data.Length);

        //MathExpression.Expression.Vector<double>[] frequencies = MathExpression.Expression.BuiltInMethods.HGT(doubles.ToArray(), 2048, 2048, out inputMax, 1, 1);
        //MathExpression.Expression.Vector<double>[] frequencies = MathExpression.Expression.BuiltInMethods.HGT(data, 2048, 2048, out inputMax, 1, 1);
        //double[] inverse = MathExpression.Expression.BuiltInMethods.InverseHGT(frequencies, 2048, 2048, inputMax, 1, 1, 768);

        //double inMax = doubles.Max();
        //double invMax = inverse.Max();
        //double multiplier = inMax / invMax;

        //Vector3[] points1 = new Vector3[chart.ClampedRotationValues.Count];
        //for (int z = 0; z < points1.Length; z++)
        //{
        //    points1[z] = new Vector3(0f, (float)(chart.ClampedRotationValues[z].Total / 2048.0 * 50.0), (float)z / (float)points1.Length * 1000f);
        //}

        //Vector3[] points2 = new Vector3[chart.ClampedRotationValues.Count];
        //for (int z = 0; z < points2.Length; z++)
        //{
        //    points2[z] = new Vector3(0f, (float)(chart.ClampedRotationValues[z].Total / 2048.0 * 50.0), (float)z / (float)points2.Length * 1000f);
        //}

        // 1/21/2019 marked
        //Vector3[] points1 = new Vector3[smoothPoints.Length];
        //for (int z = 0; z < points1.Length; z++)
        //{
        //    points1[z] = new Vector3(0f, (float)(smoothPoints[z]), (float)(z+1) / (float)points1.Length * 1000f);
        //}

        //Vector3[] points2 = new Vector3[outputs.Length];
        //for (int z = 0; z < points2.Length; z++)
        //{
        //    //points2[z] = new Vector3(0f, (float)(chart.ClampedRotationValues[z].Total / 2048.0 * 50.0), (float)z / (float)points2.Length * 1000f);
        //}


        //MathExpression.Expression.Vector[] vectors = MathExpression.Expression.BuiltInMethods.VODFT(doubles.ToArray(), 20000);
        //MathExpression.Expression.Vector<double>[] vectors = MathExpression.Expression.BuiltInMethods.DFT(doubles.ToArray());
        //for (int z = 0; z < points1.Length; z++)
        //{
        //    points1[z] = new Vector3(0f, (float)(vectors[z].Values[0] / 2048.0 * 50.0), (float)z / (float)points1.Length * 1000f);
        //}

        double[] test1 = new double[2048];
        double[] test0 = new double[2048];
        for (int z = 0; z < 2048; z++)
        {
            test1[z] = 1000000.0;
            test0[z] = -1000000.0;
        }
        MathExpression.Expression.Vector<double>[,] vecA = MathExpression.Expression.BuiltInMethods.DFTCapture(test1);
        MathExpression.Expression.Vector<double>[,] vecB = MathExpression.Expression.BuiltInMethods.DFTCapture(test0);
        ////MathExpression.Expression.Vector<double>[,] vecB = MathExpression.Expression.BuiltInMethods.VODFTCapture(doubles.ToArray(), 20480);
        double[] out1 = MathExpression.Expression.BuiltInMethods.SumMagnitudes(vecA, true);
        double[] out2 = MathExpression.Expression.BuiltInMethods.SumMagnitudes(vecB, true);
        ////double[] dftMultiplier = MathExpression.Expression.BuiltInMethods.CreateDFTScaleMultiplier(1024, 20480);

        Vector3[] points1 = new Vector3[out1.Length];
        pointsA = new Vector3[out2.Length];
        ////////Vector3[] points2 = new Vector3[out2.Length];
        for (int z = 0; z < points1.Length; z++)
        {
            points1[z] = new Vector3(0f, (float)out1[z], (float)z / (float)points1.Length * 1000f);
            pointsA[z] = new Vector3(0f, (float)out2[z], (float)z / (float)pointsA.Length * 1000f);
        }

        ////for (int z=0;z<points2.Length;z++)
        ////{
        ////    points2[z] = new Vector3(0f, (float)out2[z], (float)z / (float)points2.Length * 1000f);
        ////}

        /*
        double lastValue = 0.0;
        for (int z = 0; z < points2.Length; z++)
        {
            double value = out2[z];
            double dftZ = dftMultiplier[z];
            if (dftZ == 0.0001)
            {
                value = lastValue;
            }
            else
            {
                value *= dftMultiplier[z];
                lastValue = value;
            }
            points2[z] = new Vector3(0f, (float)(value), (float)z / (float)points2.Length * 1000f);
        }
        */
        line.positionCount = pointsA.Length;
        line.SetPositions(pointsA);

        line2.positionCount = points1.Length;
        line2.SetPositions(points1);
        //line3.positionCount = points2.Length;
        //line3.SetPositions(points2);


        ////double[] fPointsA = MathExpression.Expression.BuiltInMethods.InverseDFTCaptureHarmonicBySums(vecA, 3);
        ////double[] fPointsB = MathExpression.Expression.BuiltInMethods.InverseDFTCaptureHarmonicBySums(vecB, 3);
        ////Vector3[] pointsI1 = new Vector3[fPointsA.Length];
        ////Vector3[] pointsI2 = new Vector3[fPointsB.Length];
        ////for (int z = 0; z < pointsI1.Length; z++)
        ////{
        ////    pointsI1[z] = new Vector3(0f, (float)fPointsA[z], (float)z / (float)pointsI1.Length * 1000f);
        ////}
        ////for (int z = 0; z < pointsI2.Length; z++)
        ////{
        ////    pointsI2[z] = new Vector3(0f, (float)fPointsB[z], (float)z / (float)pointsI2.Length * 1000f);
        ////}

        //////double[,] final = MathExpression.Expression.BuiltInMethods.InverseDFTCaptureHarmonicAll(vecB);

        //////for (int z=0;z<final.Length;z++)
        //////{
        //////    final[z] = MathExpression.Expression.BuiltInMethods.InverseDFTCaptureHarmonicSingularDistance(vecB, z);
        //////}
        //////pointsI2 = new Vector3[final.Length];
        //////for (int z = 0; z < final.Length; z++)
        //////{
        //////    pointsI2[z] = new Vector3(0f, (float)final[z], (float)z / (float)final.Length * 1000f);
        //////}

        ////lineinv1.positionCount = pointsI1.Length;
        ////lineinv1.SetPositions(pointsI1);
        ////lineinv2.positionCount = pointsI2.Length;
        ////lineinv2.SetPositions(pointsI2);


        ////Vector3[] points = new Vector3[vectors.Length];
        ////for (int z=0;z<vectors.Length;z++)
        ////{
        ////    //double magnitude = Math.Sqrt(vectors[z].Values[0] * vectors[z].Values[0] + vectors[z].Values[1] * vectors[z].Values[1]);
        ////    ////points[z] = new Vector3((float)vectors[z].Values[0], (float)vectors[z].Values[1], (float)z / (float)vectors.Length * 1000f);
        ////    //points[z] = new Vector3(0f, (float)magnitude, (float)z / (float)vectors.Length * 1000f);
        ////}
        ////double[] inverse = MathExpression.Expression.BuiltInMethods.InverseVODFT(vectors, doubles.Count);
        ////Vector3[] pointsB = new Vector3[inverse.Length];
        ////for (int z=0;z<inverse.Length;z++)
        ////{
        ////    float v = (float)inverse[z];
        ////    pointsB[z] = new Vector3(0f, (float)v, (float)z / (float)doubles.Count * 1000f);
        ////}
        ////line.positionCount = vectors.Length;
        ////line.SetPositions(points);
        ////line2.positionCount = doubles.Count;
        ////line2.SetPositions(pointsA);
        ////line3.positionCount = inverse.Length;
        ////line3.SetPositions(pointsB);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateChart()
    {
        //double inputMax = 0.0;
        //List<MathExpression.Expression.BuiltInMethods.FrequencyOutput> frequencies = new List<MathExpression.Expression.BuiltInMethods.FrequencyOutput>();
        //chart = MathExpression.Expression.BuiltInMethods.HGT(data, 2048, 2048, out inputMax, out frequencies, 1.0, 1.0);
        data = new MathExpression.Expression.BuiltInMethods.TimeSampledData[testData.Count];
        for (int z = 0; z < testData.Count; z++)
        {
            data[z] = new MathExpression.Expression.BuiltInMethods.TimeSampledData(testData[z].TimeValue, testData[z].StockClose);
        }
        double[] dpoints = data.Select(x => x.Value).ToArray();
        double[] smoothPoints = MathExpression.Expression.BuiltInMethods.SmoothDFT(dpoints, Harmonic);
        //List<MathExpression.Expression.BuiltInMethods.FrequencyChart.TimeValue> tv = chart.ProduceInverseHarmonic(Harmonic);
        Vector3[] points1 = new Vector3[smoothPoints.Length];
        for (int z = 0; z < smoothPoints.Length; z++)
        {
            if (Harmonic <= 1.0)
                points1[z] = new Vector3(0f, (float)(smoothPoints[z] / 2048.0 * ChartScale), (float)(z+1) / (float)points1.Length * 1000f);
        }
        line2.positionCount = points1.Length;
        line2.SetPositions(points1);
    }
}
