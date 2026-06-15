using FlyJusticeLite.ViewModels;

namespace FlyJusticeLite.Services;

public sealed class PublicContentService : IPublicContentService
{
    private static readonly IReadOnlyList<ServiceFeature> ServiceFeatures =
    [
        new("Flight delays", "Claims for arrivals delayed by three hours or more when the airline was responsible.", "Delay rights", "/Rights", "flight-delay"),
        new("Cancelled flights", "Support for last-minute cancellations, refund choices, rerouting, and compensation checks.", "Cancellation rights", "/Rights", "flight-cancellation"),
        new("Missed connections", "Eligibility guidance when a delay makes you miss an onward flight on the same booking.", "Connection rights", "/Rights", "missed-connection"),
        new("Denied boarding", "Clear next steps for overbooking, involuntary boarding refusal, care, rerouting, and compensation.", "Denied boarding rights", "/Rights", "denied-boarding"),
        new("Care and assistance", "Meals, hotel stays, airport transfers, and communication rights during long disruptions.", "Care rules", "/Rights", "care-assistance"),
        new("EU 261 coverage", "Understand whether your route, airline, and disruption fall under EU passenger law.", "EU 261 guide", "/Rights", "eu-261-2004")
    ];

    private static readonly IReadOnlyList<ProcessStep> ProcessSteps =
    [
        new("01", "Submit flight details", "Enter the flight, route, passenger, and delay information through a validated form.", true),
        new("02", "Upload ticket evidence", "Attach a ticket, booking confirmation, boarding pass, or disruption proof.", true),
        new("03", "Eligibility is calculated", "FlyJustice Lite estimates compensation and stores the claim securely.", false),
        new("04", "Claim team reviews", "Operators can review documents, update status, and keep the case moving.", false),
        new("05", "Track every update", "Passengers can search by claim number and see the latest status anytime.", true)
    ];

    private static readonly IReadOnlyList<TrustPoint> TrustPoints =
    [
        new("Passenger-first by design", "No account maze. Check eligibility, submit evidence, and track progress with one claim number."),
        new("No win, no fee model", "Eligibility and submission are free. A service fee applies only when compensation is recovered."),
        new("GDPR-aware handling", "The platform collects only the information needed to assess and operate a passenger claim."),
        new("Clear case progress", "Every claim moves through a simple, visible status journey from submission to a final decision.")
    ];

    private static readonly IReadOnlyList<SupportedAirline> SupportedAirlines =
    [
        new("British Airways", "BA"),
        new("Lufthansa", "LH"),
        new("Air France", "AF"),
        new("KLM", "KL"),
        new("Ryanair", "FR"),
        new("easyJet", "U2"),
        new("Wizz Air", "W6"),
        new("Iberia", "IB"),
        new("TAP Air Portugal", "TP"),
        new("Aer Lingus", "EI"),
        new("SAS", "SK"),
        new("Vueling", "VY")
    ];

    private static readonly IReadOnlyList<PassengerStory> PassengerStories =
    [
        new(
            "The eligibility check took less than a minute, and I always knew what the next step was.",
            "Maya R.",
            "London to Lisbon",
            "Approved for EUR 400"),
        new(
            "I uploaded my booking confirmation from my phone and tracked the claim without creating another account.",
            "Daniel K.",
            "Berlin to Barcelona",
            "Approved for EUR 250"),
        new(
            "The airline called it extraordinary circumstances. The review explained what evidence still mattered.",
            "Sofia P.",
            "Paris to Athens",
            "Case moved to review")
    ];

    private static readonly IReadOnlyList<SuccessStory> SuccessStories =
    [
        new("Manchester to Madrid", "Flight arrived 4h 18m late", "Delay evidence accepted after review", "EUR 400"),
        new("Dublin to Amsterdam", "Cancellation announced on departure day", "Rerouting and compensation assessed", "EUR 250"),
        new("Frankfurt to Rome", "Connection missed on one booking", "Final-arrival delay confirmed", "EUR 400")
    ];

