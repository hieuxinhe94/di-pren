define(["jquery"], function ($) {
    return {
        area: $("#contactArea"),

        togglebtns: $("#contactArea .togglebtn"),

        elementCusno: $("#customerNumber"),

        name: {
            area: $("#nameArea"),
            form: $("#nameform"),
            element: $("#fullname"),
            elementInputs: $("#nameform input"),
            elementFirstName: $("#nameform #firstNameInput"),
            elementLastName: $("#nameform #lastNameInput"),
            submit: $("#saveName"),
            edit: $("#nameArea .edit"),
            errors: {
                update: "<div class='alert alert-danger'>Det gick inte att uppdatera ditt namn.</div>",
            }
        },

        phone: {
            area: $("#phoneArea"),
            form: $("#phoneform"),
            element: $("#phone"),
            elementPhone: $("#phoneform #phoneInput"),
            elementInputs: $("#phoneform input"),
            submit: $("#savePhone"),
            edit: $("#phoneArea .edit"),
            errors: {
                update: "<div class='alert alert-danger'>Det gick inte att uppdatera ditt telefonnummer.</div>",
            }
        },

        email: {
            area: $("#emailArea"),
            form: $("#emailform"),
            element: $("#email"),
            elementEmail: $("#emailform #emailInput"),
            elementInputs: $("#emailform input"),
            submit: $("#saveEmail"),
            edit: $("#emailArea .edit"),
            errors: {
                update: "<div class='alert alert-danger'>Det gick inte att uppdatera din e-postadress.</div>",
            }
        }
    }   
});