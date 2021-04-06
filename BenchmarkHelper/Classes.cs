﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BenchmarkHelper;

[assembly: JsonSerializable(typeof(LoginViewModel))]
[assembly: JsonSerializable(typeof(Location))]
[assembly: JsonSerializable(typeof(IndexViewModel))]
[assembly: JsonSerializable(typeof(MyEventsListerViewModel))]

namespace BenchmarkHelper
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
    }

    public class IndexViewModel
    {
        public List<ActiveOrUpcomingEvent> ActiveOrUpcomingEvents { get; set; }
        public CampaignSummaryViewModel FeaturedCampaign { get; set; }
        public bool IsNewAccount { get; set; }
        public bool HasFeaturedCampaign => FeaturedCampaign != null;
    }

    public class MyEventsListerViewModel
    {
        public List<MyEventsListerItem> CurrentEvents { get; set; } = new List<MyEventsListerItem>();
        public List<MyEventsListerItem> FutureEvents { get; set; } = new List<MyEventsListerItem>();
        public List<MyEventsListerItem> PastEvents { get; set; } = new List<MyEventsListerItem>();
    }

    public class MyEventsListerItemTask
    {
        public string Name { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        [JsonIgnore]
        public string FormattedDate
        {
            get
            {
                if (!StartDate.HasValue || !EndDate.HasValue)
                {
                    return null;
                }

                var startDateString = string.Format("{0:g}", StartDate.Value);
                var endDateString = string.Format("{0:g}", EndDate.Value);

                return string.Format($"From {startDateString} to {endDateString}");
            }
        }
    }

    public class MyEventsListerItem
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string TimeZone { get; set; }
        public string Campaign { get; set; }
        public string Organization { get; set; }
        public int VolunteerCount { get; set; }

        public List<MyEventsListerItemTask> Tasks { get; set; } = new List<MyEventsListerItemTask>();
    }

    public class CampaignSummaryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string OrganizationName { get; set; }
        public string Headline { get; set; }
    }

    public class ActiveOrUpcomingEvent
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string CampaignName { get; set; }
        public string CampaignManagedOrganizerName { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