    private static readonly IReadOnlyList<PublicContentPage> RightsPages =
    [
        new(
            "flight-delay",
            "Flight Delay Compensation",
            "Passenger rights",
            "If your flight reaches the final destination three or more hours late, EU passenger rules may entitle you to compensation.",
            ["Three-hour arrival delay threshold", "Compensation up to EUR 600", "Airline responsibility matters"],
            [
                new("When a delay can qualify", "A claim is strongest when the airline could reasonably control the cause of the delay and the final arrival delay is at least three hours."),
                new("What passengers should keep", "Evidence makes the case easier to review.", ["Ticket or booking confirmation", "Boarding pass if available", "Airline messages about the delay", "Receipts for meals or hotel costs"])
            ]),
        new(
            "flight-cancellation",
            "Flight Cancellation Rights",
            "Passenger rights",
            "Cancelled flights can create refund, rerouting, care, and compensation rights depending on notice period and cause.",
            ["Refund or rerouting choices", "Possible compensation", "Care during long waits"],
            [
                new("Notice period matters", "If an airline cancels close to departure, compensation may apply unless it offered a suitable alternative or the cancellation came from extraordinary circumstances."),
                new("Your basic options", "Passengers usually have a choice between refund, rerouting as soon as possible, or rerouting later when seats are available.")
            ]),
        new(
            "missed-connection",
            "Missed Connection Claims",
            "Passenger rights",
            "A missed onward flight can qualify when all legs are on one booking and the final destination is delayed by three or more hours.",
            ["Same booking required", "Final arrival delay counts", "Route coverage still applies"],
            [
                new("How connection claims work", "The key question is not only whether you missed the connection, but how late you reached the final destination shown on the booking."),
                new("Helpful documents", "Keep the complete itinerary, replacement boarding passes, airline notices, and receipts for expenses caused by the disruption.")
            ]),
        new(
            "denied-boarding",
            "Denied Boarding and Overbooking",
            "Passenger rights",
            "When an airline refuses boarding because of overbooking, passengers may have compensation, rerouting, refund, and care rights.",
            ["Overbooking support", "Refund or rerouting", "Compensation review"],
            [
                new("Voluntary vs involuntary refusal", "If you volunteer your seat, the airline offer controls the outcome. If you are involuntarily denied boarding, statutory rights may apply."),
                new("What to request at the airport", "Ask for written confirmation of the denied boarding reason and keep any meal, hotel, or transport receipts.")
            ]),
        new(
            "overbooking",
            "Overbooking Rights",
            "Passenger rights",
            "Overbooking can lead to denied boarding, replacement travel, refund choices, and compensation depending on whether you volunteered your seat.",
            ["Voluntary offers differ", "Involuntary refusal can qualify", "Care and rerouting still matter"],
            [
                new("If you volunteer", "When passengers accept an airline offer to give up a seat, the agreed benefits usually control the outcome."),
                new("If you are refused boarding", "If the airline involuntarily refuses boarding, ask for written confirmation and keep all replacement travel and expense evidence.")
            ]),
        new(
            "strikes",
            "Strike Disruptions",
            "Special cases",
            "Strike-related claims depend on who was striking and whether the airline could reasonably prevent or manage the disruption.",
            ["Airline crew strikes may qualify", "Airport or air traffic strikes often differ", "Care rights can still apply"],
            [
                new("Airline responsibility", "Claims are strongest when the disruption sits within the airline's operational control."),
                new("Care is separate", "Even where compensation is not due, passengers may still have rights to meals, accommodation, transfers, rerouting, or refunds.")
            ]),
        new(
            "weather-reasons",
            "Weather and Extraordinary Events",
            "Special cases",
            "Severe weather can remove cash compensation, but it does not erase basic care, refund, and rerouting rights.",
            ["Weather often counts as extraordinary", "Care rights continue", "Evidence still helps"],
            [
                new("What weather changes", "Airlines are usually not responsible for truly severe or unsafe weather conditions."),
                new("What remains available", "Passengers may still be entitled to practical assistance while waiting and refund or rerouting options if travel becomes impossible.")
            ]),
        new(
            "eu-261-2004",
            "EU Regulation 261/2004",
            "Legal framework",
            "EU 261 protects eligible passengers affected by delays, cancellations, denied boarding, and certain missed connections.",
            ["EU departures covered", "EU-airline arrivals can be covered", "Compensation from EUR 250 to EUR 600"],
            [
                new("Route coverage", "Flights departing from the EU are generally covered. Flights arriving into the EU can be covered when operated by an EU-based airline."),
                new("Compensation bands", "Compensation depends on disruption type, route, distance, arrival delay, and airline responsibility.")
            ]),
        new(
            "extraordinary-circumstances",
            "Extraordinary Circumstances",
            "Special cases",
            "Airlines may avoid cash compensation when disruption comes from events outside their control, but passengers still retain important care rights.",
            ["Severe weather", "Air traffic restrictions", "Security risks", "Medical emergencies"],
            [
                new("Common examples", "Extraordinary circumstances can include severe storms, volcanic ash, airspace closure, security emergencies, and some third-party strikes."),
                new("Still worth checking", "The label alone is not enough. A review should look at the actual cause, timing, and whether the airline took reasonable steps.")
            ]),
        new(
            "care-assistance",
            "Care and Assistance Rights",
            "Passenger rights",
            "During long waits, airlines may need to provide meals, refreshments, communication, hotel accommodation, and airport transfers.",
            ["Meals and drinks", "Hotel for overnight waits", "Airport transfers", "Communication support"],
            [
                new("When care applies", "Care depends on waiting time, route length, and disruption type, and can apply even when cash compensation does not."),
                new("Keep receipts", "If the airline does not provide support directly, reasonable receipts can help a reimbursement request.")
            ])
    ];

