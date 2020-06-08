define(["jquery"], function ($) {
    return {
        tabContainer: $("#subscription-tabs"),
        collapsibles: $("#subscriptions-info .panel-collapse"),
        subscriptionInfoContainer: $("#subscriptions-info"),

        subscriptionInfo : {
            subscriptionNumber: $("#subsinfo-id"),
            startDate: $("#subsinfo-startdate"),
            endDate: $("#subsinfo-enddate"),
            collapseHeading: $("#infoHeading"),
            collapsePanel: $("#infoCollapse"),
        },

        reclaim: {
            collapseHeading: $("#reclaimHeading"),
            collapsePanel: $("#reclaimCollapse"),
            area: $("#reclaimArea"),
            form: $("#reclaimArea #reclaimform"),
            elementSubsNoInput: $("#reclaimform input#subscriptionId"),
            submit: $("#reclaimform #sendReclaim"),
            ph: $("#reclaimform #reclaim-details"),
            errors: {
                load: "<div class='alert alert-danger'>Just nu kan vi inte ta emot reklamationer.</div>",
                save: "<div class='alert alert-danger'>Din anmälan kunde inte sparas. Vänligen försök igen.</div>",
            },
            feedback: {
                save: "<div class='alert alert-success'>Din anmälan har skickats.</div>",
            }
        },
        subssleep: {
            collapseHeading: $("#sleepHeading"),
            collapsePanel: $("#sleepCollapse"),
            area: $("#sleepArea"),
            form: $("#sleepArea #sleepform"),
            elementSubsNoInput: $("#sleepform input#subscriptionId"),
            submit: $("#sleepArea #sendSleep"),
            ph: $("#sleepArea #subsSleeps"),                      
            selectors: {
                deletebtn: ".deleteBtn",
                savebtn: ".saveBtn",
                togglebtn: ".editBtn, .undoBtn",
                dateinput: ".dateinput",
            },
            errors: {
                save: "<div class='alert alert-danger'>Ditt uppehåll kunde inte sparas. Kontrollera:<br />- Att datumintervallet inte kolliderar med tidigare sparat uppehåll.<br />- Fråndatum kan inte vara ett tidigare datum än tilldatum.<br />- Uppehållet får max vara 135 dagar långt.</div>",
                load: "<div class='alert alert-danger'>Just nu kan vi inte visa dina kommande uppehåll.</div>",
                remove: "<div class='alert alert-danger'>Det gick inte att ta bort ditt uppehåll.</div>"
            }            
        },
        tmpaddress: {
            collapseHeading: $("#tmpaddressHeading"),
            collapsePanel: $("#tmpaddressCollapse"),
            area: $("#tmpAddressArea"),
            form: $("#tmpAddressArea #tmpAddressForm"),
            elementSubsNoInput: $("#tmpAddressForm input#subscriptionId"),
            submit: $("#tmpAddressArea #submitTmpAddressForm"),
            ph: $("#tmpAddressArea #tmpAddressChanges"),
            phAddresses: $("#tmpAddressArea #tmpAddresses"),
            dateinputs: "#tmpAddressForm .dateinput",

            forminputs: {
                streetaddress: $("#tmpAddressForm #streetaddresstmpinput"),
                streetnumber: $("#tmpAddressForm #streetnotmpinput"),
                zip: $("#tmpAddressForm #ziptmpinput"),
                city: $("#tmpAddressForm #citytmpinput"),
                all: $("#tmpAddressForm .form-control")
            },

            selectors: {
                deletebtn: ".deleteBtn",
                savebtn: ".saveBtn",
                togglebtn: ".editBtn, .undoBtn",
                dateinput: ".dateinput",
                tmpaddresses: "#tmpAddresses select"
            },
            errors: {
                save: "<div class='alert alert-danger'>Din addressändring kunde inte sparas. Kontrollera:<br />- Att datumintervallet inte kolliderar med tidigare sparad adressändring.<br />- Fråndatum kan inte vara ett tidigare datum än tilldatum.<br />- Adressändringen får max vara 135 dagar långt.</div>",
                load: "<div class='alert alert-danger'>Just nu kan vi inte visa dina kommande adressändringar.</div>",
                remove: "<div class='alert alert-danger'>Det gick inte att ta bort din adressändring.</div>"
            }

        },
        permaddress: {
            area: $("#addressArea"),
            editAddress: $("#addressArea .edit"),
            newAddressForm: $("#addressform"),
            newAddressFormInputs: $("#addressform input"),
            newAddressFormDateInputs: $("#addressform .dateinput"),
            newAddressFormBtn: $("#addressform #submitPermAddressForm"),
            permAddressChangesPh: $("#permAddressChanges"),

            form: {
                co: $("#coinput"),
                streetAddress: $("#streetaddressinput"),
                streetNo: $("#streetnoinput"),
                stairCase: $("#staircaseinput"),
                stairs: $("#stairsinput"),
                apartmentNo: $("#apartmentnumberinput"),
                zip: $("#zipinput"),
                city: $("#cityinput"),
                fromDate: $("#fromdateinput")
            },

            selectors: {
                togglebtn: "#togglePermAddress",
                deletebtn: "#deletePermBtn",
                editbtn: "#editPermBtn"
            },

            confirm: {
                remove: "Vill du ta bort adressändringen?"
            },
            errors: {
                load: "<div class='alert alert-danger'>Just nu kan vi inte visa dina kommande adressändringar.</div>",
                save: "<div class='alert alert-danger'>Din adressändring kunde inte sparas. Vänligen försök igen.</div>",
                remove: "<div class='alert alert-danger'>Det gick inte att ta bort din adressändring.</div>",
                edit: "<div class='alert alert-danger'>Just nu går det inte att redigera din adressändring.</div>"
            }
        },
        selectors: {
            tabs: "#subscription-tabs a"
        },

        confirm: {
            remove: "Vill du ta bort raden?"
        }
    }
});