using System;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Utilities;

namespace SPlanAdquisicionPago.Controllers
{
    public class PagoAdquisicionValidator : AbstractValidator<JObject>
    {
        public PagoAdquisicionValidator()
        {
            RuleFor(pago_adquisicion => pago_adquisicion["planId"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must((pago_adquisicion, type) => { return GenericValidatorType.ValidateType(pago_adquisicion["planId"].ToString(), typeof(Int32)); });
            RuleFor(pago_adquisicion => pago_adquisicion["pagos"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must((pago_adquisicion, type) => { return GenericValidatorType.ValidateType(pago_adquisicion["pagos"].ToString(), typeof(String)); });
        }
    }
}