    private static readonly IReadOnlyList<FaqCategory> FaqCategories =
    [
        new("Compensation", "Eligibility, amounts, and delay thresholds.", [
            new("How much compensation can passengers receive?", "Typical EU 261 compensation bands are EUR 250, EUR 400, and EUR 600. The amount depends on route distance, arrival delay, and whether the airline was responsible."),
            new("Is a two-hour delay enough for cash compensation?", "Usually no. A two-hour delay may trigger care rights on some routes, but cash compensation normally requires a three-hour arrival delay."),
            new("Can compensation be reduced?", "In some rerouting situations, compensation may be reduced when the replacement arrival time is close to the original schedule.")
        ]),
        new("Cancellations and delays", "Refunds, rerouting, and disruption handling.", [
            new("What if the airline cancels my flight?", "You can usually choose between a refund and rerouting. Compensation may also apply when the cancellation notice was late and the airline was responsible."),
            new("Can I request a refund after a long delay?", "If the delay is long enough that travel no longer makes sense, passengers may have refund options. Keep evidence of the disruption and airline communications."),
            new("Who is responsible on a codeshare flight?", "The operating airline is usually responsible for EU 261 obligations, even if another airline sold the ticket.")
        ]),
        new("Documents and process", "What to upload and how tracking works.", [
            new("What documents should I upload?", "A ticket, booking confirmation, boarding pass, airline message, disruption proof, or expense receipt can help the review team."),
            new("How do I track a claim?", "After submission, use the claim number on the tracking page to see status, compensation estimate, and submission date."),
            new("Can I submit without creating an account?", "Yes. FlyJustice Lite lets passengers submit a claim and track it by claim number.")
        ]),
        new("Deadlines and coverage", "Where EU 261 applies and when to act.", [
            new("Which routes are covered?", "Flights departing from the EU are generally covered. Flights arriving into the EU can be covered when the operating airline is EU-based."),
            new("How long do I have to claim?", "Time limits vary by country. It is best to submit early while evidence and airline records are easier to collect."),
            new("Do extraordinary circumstances remove every right?", "No. They may affect cash compensation, but care, refund, and rerouting rights may still apply.")
        ])
    ];

