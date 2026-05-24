namespace ITravel_App.Models
{
    public static class Labels
    {
        public static Dictionary<string, Dictionary<string, string>> Texts = new()
        {
            ["en"] = new Dictionary<string, string>
            {
                ["Title"] = "BOOKING CONFIRMATION",
                ["Greeting"] = "Hi",
                ["Confirmed"] = "Your trip is confirmed",
                ["Trip"] = "Trip",
                ["BookingRef"] = "Booking Ref",
                ["Date"] = "Date",
                ["Guests"] = "Guests",
                ["Pickup"] = "Pickup",
                ["Payment"] = "Payment",
                ["Itinerary"] = "Itinerary",
                ["PriceSummary"] = "Price Summary",
                ["TripPrice"] = "Trip price",
                ["Discount"] = "Discount",
                ["Total"] = "Total",
                ["Help"] = "Need help? Contact us at",
                ["Nationality"]= "Nationality",
                ["ReturnDate"] = "Return Date",
                ["Extra"] = "Extras",
                ["Drop"] = "Drop Address"
            },
            ["de"] = new Dictionary<string, string>
            {
                ["Title"] = "REISEBESTÄTIGUNG",
                ["Greeting"] = "Hallo",
                ["Confirmed"] = "Ihre Reise ist bestätigt",
                ["Trip"] = "Reise",
                ["BookingRef"] = "Buchungsnr.",
                ["Date"] = "Daten",
                ["Guests"] = "Gäste",
                ["Pickup"] = "Abholung",
                ["Payment"] = "Zahlung",
                ["Itinerary"] = "Reiseplan",
                ["PriceSummary"] = "Preisübersicht",
                ["TripPrice"] = "Reisepreis",
                ["Discount"] = "Rabatt",
                ["Total"] = "Gesamt",
                ["Help"] = "Brauchen Sie Hilfe? Kontaktieren Sie uns unter",
                ["Nationality"] = "Nationalität",
                ["ReturnDate"] = "Rückflugdatum",
                ["Extra"] = "Extras",
                ["Drop"] = "Drop-Adresse"
            },
            ["ru"] = new Dictionary<string, string>
            {
                ["Title"] = "ПОДТВЕРЖДЕНИЕ БРОНИРОВАНИЯ",
                ["Greeting"] = "Здравствуйте",
                ["Confirmed"] = "Ваша поездка подтверждена",
                ["Trip"] = "Поездка",
                ["BookingRef"] = "Номер бронирования",
                ["Date"] = "Дата",
                ["Guests"] = "Гости",
                ["Pickup"] = "Место встречи",
                ["Payment"] = "Оплата",
                ["Itinerary"] = "Маршрут",
                ["PriceSummary"] = "Сводка стоимости",
                ["TripPrice"] = "Стоимость поездки",
                ["Discount"] = "Скидка",
                ["Total"] = "Итого",
                ["Help"] = "Нужна помощь? Свяжитесь с нами:",
                ["Nationality"] = "Национальность",
                ["ReturnDate"] = "Дата возвращения",
                ["Extra"] = "Дополнительно",
                ["Drop"] = "Адрес высадки"
            },
        };
    }

}
