﻿using DomainLayer.Enums;

namespace API_Controller_Demo.Model
{
    public class ActionResultData
    {
        public ActionResultData()
        {
            Status = Status.Failed;
        }

        public Status Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public string StatusString => Status.ToString();
    }
}