    private static readonly IReadOnlyList<FeeItem> Fees =
    [
        new("Eligibility check", "Free", "Passengers can check the compensation estimate before opening a claim."),
        new("Claim submission", "Free", "The intake form, claim number, ticket upload, and tracking page are available without upfront payment."),
        new("Service fee", "Success-based", "A service fee should only apply when compensation is recovered. Configure the exact percentage in your operational terms."),
        new("Rejected claims", "No recovery, no service fee", "If the claim is rejected or not pursued, the passenger should not be charged a service fee.")
    ];

    private static readonly IReadOnlyList<PublicContentPage> LegalPages =
    [
        new("terms", "Terms and Conditions", "Compliance", "Operational terms for using FlyJustice Lite claim intake and tracking.", [], [
            new("Service scope", "FlyJustice Lite helps collect claim details, estimate compensation, store documents, and support review workflows. It is not a substitute for independent legal advice."),
            new("Passenger responsibility", "Passengers should provide accurate details and upload documents they are authorized to share."),
            new("Operator responsibility", "Operators should review claims carefully, protect uploaded documents, and limit access to internal admin routes.")
        ]),
        new("privacy", "Privacy Policy", "Compliance", "A practical privacy summary for passenger claim data handled by FlyJustice Lite.", [], [
            new("Data collected", "The app stores passenger identity, contact information, flight details, compensation estimates, claim status, and uploaded ticket documents."),
            new("Purpose", "Data is used to review claims, communicate status, and administer the claim workflow."),
            new("Security", "Use strong admin credentials, HTTPS, protected database credentials, and least-privilege access for production operations.")
        ]),
        new("gdpr", "GDPR", "Compliance", "FlyJustice Lite is designed for careful, minimal handling of passenger claim information.", [], [
            new("Data minimization", "Forms collect the fields needed for claim review and tracking."),
            new("Access and deletion", "Operators can delete claims and associated documents when retention is no longer required."),
            new("Operational controls", "Production operators should define retention, access, and data export procedures.")
        ]),
        new("imprint", "Imprint", "Compliance", "Public company and operator information can be configured here before commercial launch.", [], [
            new("Operator details", "Add registered company name, address, registration number, and contact email."),
            new("Responsible party", "Add the person or department responsible for the public website and claim workflow.")
        ])
    ];

    public IReadOnlyList<ServiceFeature> GetServiceFeatures() => ServiceFeatures;

    public IReadOnlyList<ProcessStep> GetProcessSteps() => ProcessSteps;

    public IReadOnlyList<TrustPoint> GetTrustPoints() => TrustPoints;

    public IReadOnlyList<SupportedAirline> GetSupportedAirlines() => SupportedAirlines;

    public IReadOnlyList<PassengerStory> GetPassengerStories() => PassengerStories;

    public IReadOnlyList<SuccessStory> GetSuccessStories() => SuccessStories;

    public IReadOnlyList<PublicContentPage> GetRightsPages() => RightsPages;

    public PublicContentPage? GetRightsPage(string slug)
    {
        return RightsPages.FirstOrDefault(page =>
            string.Equals(page.Slug, slug, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<FaqCategory> GetFaqCategories() => FaqCategories;

    public IReadOnlyList<FeeItem> GetFees() => Fees;

    public IReadOnlyList<PublicContentPage> GetLegalPages() => LegalPages;

    public PublicContentPage? GetLegalPage(string slug)
    {
        return LegalPages.FirstOrDefault(page =>
            string.Equals(page.Slug, slug, StringComparison.OrdinalIgnoreCase));
    }
}
