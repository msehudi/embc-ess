using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Services.Registrations
{
    public class NewRegistrationService : IRequestHandler<CreateNewRegistrationCommand, ViewModels.Registration>
    {
        private readonly IDataInterface dataInterface;
        private readonly IEmailSender emailSender;
        private readonly IMediator mediator;

        public NewRegistrationService(IDataInterface dataInterface, IEmailSender emailSender, IMediator mediator)
        {
            this.dataInterface = dataInterface;
            this.emailSender = emailSender;
            this.mediator = mediator;
        }

        public async Task<ViewModels.Registration> Handle(CreateNewRegistrationCommand request, CancellationToken cancellationToken)
        {
            var registration = request.Registration;
            registration.Id = null;
            registration.Active = true;
            var essFileNumber = await dataInterface.CreateEvacueeRegistrationAsync(registration);
            var result = await dataInterface.GetEvacueeRegistrationAsync(essFileNumber.ToString());
            await mediator.Publish(new RegistrationCreated(essFileNumber.ToString(), result));
            if (result.IsFinalized) await mediator.Publish(new RegistrationFinalized(essFileNumber.ToString(), result));

            //TODO: move to a notification handler
            if (!string.IsNullOrWhiteSpace(request.Registration.HeadOfHousehold.Email))
            {
                var registrationEmail = CreateEmailMessageForRegistration(result);
                emailSender.Send(registrationEmail);
            }
            return result;
        }

        private EmailMessage CreateEmailMessageForRegistration(ViewModels.Registration registration)
        {
            var essRegistrationLink = @"<a target='_blank' href='https://justice.gov.bc.ca/embcess/self-registration'>Evacuee Self-Registration</a>";
            var emergencyInfoBCLink = @"<a target='_blank' href='https://www.emergencyinfobc.gov.bc.ca/'>Emergency Info BC</a>";

            var subject = "Registration completed successfully";
            var body = $@"
<p>This email has been generated by the Emergency Support Services program to provide you with a record of your Emergency Support Services File Number. If you have not registered online via the {essRegistrationLink} and are receiving this email, please contact your family members to ensure they have not registered on your behalf. If you have received this email in error, please disregard.</p>
<p> Your Emergency Support Services File Number is: <strong>{registration.EssFileNumber}</strong></p>
";

            // var body = "<h2>Evacuee Registration Success</h2><br/>" + "<b>What you need to know:</b><br/><br/>" +
            //    $"Your Emergency Support Services File Number is: <b>{registration.EssFileNumber}</b>";

            if (registration.IncidentTask == null)
            {
                body += $@"
<ul>
    <li>If you are under order and require food, clothing, lodging, transportation, incidentals or other emergency supports, proceed to your nearest Reception Centre. A list of open Reception Centres can be found at {emergencyInfoBCLink}.</li>
    <li>If you do not require supports, or are under alert, no further actions are required.</li>
    <li>If you are in a Reception Centre, proceed to one of the Emergency Support Services volunteers on site who will be able to assist you with completing your registration.</li>
    <li>Please bring your Emergency Support Services File Number with you to the Reception Centre.</li>
</ul>
";
            }

            return new EmailMessage(registration.HeadOfHousehold.Email, subject, body);
        }
    }

    public class CreateNewRegistrationCommand : IRequest<ViewModels.Registration>
    {
        public CreateNewRegistrationCommand(ViewModels.Registration registration)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }

    public class RegistrationCreated : RegistrationEvent
    {
        public RegistrationCreated(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }

    public class RegistrationFinalized : RegistrationEvent
    {
        public RegistrationFinalized(string essFileNumber, ViewModels.Registration registration) : base(essFileNumber)
        {
            Registration = registration;
        }

        public ViewModels.Registration Registration { get; }
    }
}
