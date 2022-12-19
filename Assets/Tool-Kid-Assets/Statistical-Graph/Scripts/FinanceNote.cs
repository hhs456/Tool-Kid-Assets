using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Text;

public class DateLoader {
    public DateLoader() {
        var url = "http://data.ntpc.gov.tw/api/v1/rest/datastore/382000000A-000077-002";
        var request = WebRequest.Create(url);
        // �z�L Chrome �}�o�̤u��i�H���o Method, ContentType
        request.Method = "GET";
        request.ContentType = "application/json;charset=UTF-8";
        var response = request.GetResponse() as HttpWebResponse;
        var responseStream = response.GetResponseStream();
        var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
        var srcString = reader.ReadToEnd();
    }
}

public class JsonDate {

}

public class FinanceNote {
    public Date date;
    public string[] tags;
    public MoneyType type;
    public float money;
    FinanceNote() {
        date = new Date(DateTime.Today);
        
    }
}

public struct Date {    
    public int year;
    public int month;    
    public int day;
    public DayOfWeek week;
    public int count;

    public Date(DateTime dateTime) {
        year = dateTime.Year;
        month = dateTime.Month;
        day = dateTime.Day;
        week = dateTime.DayOfWeek;
        count = dateTime.DayOfYear;
    }
}

public enum MoneyType {
    [InspectorName("�s�x�� NTD")] NewTaiwanDollar
}

public struct Holiday {
    [SerializeField, Label("���")] private Date date;
    [SerializeField, Label("�W��")] private string name;
    [SerializeField, Label("�O�_��")] private bool isHoliday;
    [SerializeField, Label("����O")] private string holidayCategory;
    [SerializeField, Label("�y�z")] private string description;

    public Date Date { get => date; set => date = value; }
    public string Name { get => name; set => name = value; }
    public bool IsHoliday { get => isHoliday; set => isHoliday = value; }
    public string HolidayCategory { get => holidayCategory; set => holidayCategory = value; }
    public string Description { get => description; set => description = value; }
}