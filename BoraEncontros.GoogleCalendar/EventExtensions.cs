using Google.Apis.Calendar.v3.Data;
using System.Text.RegularExpressions;

namespace BoraEncontros.GoogleCalendar;
public static class EventExtensions
{
    public static string[]? GetAttachments(this Event @event)
    {
        if (@event?.Attachments != null)
        {
            return @event.Attachments.Select(e => $"https://drive.google.com/uc?id={e.FileId}").ToArray();
        }
        return null;
    }
    public static string? GetTicketUrl(this Event @event)
    {
        var ticketDomains = new[] { "sympla", "eleventickets", "uhuu", "eventbrite", "blueticket", "ingresse", "ticketswap", "vamoapp", "ingressorapido", "uhuu", "eventbrite", "lets.events", "appticket", "ingressonacional", "minhaentrada", "eventim" };
        return GetUrl(@event.Description, ticketDomains);
    }
    public static string? GetTicketDomain(this Event @event)
    {
        if (@event?.Description != null)
        {
            var urlPattern = @"(?:https?:\/\/)?(?:www\.)?([^\/]+)";
            var matches = Regex.Matches(@event.Description, urlPattern);
            return matches?.ElementAtOrDefault(1)?.Value;
        }
        return null;
    }
    public static string? GetSpotifyUrl(this Event @event)
    {
        return GetUrl(@event.Description, "spotify");
    }
    public static string? GetInstagramUrl(this Event @event)
    {
        return GetUrl(@event.Description, "instagram");
    }
    public static string? GetYouTubeUrl(this Event @event)
    {
        return GetUrl(@event.Description, "youtube", "youtu.be");
    }
    public static string? GetWhatsAppGroupChat(this Event @event)
    {
        return GetUrl(@event.Description, "chat.whatsapp.com");
    }
    public static string? GetSpotifyJam(this Event @event)
    {
        return GetUrl(@event.Description, "spotify.link", "open.spotify.com");
    }
    public static string? GetDiscordChannel(this Event @event)
    {
        bool hasDiscord = !string.IsNullOrEmpty(@event.Location) && @event.Location.Contains("discord.gg");
        return hasDiscord ? GetUrl(@event.Location, "discord.gg") : null;
    }
    public static string? GetWhatsAppMe(this Event @event)
    {
        bool hasWaMe = !string.IsNullOrEmpty(@event.Location) && @event.Location.Contains("wa.me");
        return hasWaMe ? GetUrl(@event.Location, "wa.me") : null;
    }
    public static string? GetMetaChannel(this Event @event)
    {
        bool hasMetaChannel = !string.IsNullOrEmpty(@event.Location) && @event.Location.Contains("horizon.meta.com");
        return hasMetaChannel ? GetUrl(@event.Location, "horizon.meta.com") : null;
    }
    public static string? GetTribeChannel(this Event @event)
    {
        bool hasTribeChannel = !string.IsNullOrEmpty(@event.Location) && @event.Location.Contains("live.tribexr.com");
        return hasTribeChannel ? GetUrl(@event.Location, "live.tribexr.com") : null;
    }
    public static string? GetTwitchChannel(this Event @event)
    {
        bool hasTwitchChannel = !string.IsNullOrEmpty(@event.Location) && @event.Location.Contains("twitch.tv");
        return hasTwitchChannel ? GetUrl(@event.Location, "twitch.tv") : null;
    }
    public static string GetConferenceUrl(this Event @event)
    {
        var conferenceUrl = GetDiscordChannel(@event) ??
                            GetMetaChannel(@event) ??
                            GetWhatsAppMe(@event) ??
                            GetSpotifyJam(@event) ??
                            GetTribeChannel(@event) ??
                            GetTwitchChannel(@event) ??
                            @event.HangoutLink;
        return conferenceUrl;
    }
    public static decimal GetDiscount(this Event @event)
    {
        if (@event.Summary != null)
        {
            Regex regex = new(@"\b(\d{1,3})%\$");
            Match match = regex.Match(@event.Summary);

            if (match.Success)
                return decimal.Parse(match.Groups[1].Value);
        }
        return 0;
    }
    public static bool IsPrivate(this Event @event)
    {
        if (@event.Description == null) return false;
        var isPrivate = new[] { EventResponse.PRIVADO, EventResponse.PRIVATE }.Any(pvt => @event.Description.Contains(pvt));

        return isPrivate;
    }
    public static DateTime? GetDeadLine(this Event @event)
    {
        var reminderMinutes = @event.Reminders.Overrides?.FirstOrDefault()?.Minutes;
        if (reminderMinutes != null && @event.Start.DateTime.HasValue)
        {
            return @event.Start.DateTime.Value.AddMinutes(-reminderMinutes.Value);
        }
        return null;
    }
    public static void AddGoogleMeet(this Event @event)
    {
        @event.ConferenceData = new ConferenceData
        {
            CreateRequest = new CreateConferenceRequest
            {
                RequestId = $"{@event.Summary}-{@event.Start?.DateTime}",
                ConferenceSolutionKey = new ConferenceSolutionKey
                {
                    Type = "hangoutsMeet"
                }
            }
        };
    }
    private static string? GetUrl(string description, params string[] domains)
    {
        if (description != null)
        {
            var urlPattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
            var matches = Regex.Matches(description, urlPattern);
            foreach (Match match in matches.Where(m => m.Success))
            {
                bool urlHasDomain = domains.Any(domain => match.Value.Contains(domain));
                if (urlHasDomain)
                    return match.Value;
            }
        }
        return null;
    }
}
